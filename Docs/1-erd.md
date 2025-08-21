erDiagram
    HOTEL ||--o{ ROOM : "has many"
    ROOM  ||--o{ BOOKING : "has many"

    HOTEL {
      int HotelId PK
      string Name
      string Location "optional"
      string Description "optional"
    }

    ROOM {
      int RoomId PK
      int HotelId FK "→ Hotel.HotelId"
      string RoomType "single | double | deluxe"
      int Capacity
    }

    BOOKING {
      int BookingId PK
      string BookingReference "unique"
      int HotelId FK "→ Hotel.HotelId (denormalised)"
      int RoomId FK "→ Room.RoomId"
      int GuestCount
      date StartDate "DateOnly"
      date EndDate "DateOnly, exclusive"
    }
