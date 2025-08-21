namespace HotelBookingApi.Domain;

public class Room
{
    public int Id { get; set; }
    public required string RoomType { get; set; } // "single" | "double" | "deluxe"
    public int Capacity { get; set; }
    public int HotelId { get; set; }
    public Hotel? Hotel { get; set; }
    public List<Booking> Bookings { get; set; } = [];
}
