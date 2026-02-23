using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HostelManagement.API.Data;
using HostelManagement.API.DTOs;

namespace HostelManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Warden")]
public class RoomsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RoomsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomDTO>>> GetRooms()
    {
        var rooms = await _context.Rooms
            .Select(r => new RoomDTO
            {
                RoomId = r.RoomId,
                RoomNumber = r.RoomNumber,
                Floor = r.Floor,
                Capacity = r.Capacity,
                Occupied = r.Occupied,
                Status = r.Status
            })
            .ToListAsync();

        return Ok(rooms);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoomDTO>> GetRoom(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null)
            return NotFound();

        return Ok(new RoomDTO
        {
            RoomId = room.RoomId,
            RoomNumber = room.RoomNumber,
            Floor = room.Floor,
            Capacity = room.Capacity,
            Occupied = room.Occupied,
            Status = room.Status
        });
    }

    [HttpPost]
    public async Task<ActionResult<RoomDTO>> CreateRoom([FromBody] CreateRoomDTO dto)
    {
        if (await _context.Rooms.AnyAsync(r => r.RoomNumber == dto.RoomNumber))
            return BadRequest(new { message = "Room number already exists" });

        var room = new Models.Room
        {
            RoomNumber = dto.RoomNumber,
            Floor = dto.Floor,
            Capacity = dto.Capacity,
            Occupied = 0,
            Status = dto.Status
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRoom), new { id = room.RoomId }, new RoomDTO
        {
            RoomId = room.RoomId,
            RoomNumber = room.RoomNumber,
            Floor = room.Floor,
            Capacity = room.Capacity,
            Occupied = room.Occupied,
            Status = room.Status
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoom(int id, [FromBody] UpdateRoomDTO dto)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null)
            return NotFound();

        room.Floor = dto.Floor;
        room.Capacity = dto.Capacity;
        room.Occupied = dto.Occupied;
        room.Status = dto.Status;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null)
            return NotFound();

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}



