using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarpetCleaningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public record LoginRequest(string Email, string Password);

        public record RegisterRequest(string Email, string Password);

        public record CreateUserRequest(string Email, string Password, string Role); // Admin-only

        public record AssignRoleRequest(string Email, string Role); // Admin-only

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return Unauthorized("Invalid credentials.");

            var validPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!validPassword) return Unauthorized("Invalid credentials.");

            var roles = await _userManager.GetRolesAsync(user);
            var token = CreateJwtToken(user, roles);

            return Ok(new { accessToken = token });
        }

        // Public register (for web app signup)
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var existing = await _userManager.FindByEmailAsync(request.Email);
            if (existing != null) return Conflict("Email already exists.");

            var user = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            // Default role for new signups (web app). Choose one:
            // Option A: Employee by default (simple for your case)
            await _userManager.AddToRoleAsync(user, "Employee");

            // Option B: no role by default (then Admin assigns later)
            // (comment out AddToRoleAsync above)

            var roles = await _userManager.GetRolesAsync(user);
            var token = CreateJwtToken(user, roles);

            return Ok(new { accessToken = token });
        }

        // Admin creates users (employees/admins) explicitly
        [HttpPost("users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var existing = await _userManager.FindByEmailAsync(request.Email);
            if (existing != null) return Conflict("Email already exists.");

            var user = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            // Expect Role = "Admin" or "Employee"
            var roleResult = await _userManager.AddToRoleAsync(user, request.Role);
            if (!roleResult.Succeeded)
                return BadRequest(roleResult.Errors.Select(e => e.Description));

            return Created(string.Empty, new { email = user.Email, role = request.Role });
        }

        // Admin assigns role to an existing user
        [HttpPost("assign-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return NotFound("User not found.");

            // Optional: remove all current roles first (simple approach)
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

            var result = await _userManager.AddToRoleAsync(user, request.Role);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            return NoContent();
        }

        private string CreateJwtToken(IdentityUser user, IList<string> roles)
        {
            var jwt = _configuration.GetSection("Jwt");

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.UserName ?? string.Empty)
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}


