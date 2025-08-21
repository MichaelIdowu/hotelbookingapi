FINAL DEMO PLAN:

POST /api/v1/admin/maintenance/seed

GET /api/v1/hotels/search?name=Sun → note hotelId

GET /api/v1/hotels/{hotelId}/rooms/available?roomType=double&guests=2&startDate=2030-01-10&endDate=2030-01-12

POST /api/v1/bookings (body in README)

GET /api/v1/bookings/{reference}

Re-check availability → one fewer room

(Optional) POST /api/v1/admin/maintenance/reset