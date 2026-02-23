using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using HostelManagement.API.Data;
using HostelManagement.API.DTOs;

namespace HostelManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GrievanceController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public GrievanceController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Warden")]
    public async Task<ActionResult<IEnumerable<GrievanceDTO>>> GetGrievances()
    {
        var grievances = await _context.Grievances
            .Include(g => g.Student)
            .Select(g => new GrievanceDTO
            {
                GrievanceId = g.GrievanceId,
                StudentId = g.StudentId,
                StudentName = g.Student!.Name,
                RollNumber = g.Student.RollNumber,
                Title = g.Title,
                Description = g.Description,
                Status = g.Status,
                CreatedDate = g.CreatedDate,
                ResolvedDate = g.ResolvedDate,
                ResolvedBy = g.ResolvedBy
            })
            .OrderByDescending(g => g.CreatedDate)
            .ToListAsync();

        return Ok(grievances);
    }

    [HttpGet("student/{studentId}")]
    public async Task<ActionResult<IEnumerable<GrievanceDTO>>> GetStudentGrievances(int studentId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _context.Users.FindAsync(userId);
        
        // Students can only see their own grievances
        if (user?.Role == "Student")
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
            if (student == null || student.StudentId != studentId)
                return Forbid();
        }

        var grievances = await _context.Grievances
            .Include(g => g.Student)
            .Where(g => g.StudentId == studentId)
            .Select(g => new GrievanceDTO
            {
                GrievanceId = g.GrievanceId,
                StudentId = g.StudentId,
                StudentName = g.Student!.Name,
                RollNumber = g.Student.RollNumber,
                Title = g.Title,
                Description = g.Description,
                Status = g.Status,
                CreatedDate = g.CreatedDate,
                ResolvedDate = g.ResolvedDate,
                ResolvedBy = g.ResolvedBy
            })
            .OrderByDescending(g => g.CreatedDate)
            .ToListAsync();

        return Ok(grievances);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GrievanceDTO>> GetGrievance(int id)
    {
        var grievance = await _context.Grievances
            .Include(g => g.Student)
            .FirstOrDefaultAsync(g => g.GrievanceId == id);

        if (grievance == null)
            return NotFound();

        return Ok(new GrievanceDTO
        {
            GrievanceId = grievance.GrievanceId,
            StudentId = grievance.StudentId,
            StudentName = grievance.Student!.Name,
            RollNumber = grievance.Student.RollNumber,
            Title = grievance.Title,
            Description = grievance.Description,
            Status = grievance.Status,
            CreatedDate = grievance.CreatedDate,
            ResolvedDate = grievance.ResolvedDate,
            ResolvedBy = grievance.ResolvedBy
        });
    }

    [HttpPost]
    [Authorize(Roles = "Student")]
    public async Task<ActionResult<GrievanceDTO>> CreateGrievance([FromBody] CreateGrievanceDTO dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
        
        if (student == null)
            return BadRequest(new { message = "Student record not found" });

        var grievance = new Models.Grievance
        {
            StudentId = student.StudentId,
            Title = dto.Title,
            Description = dto.Description,
            Status = "Pending",
            CreatedDate = DateTime.Now
        };

        _context.Grievances.Add(grievance);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetGrievance), new { id = grievance.GrievanceId }, new GrievanceDTO
        {
            GrievanceId = grievance.GrievanceId,
            StudentId = grievance.StudentId,
            StudentName = student.Name,
            RollNumber = student.RollNumber,
            Title = grievance.Title,
            Description = grievance.Description,
            Status = grievance.Status,
            CreatedDate = grievance.CreatedDate,
            ResolvedDate = grievance.ResolvedDate,
            ResolvedBy = grievance.ResolvedBy
        });
    }

    [HttpPut("{id}/resolve")]
    [Authorize(Roles = "Admin,Warden")]
    public async Task<IActionResult> ResolveGrievance(int id, [FromBody] ResolveGrievanceDTO dto)
    {
        var grievance = await _context.Grievances.FindAsync(id);
        if (grievance == null)
            return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        grievance.Status = dto.Status;
        grievance.ResolvedBy = userId;
        
        if (dto.Status == "Resolved")
            grievance.ResolvedDate = DateTime.Now;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Warden")]
    public async Task<IActionResult> DeleteGrievance(int id)
    {
        var grievance = await _context.Grievances.FindAsync(id);
        if (grievance == null)
            return NotFound();

        _context.Grievances.Remove(grievance);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}



