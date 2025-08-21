using HotelBookingApi.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBookingApi.Persistence.Configurations;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> b)
    {
        b.HasKey(h => h.Id);
        b.Property(h => h.Name).IsRequired().HasMaxLength(200);
        b.HasMany(h => h.Rooms)
            .WithOne(r => r.Hotel!)
            .HasForeignKey(r => r.HotelId)
            .OnDelete(DeleteBehavior.Cascade);
        b.HasIndex(h => h.Name);
    }
}
