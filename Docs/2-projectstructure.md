
CODE (SOLUTION) DESIGN:

HotelBookingApi
├── Controllers
├── Domain         
│   ├── Hotel.cs
│   ├── Room.cs
│   └── Booking.cs
├── Persistence
├── Services
├── DTOs
├── Program.cs
├── Startup.cs
└── appsettings.json



DOMAIN-DRIVEN DESIGN (DDD):

HotelBookingApi/
├── Domain/          <-- Core business entities & rules
│   ├── Hotel.cs
│   ├── Room.cs
│   └── Booking.cs
├── DTOs/            <-- Request/Response shapes for API
│   ├── BookingRequestDto.cs
│   └── BookingResponseDto.cs
├── Persistence/     <-- EF Core DbContext, configurations
├── Services/
├── Controllers/

DOCUMENTATION:

HotelBookingApi/
  Controllers/
  Domain/
  DTOs/
  Persistence/
    Configurations/
  Services/
  Program.cs
  Dockerfile
  Docs/
    *.md <-- Set of Documentation Files


