using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using HostelManagement.API.Data;
using HostelManagement.API.DTOs;
using HostelManagement.API.Models;

namespace HostelManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Warden")]
public class StudentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public StudentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
    {
        try
        {
            var students = await _context.Students
                .Include(s => s.User)
                .Select(s => new StudentDTO
                {
                    StudentId = s.StudentId,
                    UserId = s.UserId,
                    Name = s.Name,
                    RollNumber = s.RollNumber,
                    RoomNumber = s.RoomNumber,
                    Phone = s.Phone,
                    Address = s.Address
                })
                .ToListAsync();

            return Ok(students);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving students", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDTO>> GetStudent(int id)
    {
        var student = await _context.Students
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.StudentId == id);

        if (student == null)
            return NotFound();

        return Ok(new StudentDTO
        {
            StudentId = student.StudentId,
            UserId = student.UserId,
            Name = student.Name,
            RollNumber = student.RollNumber,
            RoomNumber = student.RoomNumber,
            Phone = student.Phone,
            Address = student.Address
        });
    }

    [HttpPost]
    public async Task<ActionResult<StudentDTO>> CreateStudent([FromBody] CreateStudentDTO dto)
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
            if (string.IsNullOrWhiteSpace(dto.RollNumber))
                return BadRequest(new { message = "Roll number is required" });

            // Check if username already exists
            var existingUser = await _userManager.FindByNameAsync(dto.Username);
            if (existingUser != null)
                return BadRequest(new { message = "Username already exists" });

            // Check if email already exists
            existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                return BadRequest(new { message = "Email already exists" });

            // Check if roll number already exists
            if (await _context.Students.AnyAsync(s => s.RollNumber == dto.RollNumber))
                return BadRequest(new { message = "Roll number already exists" });

            // Create user
            var user = new ApplicationUser
            {
                UserName = dto.Username,
                Email = dto.Email,
                Role = "Student"
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

            // Create student
            var student = new Student
            {
                UserId = user.Id,
                Name = dto.Name,
                RollNumber = dto.RollNumber,
                RoomNumber = dto.RoomNumber,
                Phone = dto.Phone,
                Address = dto.Address
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudent), new { id = student.StudentId }, new StudentDTO
            {
                StudentId = student.StudentId,
                UserId = student.UserId,
                Name = student.Name,
                RollNumber = student.RollNumber,
                RoomNumber = student.RoomNumber,
                Phone = student.Phone,
                Address = student.Address
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error creating student", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(int id, [FromBody] UpdateStudentDTO dto)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
            return NotFound();

        student.Name = dto.Name;
        student.RoomNumber = dto.RoomNumber;
        student.Phone = dto.Phone;
        student.Address = dto.Address;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var student = await _context.Students.Include(s => s.User).FirstOrDefaultAsync(s => s.StudentId == id);
        if (student == null)
            return NotFound();

        _context.Students.Remove(student);
        if (student.User != null)
        {
            await _userManager.DeleteAsync(student.User);
        }
        await _context.SaveChangesAsync();

        return NoContent();
    }
}



