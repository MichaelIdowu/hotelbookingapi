namespace HotelBookingApi.DTOs;

public record BookingRequestDto(
    int HotelId,
    string RoomType,
    int GuestCount,
    DateOnly StartDate,
    DateOnly EndDate
);
