CONCRETE TEST IDEAS: 
(Map to Business Rules)

Availability

	Empty set when all rooms booked; non-empty when at least one free.

	Edge: end == start + 1 (1-night stay).

	Edge: end == other.StartDate (no overlap).

	Edge: start == other.EndDate (no overlap).

Capacity

	Reject booking guests > capacity.

	Accept booking guests == capacity.

Uniqueness

	Booking reference format BKG-XXXXXXXX.

	References are unique across many creates (100–1000).

No room switching

	Attempt to book across dates that would need a split → ensure the API approach (single room search) prevents this by design.

Validation

	All FluentValidation rules return 400 with field errors.

Faults

	Missing hotel → 404 (if you expose GET /hotels/{id}).

	Invalid room type → 400.

Concurrency (optional advanced)

	Parallel bookings on same room/type/dates → exactly one succeeds; others 409/400.


Validators
// BookingRequestValidator
	RuleFor(x => x.HotelId).GreaterThan(0);
	RuleFor(x => x.RoomType).NotEmpty().Must(t => new[] {"single","double","deluxe"}.Contains(t.ToLower()));
	RuleFor(x => x.GuestCount).GreaterThan(0);
	RuleFor(x => x.StartDate).LessThan(x => x.EndDate);

// RoomAvailabilityRequestValidator
	RuleFor(x => x.HotelId).GreaterThan(0);
	RuleFor(x => x.RoomType).NotEmpty().Must(t => new[] {"single","double","deluxe"}.Contains(t.ToLower()));
	RuleFor(x => x.Guests).GreaterThan(0);
	RuleFor(x => x.StartDate).LessThan(x => x.EndDate);

// HotelSearchValidator
	RuleFor(x => x.Name).NotEmpty().MinimumLength(2);
