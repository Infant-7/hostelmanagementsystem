using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using HostelManagement.API.Data;
using HostelManagement.API.DTOs;
using HostelManagement.API.Models;

namespace HostelManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Check if user already exists
        var existingUser = await _userManager.FindByNameAsync(request.Username);
        if (existingUser != null)
            return BadRequest(new { message = "Username already exists" });

        existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            return BadRequest(new { message = "Email already exists" });

        // Normalize role (case-insensitive)
        var normalizedRole = request.Role?.Trim();
        if (string.IsNullOrEmpty(normalizedRole))
            return BadRequest(new { message = "Role is required" });

        // Validate role
        var validRoles = new[] { "Student", "Admin", "Warden" };
        var role = validRoles.FirstOrDefault(r => r.Equals(normalizedRole, StringComparison.OrdinalIgnoreCase));
        if (role == null)
            return BadRequest(new { message = "Invalid role. Must be Student, Admin, or Warden" });

        // Create user
        var user = new ApplicationUser
        {
            UserName = request.Username,
            Email = request.Email,
            Role = role
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return BadRequest(new { message = "Registration failed", errors = result.Errors });

        // Create Student or Warden record if applicable
        if (role == "Student" && !string.IsNullOrEmpty(request.Name) && !string.IsNullOrEmpty(request.RollNumber))
        {
            var student = new Student
            {
                UserId = user.Id,
                Name = request.Name,
                RollNumber = request.RollNumber,
                Phone = request.Phone,
                Address = request.Address,
                RoomNumber = null
            };
            _context.Students.Add(student);
        }
        else if (role == "Warden" && !string.IsNullOrEmpty(request.Name))
        {
            var warden = new Warden
            {
                UserId = user.Id,
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                Department = request.Department
            };
            _context.Wardens.Add(warden);
        }

        await _context.SaveChangesAsync();

        // Generate token
        var token = GenerateJwtToken(user);
        return Ok(new AuthResponse
        {
            Token = token,
            Username = user.UserName!,
            Email = user.Email!,
            Role = user.Role,
            UserId = user.Id
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.FindByNameAsync(request.Username);
        if (user == null)
            return Unauthorized(new { message = "Invalid credentials" });

        // Check role (case-insensitive)
        var normalizedRequestRole = request.Role?.Trim();
        if (string.IsNullOrEmpty(normalizedRequestRole) || 
            !user.Role.Equals(normalizedRequestRole, StringComparison.OrdinalIgnoreCase))
            return Unauthorized(new { message = "Invalid role for this login" });

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
            return Unauthorized(new { message = "Invalid credentials" });

        // Generate token
        var token = GenerateJwtToken(user);
        return Ok(new AuthResponse
        {
            Token = token,
            Username = user.UserName!,
            Email = user.Email!,
            Role = user.Role,
            UserId = user.Id
        });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return NotFound();

        return Ok(new UserResponse
        {
            Id = user.Id,
            Username = user.UserName!,
            Email = user.Email!,
            Role = user.Role
        });
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);
        var expirationMinutes = int.Parse(jwtSettings["ExpirationInMinutes"]!);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Role, user.Role), // Use ClaimTypes.Role for role-based authorization
            new Claim("Role", user.Role) // Keep custom claim for frontend use
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

