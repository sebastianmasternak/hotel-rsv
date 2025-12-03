using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuestController : ControllerBase
    {
        private readonly HotelDbContext _db;

        public GuestController(HotelDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Guest>>> GetAll()
        {
            var guests = await _db.Guests.ToListAsync();
            return Ok(guests);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Guest>> Get([FromRoute] int id)
        {
            var guest = await _db.Guests.FindAsync(id);
            if (guest == null) return NotFound();
            return guest;
        }

        /// <summary>
        /// Creates a new guest.
        /// POST api/guest
        /// Body: { "firstName":"John", "lastName":"Doe", "email":"john@example.com", "phoneNumber":"123456789" }
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Guest>> Create([FromBody] Guest guest)
        {
            // [ApiController] will automatically validate DataAnnotations and return 400 if invalid,
            // but check ModelState explicitly to return structured errors when needed.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _db.Guests.Add(guest);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = guest.Id }, guest);
        }
    }
}
