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
public class RoomChangeController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RoomChangeController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Warden")]
    public async Task<ActionResult<IEnumerable<RoomChangeDTO>>> GetRoomChangeRequests()
    {
        var requests = await _context.RoomChangeRequests
            .Include(r => r.Student)
            .Include(r => r.CurrentRoom)
            .Include(r => r.RequestedRoom)
            .Select(r => new RoomChangeDTO
            {
                RequestId = r.RequestId,
                StudentId = r.StudentId,
                StudentName = r.Student!.Name,
                RollNumber = r.Student.RollNumber,
                CurrentRoomId = r.CurrentRoomId,
                CurrentRoomNumber = r.CurrentRoom!.RoomNumber,
                RequestedRoomId = r.RequestedRoomId,
                RequestedRoomNumber = r.RequestedRoom!.RoomNumber,
                Reason = r.Reason,
                Status = r.Status,
                ApprovedBy = r.ApprovedBy
            })
            .OrderByDescending(r => r.RequestId)
            .ToListAsync();

        return Ok(requests);
    }

    [HttpGet("student/{studentId}")]
    public async Task<ActionResult<IEnumerable<RoomChangeDTO>>> GetStudentRoomChangeRequests(int studentId)
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

        var requests = await _context.RoomChangeRequests
            .Include(r => r.Student)
            .Include(r => r.CurrentRoom)
            .Include(r => r.RequestedRoom)
            .Where(r => r.StudentId == studentId)
            .Select(r => new RoomChangeDTO
            {
                RequestId = r.RequestId,
                StudentId = r.StudentId,
                StudentName = r.Student!.Name,
                RollNumber = r.Student.RollNumber,
                CurrentRoomId = r.CurrentRoomId,
                CurrentRoomNumber = r.CurrentRoom!.RoomNumber,
                RequestedRoomId = r.RequestedRoomId,
                RequestedRoomNumber = r.RequestedRoom!.RoomNumber,
                Reason = r.Reason,
                Status = r.Status,
                ApprovedBy = r.ApprovedBy
            })
            .OrderByDescending(r => r.RequestId)
            .ToListAsync();

        return Ok(requests);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoomChangeDTO>> GetRoomChangeRequest(int id)
    {
        var request = await _context.RoomChangeRequests
            .Include(r => r.Student)
            .Include(r => r.CurrentRoom)
            .Include(r => r.RequestedRoom)
            .FirstOrDefaultAsync(r => r.RequestId == id);

        if (request == null)
            return NotFound();

        return Ok(new RoomChangeDTO
        {
            RequestId = request.RequestId,
            StudentId = request.StudentId,
            StudentName = request.Student!.Name,
            RollNumber = request.Student.RollNumber,
            CurrentRoomId = request.CurrentRoomId,
            CurrentRoomNumber = request.CurrentRoom!.RoomNumber,
            RequestedRoomId = request.RequestedRoomId,
            RequestedRoomNumber = request.RequestedRoom!.RoomNumber,
            Reason = request.Reason,
            Status = request.Status,
            ApprovedBy = request.ApprovedBy
        });
    }

    [HttpPost]
    [Authorize(Roles = "Student")]
    public async Task<ActionResult<RoomChangeDTO>> CreateRoomChangeRequest([FromBody] CreateRoomChangeDTO dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
        
        if (student == null)
            return BadRequest(new { message = "Student record not found" });

        // Verify rooms exist
        var currentRoom = await _context.Rooms.FindAsync(dto.CurrentRoomId);
        var requestedRoom = await _context.Rooms.FindAsync(dto.RequestedRoomId);

        if (currentRoom == null || requestedRoom == null)
            return BadRequest(new { message = "Invalid room" });

        var request = new Models.RoomChangeRequest
        {
            StudentId = student.StudentId,
            CurrentRoomId = dto.CurrentRoomId,
            RequestedRoomId = dto.RequestedRoomId,
            Reason = dto.Reason,
            Status = "Pending"
        };

        _context.RoomChangeRequests.Add(request);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRoomChangeRequest), new { id = request.RequestId }, new RoomChangeDTO
        {
            RequestId = request.RequestId,
            StudentId = request.StudentId,
            StudentName = student.Name,
            RollNumber = student.RollNumber,
            CurrentRoomId = request.CurrentRoomId,
            CurrentRoomNumber = currentRoom.RoomNumber,
            RequestedRoomId = request.RequestedRoomId,
            RequestedRoomNumber = requestedRoom.RoomNumber,
            Reason = request.Reason,
            Status = request.Status,
            ApprovedBy = request.ApprovedBy
        });
    }

    [HttpPut("{id}/approve")]
    [Authorize(Roles = "Admin,Warden")]
    public async Task<IActionResult> ApproveRoomChangeRequest(int id, [FromBody] ApproveRoomChangeDTO dto)
    {
        var request = await _context.RoomChangeRequests
            .Include(r => r.Student)
            .FirstOrDefaultAsync(r => r.RequestId == id);

        if (request == null)
            return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        request.Status = dto.Status;
        request.ApprovedBy = userId;

        // If approved, update student's room
        if (dto.Status == "Approved" && request.Student != null)
        {
            var requestedRoom = await _context.Rooms.FindAsync(request.RequestedRoomId);
            if (requestedRoom != null)
            {
                request.Student.RoomNumber = requestedRoom.RoomNumber;
                
                // Update room occupancy
                var currentRoom = await _context.Rooms.FindAsync(request.CurrentRoomId);
                if (currentRoom != null && currentRoom.Occupied > 0)
                    currentRoom.Occupied--;

                if (requestedRoom.Occupied < requestedRoom.Capacity)
                    requestedRoom.Occupied++;
            }
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Warden")]
    public async Task<IActionResult> DeleteRoomChangeRequest(int id)
    {
        var request = await _context.RoomChangeRequests.FindAsync(id);
        if (request == null)
            return NotFound();

        _context.RoomChangeRequests.Remove(request);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}



