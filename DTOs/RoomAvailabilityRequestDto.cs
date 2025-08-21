namespace HotelBookingApi.DTOs;

public record RoomAvailabilityRequestDto(
    int HotelId,
    string RoomType,
    int Guests,
    DateOnly StartDate,
    DateOnly EndDate
);
