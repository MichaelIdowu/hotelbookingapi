using HotelBookingApi.Domain;
using HotelBookingApi.DTOs;
using HotelBookingApi.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Controllers;


[ApiController]
//[Route("api/bookings")]
[Route("api/[controller]")]

public class BookingsController(ApplicationDbContext db) : ControllerBase
{
    //if (req.EndDate <= req.StartDate) return BadRequest("End must be after start.");

    //var room = await ...
    //if (room is null) return BadRequest("No available room for that range/capacity.");

    //var reference ...
    //db.Bookings.Add(booking);
    //await db.SaveChangesAsync(ct);

    //return Ok(new BookingResponseDto(reference, room.Id, req.GuestCount, req.StartDate, req.EndDate));

    /// <summary>Create a booking. EndDate is exclusive (i.e., nights = End - Start).</summary>
    [HttpPost]
    public async Task<IActionResult> Book([FromBody] BookingRequestDto req, CancellationToken ct)
    {
        if (req is null) return BadRequest("Request body is required.");
        if (req.HotelId <= 0) return BadRequest("HotelId must be > 0.");
        if (string.IsNullOrWhiteSpace(req.RoomType)) return BadRequest("RoomType is required.");
        if (req.GuestCount <= 0) return BadRequest("GuestCount must be > 0.");
        if (req.EndDate <= req.StartDate) return BadRequest("EndDate must be after StartDate.");

        // Find an available room that meets capacity (best-fit by capacity)
        var room = await db.Room
            .Where(r => r.HotelId == req.HotelId &&
                        r.RoomType == req.RoomType &&
                        r.Capacity >= req.GuestCount)
            .Where(r => !r.Bookings.Any(b => !(b.EndDate <= req.StartDate || b.StartDate >= req.EndDate)))
            .OrderBy(r => r.Capacity) // simplest “best fit”
            .FirstOrDefaultAsync(ct);

        if (room is null)
            return BadRequest("No available room for the requested dates/capacity.");

        // Create booking with unique reference
        var reference = $"BKG-{Guid.NewGuid().ToString()[..8].ToUpper()}";
        var booking = new Booking
        {
            BookingReference = reference,
            RoomId = room.Id,
            GuestCount = req.GuestCount,
            StartDate = req.StartDate,
            EndDate = req.EndDate
        };

        db.Booking.Add(booking);
        await db.SaveChangesAsync(ct);

        var dto = new BookingResponseDto(reference, room.Id, req.GuestCount, req.StartDate, req.EndDate);
        return CreatedAtAction(nameof(GetByReference), new { reference }, dto);
    }

    /// <summary>Get booking details by booking reference.</summary>
    [HttpGet("{reference}")]
    //public async Task<IActionResult> GetByRef(string reference, CancellationToken ct)
    //{
    //var b = await db.Bookings.Include ...
    //if (b is null) ...

    //return Ok(new ...
    //}
    public async Task<IActionResult> GetByReference([FromRoute] string reference, CancellationToken ct)
    {
        

        if (string.IsNullOrWhiteSpace(reference)) return BadRequest("reference is required.");

        var b = await db.Booking
            .Include(x => x.Room)
            .ThenInclude(r => r!.Hotel)
            .FirstOrDefaultAsync(x => x.BookingReference == reference, ct);

        if (b is null) return NotFound();

        return Ok(new
        {
            b.BookingReference,
            b.GuestCount,
            b.StartDate,
            b.EndDate,
            Room = new { b.RoomId, b.Room!.RoomType, b.Room.Capacity },
            Hotel = new { b.Room!.HotelId, b.Room!.Hotel!.Name }
        });
    }
}
