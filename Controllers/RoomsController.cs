using HotelBookingApi.Domain;
using HotelBookingApi.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Controllers;

[ApiController]
//[Route("api/rooms")]
[Route("api/[controller]")]

public class RoomsController(ApplicationDbContext db) : ControllerBase
{
    //var rooms = await db.Rooms
    //         .Where(r => r.HotelId == hotelId && r.RoomType == roomType && r.Capacity >= guests)
    //         .Where(r => !r.Bookings.Any(b =>
    //             // overlap check with End exclusive
    //             !(b.EndDate <= start || b.StartDate >= end)))
    //         .Select(r => new { r.Id, r.RoomType, r.Capacity })
    //         .ToListAsync(ct);

    //    return Ok(rooms);

    /// <summary>
    /// Find available rooms in a hotel for a date range (End exclusive) and guest count.
    /// </summary>
    [HttpGet("available")]
    public async Task<IActionResult> Available(
        [FromQuery] int hotelId,
        [FromQuery] string roomType,
        [FromQuery] int guests,
        [FromQuery] DateOnly start,
        [FromQuery] DateOnly end,
        CancellationToken ct)
    {
        if (hotelId <= 0) return BadRequest("hotelId must be > 0.");
        if (string.IsNullOrWhiteSpace(roomType)) return BadRequest("roomType is required.");
        if (guests <= 0) return BadRequest("guests must be > 0.");
        if (end <= start) return BadRequest("end must be after start.");

        var rooms = await db.Room
            .Where(r => r.HotelId == hotelId &&
                        r.RoomType == roomType &&
                        r.Capacity >= guests)
            .Where(r => !r.Bookings.Any(b =>
                // overlap check with End exclusive
                !(b.EndDate <= start || b.StartDate >= end)))
            .OrderBy(r => r.Capacity)
            .Select(r => new { r.Id, r.RoomType, r.Capacity })
            .ToListAsync(ct);

        return Ok(rooms);
    }
}
