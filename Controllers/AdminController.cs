using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "admin")]
    public class AdminController : ControllerBase
    {
        [HttpGet("dashboard")]
        public IActionResult AdminDashboard()
        {
            return Ok("Welcome, Admin!");
        }

        [HttpGet("settings")]
        public IActionResult AdminSettings()
        {
            return Ok("Admin Settings Page");
        }
    }
}
