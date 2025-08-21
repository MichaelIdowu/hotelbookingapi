using FluentValidation;
using HotelBookingApi.DTOs;

namespace HotelBookingApi.DTOs;

public class HotelSearchValidator : AbstractValidator<HotelSearchDto>
{
    public HotelSearchValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name query must be provided.")
            .MinimumLength(2).WithMessage("Search term must be at least 2 characters long.");
    }
}
