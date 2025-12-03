using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly HotelDbContext _context;
        public RoomController(HotelDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetAll()
        {
            var rooms = await _context.Rooms.Include(r => r.Type).ToListAsync();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> Get([FromRoute] int id)
        {
            var room = await _context.Rooms.Include(r => r.Type).FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }

        /// <summary>
        /// Returns rooms that are NOT reserved for any overlapping reservation between checkIn (inclusive) and checkOut (exclusive).
        /// Query parameters: checkIn, checkOut (ISO 8601).
        /// Example: GET api/room/available?checkIn=2025-12-05&checkOut=2025-12-08
        /// </summary>
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<Room>>> GetAvailable([FromQuery] DateTime checkIn, [FromQuery] DateTime checkOut)
        {
            if (checkIn == default || checkOut == default)
            {
                return BadRequest("Both 'checkIn' and 'checkOut' query parameters are required in ISO 8601 format.");
            }

            if (checkOut <= checkIn)
            {
                return BadRequest("'checkOut' must be after 'checkIn'.");
            }

            // Room is available when there is no reservation that overlaps the requested interval.
            var availableRooms = await _context.Rooms
                .Include(r => r.Type)
                .Where(r => !_context.Reservations.Any(res =>
                    res.RoomId == r.Id &&
                    res.CheckInDate < checkOut &&
                    res.CheckOutDate > checkIn
                ))
                .ToListAsync();

            return Ok(availableRooms);
        }

        /// <summary>
        /// Creates a new room.
        /// POST api/room
        /// Body: { "number": "101A", "roomTypeId": 1 }
        /// </summary>
        public class RoomCreateDto
        {
            [System.ComponentModel.DataAnnotations.Required]
            [System.ComponentModel.DataAnnotations.StringLength(10, ErrorMessage = "Room number cannot be longer than 10 characters.")]
            public string Number { get; set; }

            [System.ComponentModel.DataAnnotations.Required]
            public int RoomTypeId { get; set; }
        }

        [HttpPost]
        public async Task<ActionResult<Room>> Create([FromBody] RoomCreateDto dto)
        {
            // [ApiController] ensures model validation; still check dto explicitly for clarity.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verify the referenced RoomType exists.
            var roomTypeExists = await _context.RoomTypes.AnyAsync(rt => rt.Id == dto.RoomTypeId);
            if (!roomTypeExists)
                return BadRequest($"RoomTypeId {dto.RoomTypeId} does not exist.");

            var room = new Room
            {
                Number = dto.Number,
                RoomTypeId = dto.RoomTypeId
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            // Load navigation property if needed
            await _context.Entry(room).Reference(r => r.Type).LoadAsync();

            return CreatedAtAction(nameof(Get), new { id = room.Id }, room);
        }

        /// <summary>
        /// Deletes a room by id.
        /// DELETE api/room/{id}
        /// Prevents deletion if there are reservations referencing the room.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
                return NotFound();

            // Prevent deleting a room that has any reservations
            var hasReservations = await _context.Reservations.AnyAsync(r => r.RoomId == id);
            if (hasReservations)
                return Conflict("Cannot delete room while reservations referencing it exist.");

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
