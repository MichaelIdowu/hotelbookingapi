using HotelBookingApi.Domain;
using System.Net.NetworkInformation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HotelBookingApi.DTOs;

// Using a record keeps it lightweight.
// We’ll bind it from query string (e.g., GET /api/hotels/search? name = DMI).
public record HotelSearchDto(string Name);
