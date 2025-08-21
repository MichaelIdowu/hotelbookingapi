namespace HotelBookingApi.Domain;

public class Booking
{
    public int Id { get; set; }
    public required string BookingReference { get; set; }
    public int RoomId { get; set; }
    public Room? Room { get; set; }
    public int GuestCount { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; } // exclusive end (recommended)
}
