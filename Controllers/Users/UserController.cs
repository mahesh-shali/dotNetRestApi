using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApi.Data;
using System.Security.Claims;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Authorize(Roles = "user")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Endpoint for user dashboard
        [HttpGet("dashboard")]
        public IActionResult UserDashboard()
        {
            return Ok("Welcome, User!");
        }

        // Endpoint for user settings
        [HttpGet("settings")]
        public IActionResult UserSettings()
        {
            return Ok("User Settings Page");
        }
    }
}
