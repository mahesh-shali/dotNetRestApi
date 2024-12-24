using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApi.Data;
using RestApi.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "admin")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ApplicationDbContext context, IConfiguration configuration, ILogger<AdminController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // Admin Dashboard Route

        [HttpGet("dashboard")]
        [Authorize(Roles = "admin")]
        public IActionResult AdminDashboard()
        {
            return Ok("Welcome, Admin!");
        }

        // Admin Settings Route
        [Authorize(Roles = "admin")]
        [HttpGet("settings")]
        public IActionResult AdminSettings()
        {
            return Ok("Admin Settings Page");
        }

        // Get specific user by ID

        [HttpGet("user/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUserById(long id)
        {
            try
            {
                var user = await _context.Users
                                         .Include(u => u.Role)
                                         .FirstOrDefaultAsync(u => u.UserId == id);

                if (user == null)
                {
                    return NotFound(new ApiErrorResponse(
                        status: (int)HttpStatusCode.NotFound,
                        message: "User not found.",
                        detail: $"The user with ID {id} does not exist.",
                        instance: HttpContext.Request.Path
                    ));
                }

                return Ok(new
                {
                    user.UserId,
                    user.Name,
                    user.Email,
                    user.Phone,
                    RoleId = user.RoleId,
                    Role = user.Role?.Name
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the user by ID.");
                return StatusCode(500, new ApiErrorResponse(
                    status: (int)HttpStatusCode.InternalServerError,
                    message: "Internal server error.",
                    detail: ex.Message,
                    instance: HttpContext.Request.Path
                ));
            }
        }

        // Get all users

        [HttpGet("users")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _context.Users
                                           .Include(u => u.Role)
                                           .ToListAsync();

                if (users == null || users.Count == 0)
                {
                    return NotFound(new ApiErrorResponse(
                        status: (int)HttpStatusCode.NotFound,
                        message: "No users found.",
                        detail: "There are no users available in the database.",
                        instance: HttpContext.Request.Path
                    ));
                }

                var userList = users.Select(u => new
                {
                    u.UserId,
                    u.Name,
                    u.Email,
                    u.Phone,
                    RoleId = u.RoleId,
                    Role = u.Role?.Name
                }).ToList();

                return Ok(userList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all users.");
                return StatusCode(500, new ApiErrorResponse(
                    status: (int)HttpStatusCode.InternalServerError,
                    message: "Internal server error.",
                    detail: ex.Message,
                    instance: HttpContext.Request.Path
                ));
            }
        }
    }
}
