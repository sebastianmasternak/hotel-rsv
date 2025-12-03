using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly HotelDbContext _db;

        public ReservationController(HotelDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetAll()
        {
            return await _db.Reservations
                .Include(r => r.Guest)
                .Include(r => r.Room)
                .ThenInclude(rm => rm.Type)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> Get([FromRoute] int id)
        {
            var reservation = await _db.Reservations
                .Include(r => r.Guest)
                .Include(r => r.Room)
                .ThenInclude(rm => rm.Type)
                .SingleOrDefaultAsync(r => r.Id == id);

            if (reservation == null) return NotFound();
            return Ok(reservation);
        }

        /// <summary>
        /// Create a reservation.
        /// POST api/reservation
        /// Body: { "guestId":1, "roomId":2, "checkInDate":"2025-12-05", "checkOutDate":"2025-12-08", "totalPrice":123.45 }
        /// </summary>
        public class ReservationCreateDto
        {
            [Required]
            public int GuestId { get; set; }

            [Required]
            public int RoomId { get; set; }

            [Required]
            public DateTime CheckInDate { get; set; }

            [Required]
            public DateTime CheckOutDate { get; set; }

            [Range(0, double.MaxValue, ErrorMessage = "TotalPrice must be non-negative.")]
            public decimal TotalPrice { get; set; }
        }

        [HttpPost]
        public async Task<ActionResult<Reservation>> Create([FromBody] ReservationCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.CheckOutDate <= dto.CheckInDate)
                return BadRequest("'checkOutDate' must be after 'checkInDate'.");

            var guestExists = await _db.Guests.AnyAsync(g => g.Id == dto.GuestId);
            if (!guestExists)
                return BadRequest($"Guest with id {dto.GuestId} does not exist.");

            var room = await _db.Rooms.FindAsync(dto.RoomId);
            if (room == null)
                return BadRequest($"Room with id {dto.RoomId} does not exist.");

            // Check for overlapping reservations for the same room:
            // overlap condition: existing.CheckIn < requested.CheckOut && existing.CheckOut > requested.CheckIn
            var hasOverlap = await _db.Reservations.AnyAsync(r =>
                r.RoomId == dto.RoomId &&
                r.CheckInDate < dto.CheckOutDate &&
                r.CheckOutDate > dto.CheckInDate
            );

            if (hasOverlap)
                return Conflict("Room is already reserved for the requested dates.");

            var reservation = new Reservation
            {
                GuestId = dto.GuestId,
                RoomId = dto.RoomId,
                CheckInDate = dto.CheckInDate,
                CheckOutDate = dto.CheckOutDate,
                TotalPrice = dto.TotalPrice
            };

            _db.Reservations.Add(reservation);
            await _db.SaveChangesAsync();

            // load navigation properties for response
            await _db.Entry(reservation).Reference(r => r.Guest).LoadAsync();
            await _db.Entry(reservation).Reference(r => r.Room).LoadAsync();
            await _db.Entry(reservation.Room).Reference(rm => rm.Type).LoadAsync();

            return CreatedAtAction(nameof(Get), new { id = reservation.Id }, reservation);
        }

        /// <summary>
        /// Deletes a reservation by id.
        /// DELETE api/reservation/{id}
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var reservation = await _db.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound();

            _db.Reservations.Remove(reservation);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
