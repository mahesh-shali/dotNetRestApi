using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestApi.Data;
using RestApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

//just checking the git connections

namespace RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ApplicationDbContext context, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation($"Received registration request: Name={registerRequest.Name}, Email={registerRequest.Email}, Phone={registerRequest.Phone}, RoleId={registerRequest.RoleId}, IpAddress={registerRequest.IpAddress}, Browser={registerRequest.BrowserInfo?.Browser}, BrowserVersion={registerRequest.BrowserInfo?.BrowserVersion}, OS={registerRequest.OS}, PhonePrefix={registerRequest.PhonePrefix}");

                // Check if the role exists
                var role = await _context.Roles.FindAsync(registerRequest.RoleId);

                if (role == null)
                {
                    registerRequest.RoleId = 2; // Default RoleId
                }

                // Check if the email is already registered
                if (await _context.Users.AnyAsync(u => u.Email == registerRequest.Email))
                    return BadRequest("Email is already registered.");

                // Check if the phone is already registered
                if (await _context.Users.AnyAsync(u => u.Phone == registerRequest.Phone))
                    return BadRequest("Phone number already registered.");

                if (registerRequest.Password.Length < 8)
                {
                    return BadRequest("Password must be at least 8 characters.");
                }

                // Hash the password
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

                // Create a new user object
                var newUser = new User
                {
                    Name = registerRequest.Name,
                    Email = registerRequest.Email,
                    Password = hashedPassword,
                    Phone = registerRequest.Phone,
                    RoleId = registerRequest.RoleId ?? 2,
                    createdAt = DateTime.UtcNow,
                    modifiedAt = null,
                    IpAddress = registerRequest.IpAddress,
                    Browser = registerRequest.BrowserInfo?.Browser,
                    BrowserVersion = registerRequest.BrowserInfo?.BrowserVersion,
                    OS = registerRequest.OS,
                    PhonePrefix = registerRequest.PhonePrefix,
                    IsEmailVerified = false,
                    IsPhoneNumberVerified = false,
                    IsLoggedInByGoogleId = false,
                    Street = "",
                    City = "",
                    State = "",
                    PostalCode = "",
                    Country = "",
                    IsLoggedInByFaceBookId = false
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return Ok("User registered successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user.");
                return StatusCode(500, "Internal server error");
            }
        }




        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                // Validate the model
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Find user by email or username
                var user = await _context.Users
                                         .Include(u => u.Role)
                                         .FirstOrDefaultAsync(u => u.Email == loginRequest.EmailOrUsername || u.Name == loginRequest.EmailOrUsername);

                // Check if user exists or password is invalid
                if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
                {
                    // Return a single generic error for invalid credentials
                    return Unauthorized(new
                    {
                        message = "Invalid email/username or password.",
                        errors = new
                        {
                            credentials = "Invalid email/username or password."
                        }
                    });
                }

                // Generate claims for JWT
                var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.Name),
            new Claim("RoleId", user.RoleId.ToString()) // Adding RoleId explicitly
        };

                var token = GenerateToken(authClaims);

                return Ok(new
                {
                    Message = "Login successful",
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                    User = new
                    {
                        user.UserId,
                        user.Email,
                        user.Name,
                        Role = user.Role.Name
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred during login: {ex.Message}");

                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }


        private JwtSecurityToken GenerateToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            if (authSigningKey.KeySize < 256)
            {
                throw new Exception("JWT Key must be at least 256 bits (32 characters).");
            }

            return new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
        }
    }
}
