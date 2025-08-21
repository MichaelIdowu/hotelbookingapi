TRIGGER VALIDATION 
(to see FluentValidation at work in Swagger)

INVALID INPUTS WITH ERRRS:

Empty/short hotel search

	GET /api/v1/hotels/search?name=s → 400

	Error: Name must be at least 2 characters.

Bad availability request

	GET /api/v1/hotels/{hotelId}/rooms/available?roomType=vip&guests=0&startDate=2030-01-12&endDate=2030-01-10

	400 with errors for RoomType, Guests, and StartDate < EndDate.

Bad booking

	POST /api/v1/bookings with guestCount=0 or endDate <= startDate

	400 with exactly the messages you wrote in rules.