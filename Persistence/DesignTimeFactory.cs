// Persistence/DesignTimeFactory.cs
using HotelBookingApi.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class DesignTimeFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // 1) Prefer env-var override (works great in CI/Docker)
        var cs = Environment.GetEnvironmentVariable("ConnectionStrings__Default");

        // 2) Fallback to local SQLite for dev migrations
        cs ??= "Data Source=hotel.db";

        var opts = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(cs) // swap to UseNpgsql(cs) / UseSqlServer(cs) if that's your provider
            .Options;

        return new ApplicationDbContext(opts);
    }
}
