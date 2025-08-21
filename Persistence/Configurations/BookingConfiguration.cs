using HotelBookingApi.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBookingApi.Persistence.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.BookingReference).IsRequired().HasMaxLength(32);
        b.HasIndex(x => x.BookingReference).IsUnique();
        b.Property(x => x.GuestCount).IsRequired();

        // NEW: put check constraints on the TableBuilder
        b.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Booking_DateRange", "\"EndDate\" > \"StartDate\"");
            t.HasCheckConstraint("CK_Booking_Guests", "\"GuestCount\" > 0");
        });

        // OLD: ...
        //b.HasCheckConstraint("CK_Booking_DateRange", "\"EndDate\" > \"StartDate\"");
        //b.HasCheckConstraint("CK_Booking_Guests", "\"GuestCount\" > 0");
    }
}
