Run the “happy-path” flow in Swagger

Seed data

	POST /api/v1/admin/maintenance/seed

	Try it out → Execute → expect 200 OK.

Search hotel

	GET /api/v1/hotels/search?name=DMI

	Expect 200 OK, note hotelId.

Check availability

	GET /api/v1/hotels/{hotelId}/rooms/available

	roomType=double, guests=2, startDate=2030-01-10, endDate=2030-01-12

	Expect 200 OK with matching rooms.

Create booking

	POST /api/v1/bookings

	Body:

	{
	  "hotelId": 1,
	  "roomType": "double",
	  "guestCount": 2,
	  "startDate": "2030-01-10",
	  "endDate": "2030-01-12"
	}


	Expect 201 Created with a BookingReference.

Get booking

	GET /api/v1/bookings/{reference}

	Expect 200 OK with booking details.