using Cw3.Data;
using Cw3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cw3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll(
        [FromQuery] DateOnly? date,
        [FromQuery] string? status,
        [FromQuery] int? roomId)
    {
        IEnumerable<Reservation> reservations = DataStore.Reservations;

        if (date.HasValue)
            reservations = reservations.Where(r => r.Date == date.Value);

        if (!string.IsNullOrWhiteSpace(status))
            reservations = reservations.Where(r => r.Status == status);

        if (roomId.HasValue)
            reservations = reservations.Where(r => r.RoomId == roomId.Value);

        return Ok(reservations);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var reservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation is null)
            return NotFound($"Reservation with id {id} not found.");

        return Ok(reservation);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Reservation reservation)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
        if (room is null)
            return NotFound($"Room with id {reservation.RoomId} not found.");

        if (!room.IsActive)
            return Conflict($"Room {room.Id} is not active.");

        var hasConflict = DataStore.Reservations.Any(r =>
            r.RoomId == reservation.RoomId &&
            r.Date == reservation.Date &&
            r.StartTime < reservation.EndTime &&
            r.EndTime > reservation.StartTime);

        if (hasConflict)
            return Conflict("This room is already reserved at the given time.");

        reservation.Id = DataStore.GetNextReservationId();
        DataStore.Reservations.Add(reservation);

        return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update([FromRoute] int id, [FromBody] Reservation updated)
    {
        var existing = DataStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (existing is null)
            return NotFound($"Reservation with id {id} not found.");

        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == updated.RoomId);
        if (room is null)
            return NotFound($"Room with id {updated.RoomId} not found.");

        if (!room.IsActive)
            return Conflict($"Room {room.Id} is not active.");

        var hasConflict = DataStore.Reservations.Any(r =>
            r.Id != id &&
            r.RoomId == updated.RoomId &&
            r.Date == updated.Date &&
            r.StartTime < updated.EndTime &&
            r.EndTime > updated.StartTime);

        if (hasConflict)
            return Conflict("This room is already reserved at the given time.");

        existing.RoomId = updated.RoomId;
        existing.OrganizerName = updated.OrganizerName;
        existing.Topic = updated.Topic;
        existing.Date = updated.Date;
        existing.StartTime = updated.StartTime;
        existing.EndTime = updated.EndTime;
        existing.Status = updated.Status;

        return Ok(existing);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var reservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation is null)
            return NotFound($"Reservation with id {id} not found.");

        DataStore.Reservations.Remove(reservation);
        return NoContent();
    }
}