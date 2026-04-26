using Cw3.Data;
using Cw3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cw3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll(
        [FromQuery] int? minCapacity,
        [FromQuery] bool? hasProjector,
        [FromQuery] bool? activeOnly)
    {
        IEnumerable<Room> rooms = DataStore.Rooms;

        if (minCapacity.HasValue)
            rooms = rooms.Where(r => r.Capacity >= minCapacity.Value);

        if (hasProjector.HasValue)
            rooms = rooms.Where(r => r.HasProjector == hasProjector.Value);

        if (activeOnly == true)
            rooms = rooms.Where(r => r.IsActive);

        return Ok(rooms);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == id);
        if (room is null)
            return NotFound($"Room with id {id} not found.");

        return Ok(room);
    }

    [HttpGet("building/{buildingCode}")]
    public IActionResult GetByBuilding([FromRoute] string buildingCode)
    {
        var rooms = DataStore.Rooms.Where(r => r.BuildingCode == buildingCode);
        return Ok(rooms);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Room room)
    {
        room.Id = DataStore.GetNextRoomId();
        DataStore.Rooms.Add(room);

        return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update([FromRoute] int id, [FromBody] Room updated)
    {
        var existing = DataStore.Rooms.FirstOrDefault(r => r.Id == id);
        if (existing is null)
            return NotFound($"Room with id {id} not found.");

        existing.Name = updated.Name;
        existing.BuildingCode = updated.BuildingCode;
        existing.Floor = updated.Floor;
        existing.Capacity = updated.Capacity;
        existing.HasProjector = updated.HasProjector;
        existing.IsActive = updated.IsActive;

        return Ok(existing);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == id);
        if (room is null)
            return NotFound($"Room with id {id} not found.");

        var hasReservations = DataStore.Reservations.Any(r => r.RoomId == id);
        if (hasReservations)
            return Conflict($"Cannot delete room {id} because it has reservations.");

        DataStore.Rooms.Remove(room);
        return NoContent();
    }
}