using HotelBookingApi.DTOs;
using HotelBookingApi.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Controllers;

[ApiController]
//[Route("api/hotels")]   //Hard-coded
[Route("api/[controller]")] //Token-based
public class HotelsController(ApplicationDbContext db) : ControllerBase
{
    /// <summary>Find hotels whose names contain the given string (case-insensitive).</summary>
    [HttpGet]
    public async Task<IActionResult> Find([FromQuery] string name, CancellationToken ct)
    {
        //var hotels = await db.Hotels
        //    .Where(h => EF.Functions.Like(h.Name, $"%{name}%"))
        //    .ToListAsync(ct);

        //return Ok(hotels.Select(h => new { h.Id, h.Name }));

        if (string.IsNullOrWhiteSpace(name))
            return BadRequest("Query parameter 'name' is required.");

        var hotels = await db.Hotel
            .Where(h => EF.Functions.Like(h.Name, $"%{name}%"))
            .Select(h => new { h.Id, h.Name })
            .ToListAsync(ct);

        return Ok(hotels);
    }

    //If we want FluentValidation to handle query param validation in a strongly-typed way
    // - this option uses data transfer object
    /// <summary>
    /// Search hotels by (partial, case-insensitive) name via query string: /api/hotels/search?name=Sun
    /// </summary>
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] HotelSearchDto dto, CancellationToken ct)
    {
        // If FluentValidation fails, this action is never reached (400 returned automatically).
        var hotels = await db.Hotel
            .Where(h => EF.Functions.Like(h.Name, $"%{dto.Name}%"))
            .Select(h => new { h.Id, h.Name })
            .ToListAsync(ct);

        return Ok(hotels);
    }

}
