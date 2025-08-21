using FluentValidation;
using HotelBookingApi.DTOs; //Data Transfer Objects

namespace HotelBookingApi.DTOs;

public class BookingRequestValidator : AbstractValidator<BookingRequestDto>
{
    public BookingRequestValidator()
    {
        RuleFor(x => x.HotelId)
            .GreaterThan(0).WithMessage("HotelId must be greater than zero.");

        RuleFor(x => x.RoomType)
            .NotEmpty().WithMessage("RoomType is required.")
            .Must(t => new[] { "single", "double", "deluxe" }.Contains(t))
            .WithMessage("RoomType must be one of: single, double, deluxe.");

        RuleFor(x => x.GuestCount)
            .GreaterThan(0).WithMessage("GuestCount must be greater than zero.");

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate).WithMessage("StartDate must be before EndDate.");
    }
}
