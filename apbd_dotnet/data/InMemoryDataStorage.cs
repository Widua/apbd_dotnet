using apbd_dotnet.models;

namespace apbd_dotnet.data;

public class InMemoryDataStorage 
{
    public static List<Room> Rooms { get; set; } = new List<Room>
        {
            new Room { Id = 1, Name = "Sala A", BuildingCode = "A", Floor = 1, Capacity = 15, HasProjector = true, IsActive = true },
            new Room { Id = 2, Name = "Lab 204", BuildingCode = "B", Floor = 2, Capacity = 24, HasProjector = true, IsActive = true },
            new Room { Id = 3, Name = "Sala Wykładowa", BuildingCode = "A", Floor = 0, Capacity = 100, HasProjector = true, IsActive = true },
            new Room { Id = 4, Name = "Salka Spotkań", BuildingCode = "C", Floor = 3, Capacity = 4, HasProjector = false, IsActive = true },
            new Room { Id = 5, Name = "Archwium (Remont)", BuildingCode = "B", Floor = -1, Capacity = 10, HasProjector = false, IsActive = false }
        };

        public static List<Reservation> Reservations { get; set; } = new List<Reservation>
        {
            new Reservation { Id = 1, RoomId = 2, OrganizerName = "Anna Kowalska", Topic = "Warsztaty z HTTP i REST", Date = new DateOnly(2026, 5, 10), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(12, 30), Status = "confirmed" },
            new Reservation { Id = 2, RoomId = 1, OrganizerName = "Jan Nowak", Topic = "Konsultacje C#", Date = new DateOnly(2026, 5, 10), StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(11, 0), Status = "planned" },
            new Reservation { Id = 3, RoomId = 3, OrganizerName = "Michał Wiśniewski", Topic = "Wykład AI", Date = new DateOnly(2026, 5, 12), StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(16, 0), Status = "confirmed" },
            new Reservation { Id = 4, RoomId = 4, OrganizerName = "Jan Paweł", Topic = "Spotkanie Zarządu", Date = new DateOnly(2026, 5, 15), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(11, 0), Status = "cancelled" }
        };
}