using HotelBookingApi.Domain;
using Microsoft.EntityFrameworkCore;
using System.Composition;
using System.Drawing;

namespace HotelBookingApi.Persistence;

/// <summary>
/// Primary EF Core context for the Hotel Booking API.
/// - Uses per-entity configuration classes under Persistence/Configurations (EF Core 8 style),
///   including check constraints registered via ToTable(...).
/// - Exposes DbSets for core aggregates (Hotel, Room, Booking).
/// - Keeps the context intentionally minimal; provider selection and connection strings
///   are supplied via DI in Program.cs (or a design-time factory for migrations).
/// </summary>
public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        // Consider setting a predictable query tracking behaviour if your read paths are hot:
        // ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        // (Leave it default if you mix reads and writes in the same scope.)
    }

    // --- DbSets (strongly typed, discoverable) ---
    public DbSet<Hotel> Hotel => Set<Hotel>();
    public DbSet<Room> Room => Set<Room>();
    public DbSet<Booking> Booking => Set<Booking>();

    /// <summary>
    /// Central place to apply model configuration. We delegate to the per-entity
    /// configuration classes (IEntityTypeConfiguration&lt;T&gt;) in Persistence/Configurations.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all IEntityTypeConfiguration<T> in this assembly.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // If you adopt any global conventions (e.g., default string lengths, UTC DateTime),
        // prefer ConfigureConventions below (EF Core 7+), or keep it here if it needs the model.
        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Optional global conventions. Keep this light and provider-agnostic.
    /// </summary>
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // Example: If you wanted a global max length for strings, you could uncomment:
        // configurationBuilder.Properties<string>().HaveMaxLength(200);

        // EF Core 8 supports DateOnly/TimeOnly natively across major providers,
        // so no converters are registered here.
        base.ConfigureConventions(configurationBuilder);
    }
}

//DEPRECATED CODE:
//public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
//    : DbContext(options)
//{
//    //This (ApplicationDbContext) must expose a DbSet<Hotel> property, 
//    //for db.Hotels (in DataSeeder) calls to work

//    public DbSet<Hotel> Hotels => Set<Hotel>();
//    public DbSet<Room> Rooms => Set<Room>();
//    public DbSet<Booking> Bookings => Set<Booking>();

//    //option B keeps code minimal: it replaces option A which is less scalable
//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
//        base.OnModelCreating(modelBuilder);
//    }


//option A
//protected override void OnModelCreating(ModelBuilder modelBuilder)
//{
// (We’ll fill this out below in Option A or keep it minimal for Option B)
//    // HOTEL
//    modelBuilder.Entity<Hotel>(b =>
//    {
//        b.HasKey(h => h.Id);
//        b.Property(h => h.Name)
//            .IsRequired()
//            .HasMaxLength(200);

//        b.HasMany(h => h.Rooms)
//            .WithOne(r => r.Hotel!)
//            .HasForeignKey(r => r.HotelId)
//            .OnDelete(DeleteBehavior.Cascade);

//        b.HasIndex(h => h.Name); // supports “find by name”
//    });

//    // ROOM
//    modelBuilder.Entity<Room>(b =>
//    {
//        b.HasKey(r => r.Id);
//        b.Property(r => r.RoomType)
//            .IsRequired()
//            .HasMaxLength(20);

//        b.Property(r => r.Capacity)
//            .IsRequired();

//        b.HasMany(r => r.Bookings)
//            .WithOne(bk => bk.Room!)
//            .HasForeignKey(bk => bk.RoomId)
//            .OnDelete(DeleteBehavior.Cascade);

//        // Optional: constrain allowed room types at DB level
//        b.HasCheckConstraint("CK_Room_RoomType",
//            "RoomType IN ('single','double','deluxe')");

//        b.HasIndex(r => new { r.HotelId, r.RoomType });
//    });

//    // BOOKING
//    modelBuilder.Entity<Booking>(b =>
//    {
//        b.HasKey(x => x.Id);

//        b.Property(x => x.BookingReference)
//            .IsRequired()
//            .HasMaxLength(32);

//        b.HasIndex(x => x.BookingReference)
//            .IsUnique(); // business requirement: unique reference

//        b.Property(x => x.GuestCount).IsRequired();

//        // DateOnly is supported by EF Core 8 providers; no converter needed in most cases.
//        // Add guards against invalid ranges/capacity at DB level too:
//        b.HasCheckConstraint("CK_Booking_DateRange", "\"EndDate\" > \"StartDate\"");
//        b.HasCheckConstraint("CK_Booking_Guests", "\"GuestCount\" > 0");
//    });

//    base.OnModelCreating(modelBuilder);
//}

//}