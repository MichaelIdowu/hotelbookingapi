using System.Threading;
using System.Threading.Tasks;

namespace HotelBookingApi.Services;

public interface IDataSeeder
{
    Task SeedAsync(bool includeSampleBooking = false, CancellationToken ct = default);
    Task ResetAsync(CancellationToken ct = default);
}

//The above strategy is a more robust, strongly-typed seeder
//that uses your DbSet<T> 
// - previous implementation (below) required some adjustments

//namespace HotelBookingApi.Services;
public interface IDataSeeder_NonRobust
{
    Task SeedAsync(CancellationToken ct = default);
    Task ResetAsync(CancellationToken ct = default);
}