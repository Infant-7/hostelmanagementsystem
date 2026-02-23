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
public class AttendanceController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AttendanceController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Warden")]
    public async Task<ActionResult<IEnumerable<AttendanceDTO>>> GetAttendances()
    {
        var attendances = await _context.Attendances
            .Include(a => a.Student)
            .Select(a => new AttendanceDTO
            {
                AttendanceId = a.AttendanceId,
                StudentId = a.StudentId,
                StudentName = a.Student!.Name,
                RollNumber = a.Student.RollNumber,
                Date = a.Date,
                Status = a.Status,
                MarkedBy = a.MarkedBy
            })
            .OrderByDescending(a => a.Date)
            .ToListAsync();

        return Ok(attendances);
    }

    [HttpGet("student/{studentId}")]
    public async Task<ActionResult<IEnumerable<AttendanceDTO>>> GetStudentAttendance(int studentId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _context.Users.FindAsync(userId);
        
        // Students can only see their own attendance
        if (user?.Role == "Student")
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
            if (student == null || student.StudentId != studentId)
                return Forbid();
        }

        var attendances = await _context.Attendances
            .Include(a => a.Student)
            .Where(a => a.StudentId == studentId)
            .Select(a => new AttendanceDTO
            {
                AttendanceId = a.AttendanceId,
                StudentId = a.StudentId,
                StudentName = a.Student!.Name,
                RollNumber = a.Student.RollNumber,
                Date = a.Date,
                Status = a.Status,
                MarkedBy = a.MarkedBy
            })
            .OrderByDescending(a => a.Date)
            .ToListAsync();

        return Ok(attendances);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AttendanceDTO>> GetAttendance(int id)
    {
        var attendance = await _context.Attendances
            .Include(a => a.Student)
            .FirstOrDefaultAsync(a => a.AttendanceId == id);

        if (attendance == null)
            return NotFound();

        return Ok(new AttendanceDTO
        {
            AttendanceId = attendance.AttendanceId,
            StudentId = attendance.StudentId,
            StudentName = attendance.Student!.Name,
            RollNumber = attendance.Student.RollNumber,
            Date = attendance.Date,
            Status = attendance.Status,
            MarkedBy = attendance.MarkedBy
        });
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Warden")]
    public async Task<ActionResult<AttendanceDTO>> CreateAttendance([FromBody] CreateAttendanceDTO dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Check if attendance already exists for this student and date
        var existing = await _context.Attendances
            .FirstOrDefaultAsync(a => a.StudentId == dto.StudentId && a.Date.Date == dto.Date.Date);

        if (existing != null)
            return BadRequest(new { message = "Attendance already marked for this date" });

        var attendance = new Models.Attendance
        {
            StudentId = dto.StudentId,
            Date = dto.Date,
            Status = dto.Status,
            MarkedBy = userId
        };

        _context.Attendances.Add(attendance);
        await _context.SaveChangesAsync();

        var student = await _context.Students.FindAsync(dto.StudentId);
        return CreatedAtAction(nameof(GetAttendance), new { id = attendance.AttendanceId }, new AttendanceDTO
        {
            AttendanceId = attendance.AttendanceId,
            StudentId = attendance.StudentId,
            StudentName = student?.Name ?? "",
            RollNumber = student?.RollNumber ?? "",
            Date = attendance.Date,
            Status = attendance.Status,
            MarkedBy = attendance.MarkedBy
        });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Warden")]
    public async Task<IActionResult> UpdateAttendance(int id, [FromBody] UpdateAttendanceDTO dto)
    {
        var attendance = await _context.Attendances.FindAsync(id);
        if (attendance == null)
            return NotFound();

        attendance.Date = dto.Date;
        attendance.Status = dto.Status;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Warden")]
    public async Task<IActionResult> DeleteAttendance(int id)
    {
        var attendance = await _context.Attendances.FindAsync(id);
        if (attendance == null)
            return NotFound();

        _context.Attendances.Remove(attendance);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}



