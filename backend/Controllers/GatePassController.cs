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
public class GatePassController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public GatePassController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Warden")]
    public async Task<ActionResult<IEnumerable<GatePassDTO>>> GetGatePassRequests()
    {
        var requests = await _context.GatePassRequests
            .Include(g => g.Student)
            .Select(g => new GatePassDTO
            {
                RequestId = g.RequestId,
                StudentId = g.StudentId,
                StudentName = g.Student!.Name,
                RollNumber = g.Student.RollNumber,
                RequestDate = g.RequestDate,
                Purpose = g.Purpose,
                OutTime = g.OutTime,
                InTime = g.InTime,
                Status = g.Status,
                ApprovedBy = g.ApprovedBy
            })
            .OrderByDescending(g => g.RequestDate)
            .ToListAsync();

        return Ok(requests);
    }

    [HttpGet("student/{studentId}")]
    public async Task<ActionResult<IEnumerable<GatePassDTO>>> GetStudentGatePassRequests(int studentId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _context.Users.FindAsync(userId);
        
        // Students can only see their own requests
        if (user?.Role == "Student")
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
            if (student == null || student.StudentId != studentId)
                return Forbid();
        }

        var requests = await _context.GatePassRequests
            .Include(g => g.Student)
            .Where(g => g.StudentId == studentId)
            .Select(g => new GatePassDTO
            {
                RequestId = g.RequestId,
                StudentId = g.StudentId,
                StudentName = g.Student!.Name,
                RollNumber = g.Student.RollNumber,
                RequestDate = g.RequestDate,
                Purpose = g.Purpose,
                OutTime = g.OutTime,
                InTime = g.InTime,
                Status = g.Status,
                ApprovedBy = g.ApprovedBy
            })
            .OrderByDescending(g => g.RequestDate)
            .ToListAsync();

        return Ok(requests);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GatePassDTO>> GetGatePassRequest(int id)
    {
        var request = await _context.GatePassRequests
            .Include(g => g.Student)
            .FirstOrDefaultAsync(g => g.RequestId == id);

        if (request == null)
            return NotFound();

        return Ok(new GatePassDTO
        {
            RequestId = request.RequestId,
            StudentId = request.StudentId,
            StudentName = request.Student!.Name,
            RollNumber = request.Student.RollNumber,
            RequestDate = request.RequestDate,
            Purpose = request.Purpose,
            OutTime = request.OutTime,
            InTime = request.InTime,
            Status = request.Status,
            ApprovedBy = request.ApprovedBy
        });
    }

    [HttpPost]
    [Authorize(Roles = "Student")]
    public async Task<ActionResult<GatePassDTO>> CreateGatePassRequest([FromBody] CreateGatePassDTO dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
        
        if (student == null)
            return BadRequest(new { message = "Student record not found" });

        var request = new Models.GatePassRequest
        {
            StudentId = student.StudentId,
            RequestDate = dto.RequestDate,
            Purpose = dto.Purpose,
            OutTime = dto.OutTime,
            InTime = dto.InTime,
            Status = "Pending"
        };

        _context.GatePassRequests.Add(request);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetGatePassRequest), new { id = request.RequestId }, new GatePassDTO
        {
            RequestId = request.RequestId,
            StudentId = request.StudentId,
            StudentName = student.Name,
            RollNumber = student.RollNumber,
            RequestDate = request.RequestDate,
            Purpose = request.Purpose,
            OutTime = request.OutTime,
            InTime = request.InTime,
            Status = request.Status,
            ApprovedBy = request.ApprovedBy
        });
    }

    [HttpPut("{id}/approve")]
    [Authorize(Roles = "Admin,Warden")]
    public async Task<IActionResult> ApproveGatePassRequest(int id, [FromBody] ApproveGatePassDTO dto)
    {
        var request = await _context.GatePassRequests.FindAsync(id);
        if (request == null)
            return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        request.Status = dto.Status;
        request.ApprovedBy = userId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Warden")]
    public async Task<IActionResult> DeleteGatePassRequest(int id)
    {
        var request = await _context.GatePassRequests.FindAsync(id);
        if (request == null)
            return NotFound();

        _context.GatePassRequests.Remove(request);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}



