using Cw3.Models;

namespace Cw3.Data;

public static class DataStore
{
    public static readonly List<Room> Rooms = new();
    public static readonly List<Reservation> Reservations = new();

    private static int _nextRoomId = 1;
    private static int _nextReservationId = 1;

    static DataStore()
    {
        Rooms.Add(new Room
        {
            Id = _nextRoomId++,
            Name = "Lab 101",
            BuildingCode = "A",
            Floor = 1,
            Capacity = 20,
            HasProjector = true,
            IsActive = true
        });
        Rooms.Add(new Room
        {
            Id = _nextRoomId++,
            Name = "Lab 204",
            BuildingCode = "B",
            Floor = 2,
            Capacity = 24,
            HasProjector = true,
            IsActive = true
        });
        Rooms.Add(new Room
        {
            Id = _nextRoomId++,
            Name = "Aula",
            BuildingCode = "A",
            Floor = 0,
            Capacity = 120,
            HasProjector = true,
            IsActive = true
        });
        Rooms.Add(new Room
        {
            Id = _nextRoomId++,
            Name = "Sala 322",
            BuildingCode = "C",
            Floor = 3,
            Capacity = 12,
            HasProjector = false,
            IsActive = true
        });
        Rooms.Add(new Room
        {
            Id = _nextRoomId++,
            Name = "Sala 122",
            BuildingCode = "C",
            Floor = 1,
            Capacity = 15,
            HasProjector = false,
            IsActive = false
        });

        Reservations.Add(new Reservation
        {
            Id = _nextReservationId++,
            RoomId = 1,
            OrganizerName = "Jan Nowak",
            Topic = "Wprowadzenie do REST",
            Date = new DateOnly(2026, 5, 10),
            StartTime = new TimeOnly(8, 0),
            EndTime = new TimeOnly(9, 30),
            Status = "confirmed"
        });
        Reservations.Add(new Reservation
        {
            Id = _nextReservationId++,
            RoomId = 2,
            OrganizerName = "Anna Kowalska ",
            Topic = "Warsztaty z HTTP i REST",
            Date = new DateOnly(2026, 5, 10),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(12, 30),
            Status = "confirmed"
        });
        Reservations.Add(new Reservation
        {
            Id = _nextReservationId++,
            RoomId = 3,
            OrganizerName = "Piotr Zielinski",
            Topic = "Wyklad",
            Date = new DateOnly(2026, 5, 12),
            StartTime = new TimeOnly(18, 0),
            EndTime = new TimeOnly(20, 0),
            Status = "planned"
        });
        Reservations.Add(new Reservation
        {
            Id = _nextReservationId++,
            RoomId = 2,
            OrganizerName = "Maria Wisniewska",
            Topic = "Konsultacje projektowe",
            Date = new DateOnly(2026, 5, 11),
            StartTime = new TimeOnly(14, 0),
            EndTime = new TimeOnly(15, 0),
            Status = "cancelled"
        });
        Reservations.Add(new Reservation
        {
            Id = _nextReservationId++,
            RoomId = 4,
            OrganizerName = "Tomasz Lis",
            Topic = "Spotkanie kola naukowego",
            Date = new DateOnly(2026, 5, 13),
            StartTime = new TimeOnly(17, 0),
            EndTime = new TimeOnly(19, 0),
            Status = "planned"
        });
    }

    public static int GetNextRoomId() => _nextRoomId++;
    public static int GetNextReservationId() => _nextReservationId++;
}