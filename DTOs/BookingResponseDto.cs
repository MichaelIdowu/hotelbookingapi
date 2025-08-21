namespace HotelBookingApi.DTOs;

public record BookingResponseDto(
    string BookingReference,
    int RoomId,
    int GuestCount,
    DateOnly StartDate,
    DateOnly EndDate
);
