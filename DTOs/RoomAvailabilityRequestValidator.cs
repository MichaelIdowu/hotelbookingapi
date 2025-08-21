using FluentValidation;
using HotelBookingApi.DTOs;

namespace HotelBookingApi.DTOs;

public class RoomAvailabilityRequestValidator : AbstractValidator<RoomAvailabilityRequestDto>
{
    public RoomAvailabilityRequestValidator()
    {
        RuleFor(x => x.HotelId)
            .GreaterThan(0).WithMessage("HotelId must be greater than zero.");

        RuleFor(x => x.RoomType)
            .NotEmpty().WithMessage("RoomType is required.")
            .Must(t => new[] { "single", "double", "deluxe" }.Contains(t))
            .WithMessage("RoomType must be one of: single, double, deluxe.");

        RuleFor(x => x.Guests)
            .GreaterThan(0).WithMessage("Guests must be greater than zero.");

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate).WithMessage("StartDate must be before EndDate.");
    }
}
