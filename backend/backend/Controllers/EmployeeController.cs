using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly HotelDbContext _db;

        public EmployeeController(HotelDbContext db)
        {
            _db = db;
        }

        public class LoginRequest
        {
            [Required]
            public string Login { get; set; }

            [Required]
            public string Password { get; set; }
        }

        public class EmployeeResponse
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Login { get; set; }
        }

        /// <summary>
        /// Verifies employee credentials.
        /// POST api/employee/login
        /// Body: { "login": "maras", "password": "admin" }
        /// Returns 200 with employee info if credentials are valid, 401 otherwise.
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<EmployeeResponse>> Login([FromBody] LoginRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var emp = await _db.Employees.SingleOrDefaultAsync(e => e.Login == req.Login);
            if (emp == null)
                return Unauthorized("Invalid credentials.");

            if (emp.PasswordHash == null || emp.PasswordSalt == null)
                return Unauthorized("Invalid credentials.");

            var verified = PasswordHasher.VerifyPassword(req.Password, emp.PasswordHash, emp.PasswordSalt);
            if (!verified)
                return Unauthorized("Invalid credentials.");

            var resp = new EmployeeResponse
            {
                Id = emp.Id,
                Name = emp.Name,
                Login = emp.Login
            };

            return Ok(resp);
        }
    }
}
