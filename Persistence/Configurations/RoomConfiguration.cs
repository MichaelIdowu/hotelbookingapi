using HotelBookingApi.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBookingApi.Persistence.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> b)
    {
        b.HasKey(r => r.Id);
        b.Property(r => r.RoomType).IsRequired().HasMaxLength(20);
        b.Property(r => r.Capacity).IsRequired();
        b.HasMany(r => r.Bookings)
            .WithOne(bk => bk.Room!)
            .HasForeignKey(bk => bk.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        b.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Room_RoomType",
                "RoomType IN ('single','double','deluxe')");
        });
        b.HasIndex(r => new { r.HotelId, r.RoomType });
    }
}
