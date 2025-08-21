RESTful API Endpoints
(to ensure proper DTOs and mapping to domain models).

Endpoint					Verb				Description
/api/hotels?name=X				GET				Search hotel
/api/rooms/available?start=...&end=...&guests=N	GET				Available rooms
/api/bookings				POST				Book a room
/api/bookings/{reference}			GET				Retrieve booking
/api/data/seed				POST				Seed data
/api/data/reset				POST				Reset database

