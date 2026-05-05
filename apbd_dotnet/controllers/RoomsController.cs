using apbd_dotnet.data;
using apbd_dotnet.models;
using Microsoft.AspNetCore.Mvc;

namespace apbd_dotnet.controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetRooms([FromQuery] int? minCapacity, [FromQuery] bool? hasProjector,
        [FromQuery] bool? activeOnly)
    {
        var query = InMemoryDataStorage.Rooms.AsQueryable();

        if (minCapacity.HasValue)
            query = query.Where(r => r.Capacity >= minCapacity.Value);

        if (hasProjector.HasValue)
            query = query.Where(r => r.HasProjector == hasProjector.Value);

        if (activeOnly.HasValue && activeOnly.Value)
            query = query.Where(r => r.IsActive);

        return Ok(query.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult GetRoomById(int id)
    {
        var room = InMemoryDataStorage.Rooms.FirstOrDefault(r => r.Id == id);
        if (room == null) return NotFound();

        return Ok(room);
    }

    [HttpGet("building/{buildingCode}")]
    public IActionResult GetRoomsByBuilding(string buildingCode)
    {
        var rooms = InMemoryDataStorage.Rooms
            .Where(r => r.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Ok(rooms);
    }

    [HttpPost]
    public IActionResult CreateRoom([FromBody] Room room)
    {
        room.Id = InMemoryDataStorage.Rooms.Any() ? InMemoryDataStorage.Rooms.Max(r => r.Id) + 1 : 1;
        InMemoryDataStorage.Rooms.Add(room);

        return CreatedAtAction(nameof(GetRoomById), new { id = room.Id }, room);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateRoom(int id, [FromBody] Room updatedRoom)
    {
        var room = InMemoryDataStorage.Rooms.FirstOrDefault(r => r.Id == id);
        if (room == null) return NotFound();

        room.Name = updatedRoom.Name;
        room.BuildingCode = updatedRoom.BuildingCode;
        room.Floor = updatedRoom.Floor;
        room.Capacity = updatedRoom.Capacity;
        room.HasProjector = updatedRoom.HasProjector;
        room.IsActive = updatedRoom.IsActive;

        return Ok(room);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteRoom(int id)
    {
        var room = InMemoryDataStorage.Rooms.FirstOrDefault(r => r.Id == id);
        if (room == null) return NotFound();

        bool hasReservations = InMemoryDataStorage.Reservations.Any(r => r.RoomId == id);
        if (hasReservations)
        {
            return Conflict("Nie można usunąć sali, ponieważ istnieją powiązane z nią rezerwacje.");
        }

        InMemoryDataStorage.Rooms.Remove(room);
        return NoContent();
    }
}