using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HostelManagement.API.Data;
using HostelManagement.API.DTOs;
using HostelManagement.API.Models;
using System.Linq;

namespace HostelManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class WardensController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<WardensController> _logger;

    public WardensController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<WardensController> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WardenDTO>>> GetWardens()
    {
        try
        {
            _logger.LogInformation("Attempting to retrieve wardens...");
            
            var wardens = await _context.Wardens
                .Select(w => new WardenDTO
                {
                    WardenId = w.WardenId,
                    UserId = w.UserId,
                    Name = w.Name,
                    Email = w.Email ?? string.Empty,
                    Phone = w.Phone ?? string.Empty,
                    Department = w.Department ?? string.Empty
                })
                .ToListAsync();

            _logger.LogInformation($"Successfully retrieved {wardens.Count} wardens");
            return Ok(wardens);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving wardens: {Message}", ex.Message);
            _logger.LogError("Stack trace: {StackTrace}", ex.StackTrace);
            return StatusCode(500, new { 
                message = "Error retrieving wardens", 
                error = ex.Message,
                innerException = ex.InnerException?.Message,
                stackTrace = ex.StackTrace
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WardenDTO>> GetWarden(int id)
    {
        var warden = await _context.Wardens
            .FirstOrDefaultAsync(w => w.WardenId == id);

        if (warden == null)
            return NotFound();

        return Ok(new WardenDTO
        {
            WardenId = warden.WardenId,
            UserId = warden.UserId,
            Name = warden.Name,
            Email = warden.Email ?? string.Empty,
            Phone = warden.Phone ?? string.Empty,
            Department = warden.Department ?? string.Empty
        });
    }

    [HttpPost]
    public async Task<ActionResult<WardenDTO>> CreateWarden([FromBody] CreateWardenDTO dto)
    {
        try
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(dto.Username))
                return BadRequest(new { message = "Username is required" });
            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { message = "Email is required" });
            if (string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { message = "Password is required" });
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest(new { message = "Name is required" });

            // Check if username already exists
            var existingUser = await _userManager.FindByNameAsync(dto.Username);
            if (existingUser != null)
                return BadRequest(new { message = "Username already exists" });

            // Check if email already exists
            existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                return BadRequest(new { message = "Email already exists" });

            // Create user
            var user = new ApplicationUser
            {
                UserName = dto.Username,
                Email = dto.Email,
                Role = "Warden"
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errorMessages = result.Errors.Select(e => e.Description).ToList();
                var errorText = string.Join("; ", errorMessages);
                return BadRequest(new { 
                    message = "Failed to create user", 
                    errors = errorMessages,
                    error = errorText
                });
            }

            // Create warden
            var warden = new Warden
            {
                UserId = user.Id,
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Department = dto.Department
            };

            _context.Wardens.Add(warden);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWarden), new { id = warden.WardenId }, new WardenDTO
            {
                WardenId = warden.WardenId,
                UserId = warden.UserId,
                Name = warden.Name,
                Email = warden.Email ?? string.Empty,
                Phone = warden.Phone ?? string.Empty,
                Department = warden.Department ?? string.Empty
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error creating warden", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateWarden(int id, [FromBody] UpdateWardenDTO dto)
    {
        try
        {
            var warden = await _context.Wardens.FindAsync(id);
            if (warden == null)
                return NotFound();

            warden.Name = dto.Name ?? warden.Name;
            warden.Email = dto.Email ?? warden.Email;
            warden.Phone = dto.Phone ?? warden.Phone;
            warden.Department = dto.Department ?? warden.Department;

            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error updating warden", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWarden(int id)
    {
        try
        {
            var warden = await _context.Wardens.FirstOrDefaultAsync(w => w.WardenId == id);
            if (warden == null)
                return NotFound();

            // Try to find and delete the associated user
            var user = await _userManager.FindByIdAsync(warden.UserId);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            _context.Wardens.Remove(warden);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error deleting warden", error = ex.Message });
        }
    }
}



