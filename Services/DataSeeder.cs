using HotelBookingApi.Domain;
using HotelBookingApi.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis; //for robustness
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

namespace HotelBookingApi.Services;

public class DataSeeder_NonRobust(ApplicationDbContext db) : IDataSeeder_NonRobust
{
    public async Task SeedAsync(CancellationToken ct = default)
    {
        if (await db.Hotel.AnyAsync(ct)) return;

        var hotel = new Hotel { Name = "DMI Hotel" };
        hotel.Rooms.AddRange(new[]
        {
            new Room { RoomType = "single", Capacity = 1 },
            new Room { RoomType = "single", Capacity = 1 },
            new Room { RoomType = "double", Capacity = 2 },
            new Room { RoomType = "double", Capacity = 2 },
            new Room { RoomType = "deluxe", Capacity = 4 },
            new Room { RoomType = "deluxe", Capacity = 4 }
        });

        db.Hotel.Add(hotel);
        await db.SaveChangesAsync(ct);
    }

    public async Task ResetAsync(CancellationToken ct = default)
    {
        db.Booking.RemoveRange(db.Booking);
        db.Room.RemoveRange(db.Room);
        db.Hotel.RemoveRange(db.Hotel);
        await db.SaveChangesAsync(ct);
    }
}


//Notes:
//EnsureCreatedAsync is convenient for dev/test.
//In production, prefer Database.Migrate() or CI/CD migrations.
//ExecuteDeleteAsync issues a server-side DELETE without loading entities (fast & clean).
//The seeder is idempotent: running SeedAsync() multiple times won’t duplicate rooms.

public class DataSeeder : IDataSeeder
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<DataSeeder> _log;

    // Business constants (kept in one place for easy changes)
    private const string DefaultHotelName = "DMI Hotel";
    private static readonly (string Type, int Capacity, int Count)[] RoomPlan =
    [
        ("single", 1, 2),
        ("double", 2, 2),
        ("deluxe", 4, 2)
    ];

    public DataSeeder(ApplicationDbContext db, ILogger<DataSeeder> log)
    {
        _db = db;
        _log = log;
    }

    /// <summary>
    /// Idempotent seeding: creates one hotel with a fixed room plan if it doesn't exist.
    /// Optionally inserts a sample booking to exercise availability logic.
    /// </summary>
    public async Task SeedAsync(bool includeSampleBooking = false, CancellationToken ct = default)
    {
        // Ensure database exists (prefer Migrations in real environments)
        await _db.Database.EnsureCreatedAsync(ct);

        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        // 1) Ensure the hotel exists
        var hotel = await _db.Hotel
            .Include(h => h.Rooms)
            .SingleOrDefaultAsync(h => h.Name == DefaultHotelName, ct);

        if (hotel is null)
        {
            hotel = new Hotel { Name = DefaultHotelName, Rooms = new List<Room>() };
            _db.Hotel.Add(hotel);
            await _db.SaveChangesAsync(ct); // get Hotel.Id
        }

        // 2) Ensure rooms exist as per RoomPlan (idempotent top-up)
        await EnsureRoomsAsync(hotel, ct);

        // 3) Optionally add a sample booking (only if none exists)
        if (includeSampleBooking)
        {
            await EnsureSampleBookingAsync(hotel.Id, ct);
        }

        await tx.CommitAsync(ct);

        _log.LogInformation("Seeding complete. HotelId={HotelId}, Rooms={RoomCount}", hotel.Id, hotel.Rooms.Count);
    }

    /// <summary>
    /// Clears all data in dependency order using ExecuteDeleteAsync (EF 7+).
    /// </summary>
    public async Task ResetAsync(CancellationToken ct = default)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        // Delete children -> parents
        await _db.Booking.ExecuteDeleteAsync(ct);
        await _db.Room.ExecuteDeleteAsync(ct);
        await _db.Hotel.ExecuteDeleteAsync(ct);

        await tx.CommitAsync(ct);

        _log.LogInformation("Reset complete: all data removed.");
    }

    // ---------- helpers ----------

    private async Task EnsureRoomsAsync(Hotel hotel, CancellationToken ct)
    {
        // Load current room counts grouped by type
        var current = hotel.Rooms
            .GroupBy(r => r.RoomType)
            .ToDictionary(g => g.Key, g => g.Count(), StringComparer.OrdinalIgnoreCase);

        foreach (var (type, capacity, requiredCount) in RoomPlan)
        {
            current.TryGetValue(type, out var existingCount);
            var toAdd = requiredCount - existingCount;

            if (toAdd <= 0) continue;

            var newRooms = Enumerable.Range(0, toAdd)
                .Select(_ => new Room
                {
                    RoomType = type,
                    Capacity = capacity,
                    HotelId = hotel.Id
                });

            _db.Room.AddRange(newRooms);
        }

        await _db.SaveChangesAsync(ct);

        // Refresh hotel rooms if we need to use hotel.Rooms later
        await _db.Entry(hotel).Collection(h => h.Rooms).LoadAsync(ct);
    }

    private async Task EnsureSampleBookingAsync(int hotelId, CancellationToken ct)
    {
        // If any booking exists, skip
        var exists = await _db.Booking.AnyAsync(ct);
        if (exists) return;

        // Pick a double room and book it for 2 nights in the future
        var room = await _db.Room
            .Where(r => r.HotelId == hotelId && r.RoomType == "double")
            .OrderBy(r => r.Id)
            .FirstOrDefaultAsync(ct);

        if (room is null) return;

        var start = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(14));
        var end = start.AddDays(2); // end exclusive

        var booking = new Booking
        {
            BookingReference = $"BKG-{Guid.NewGuid().ToString()[..8].ToUpper()}",
            RoomId = room.Id,
            GuestCount = 2,
            StartDate = start,
            EndDate = end
        };

        _db.Booking.Add(booking);
        await _db.SaveChangesAsync(ct);
    }
}
