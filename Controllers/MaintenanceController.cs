using HotelBookingApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingApi.Controllers;

[ApiController]
//[Route("api/maintenance")]
[Route("api/[controller]")]

public class MaintenanceController(IDataSeeder seeder) : ControllerBase
{
    
    /// <summary>Populate the DB with minimal test data (idempotent).</summary>
    [HttpPost("seed")]
    public async Task<IActionResult> Seed([FromQuery] bool sampleBooking = false, CancellationToken ct = default)
    {
        await seeder.SeedAsync(sampleBooking, ct);
        return Ok(new { status = "seeded", sampleBooking });
    }

    /// <summary>Remove all data to a clean state.</summary>
    [HttpPost("reset")]
    public async Task<IActionResult> Reset(CancellationToken ct = default)
    {
        await seeder.ResetAsync(ct);
        return Ok(new { status = "reset" });
    }
}

// PS:
// POST /api/maintenance/reset
// POST /api/maintenance/seed? sampleBooking = true

//DEPRECATED
//public class MaintenanceController(IDataSeeder_NonRobust seeder) : ControllerBase
//{
//    //[HttpPost("seed")]
//    //public async Task<IActionResult> Seed(CancellationToken ct)
//    //{ await seeder.SeedAsync(ct); return Ok(); }

//    /// <summary>Populate the DB with minimal test data (idempotent).</summary>
//    [HttpPost("seed")]
//    public async Task<IActionResult> Seed(CancellationToken ct)
//    {
//        await seeder.SeedAsync(ct);
//        return Ok(new { status = "seeded" });
//    }


//    //[HttpPost("reset")]
//    //public async Task<IActionResult> Reset(CancellationToken ct) 
//    //{ await seeder.ResetAsync(ct); return Ok(); }

//    /// <summary>Remove all data to a clean state.</summary>
//    [HttpPost("reset")]
//    public async Task<IActionResult> Reset(CancellationToken ct)
//    {
//        await seeder.ResetAsync(ct);
//        return Ok(new { status = "reset" });
//    }
//}
