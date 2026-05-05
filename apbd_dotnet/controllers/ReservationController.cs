using apbd_dotnet.data;
using apbd_dotnet.models;
using Microsoft.AspNetCore.Mvc;

namespace apbd_dotnet.controllers;

[ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetReservations([FromQuery] DateOnly? date, [FromQuery] string? status, [FromQuery] int? roomId)
        {
            var query = InMemoryDataStorage.Reservations.AsQueryable();

            if (date.HasValue)
                query = query.Where(r => r.Date == date.Value);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(r => r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));

            if (roomId.HasValue)
                query = query.Where(r => r.RoomId == roomId.Value);

            return Ok(query.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetReservationById(int id)
        {
            var reservation = InMemoryDataStorage.Reservations.FirstOrDefault(r => r.Id == id);
            if (reservation == null) return NotFound();

            return Ok(reservation);
        }

        [HttpPost]
        public IActionResult CreateReservation([FromBody] Reservation reservation)
        {
            var room = InMemoryDataStorage.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
            if (room == null)
            {
                return NotFound("Podana sala nie istnieje.");
            }
            if (!room.IsActive)
            {
                return BadRequest("Podana sala jest oznaczona jako nieaktywna.");
            }

            bool isConflict = InMemoryDataStorage.Reservations.Any(r =>
                r.RoomId == reservation.RoomId &&
                r.Date == reservation.Date &&
                r.Id != reservation.Id && 
                !(reservation.EndTime <= r.StartTime || reservation.StartTime >= r.EndTime));

            if (isConflict)
            {
                return Conflict("Rezerwacja koliduje czasowo z inną rezerwacją w tej sali.");
            }

            reservation.Id = InMemoryDataStorage.Reservations.Any() ? InMemoryDataStorage.Reservations.Max(r => r.Id) + 1 : 1;
            InMemoryDataStorage.Reservations.Add(reservation);

            return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, reservation);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateReservation(int id, [FromBody] Reservation updatedReservation)
        {
            var reservation = InMemoryDataStorage.Reservations.FirstOrDefault(r => r.Id == id);
            if (reservation == null) return NotFound();

            bool isConflict = InMemoryDataStorage.Reservations.Any(r =>
                r.RoomId == updatedReservation.RoomId &&
                r.Date == updatedReservation.Date &&
                r.Id != id &&
                !(updatedReservation.EndTime <= r.StartTime || updatedReservation.StartTime >= r.EndTime));

            if (isConflict)
            {
                return Conflict("Zaktualizowana rezerwacja koliduje czasowo z inną rezerwacją.");
            }

            reservation.RoomId = updatedReservation.RoomId;
            reservation.OrganizerName = updatedReservation.OrganizerName;
            reservation.Topic = updatedReservation.Topic;
            reservation.Date = updatedReservation.Date;
            reservation.StartTime = updatedReservation.StartTime;
            reservation.EndTime = updatedReservation.EndTime;
            reservation.Status = updatedReservation.Status;

            return Ok(reservation);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReservation(int id)
        {
            var reservation = InMemoryDataStorage.Reservations.FirstOrDefault(r => r.Id == id);
            if (reservation == null) return NotFound();

            InMemoryDataStorage.Reservations.Remove(reservation);
            return NoContent();
        }
    }