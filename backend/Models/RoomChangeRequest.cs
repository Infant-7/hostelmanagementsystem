using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HostelManagement.API.Models;

public class RoomChangeRequest
{
    [Key]
    public int RequestId { get; set; }

    [Required]
    public int StudentId { get; set; }

    [ForeignKey("StudentId")]
    public Student? Student { get; set; }

    [Required]
    public int CurrentRoomId { get; set; }

    [ForeignKey("CurrentRoomId")]
    public Room? CurrentRoom { get; set; }

    [Required]
    public int RequestedRoomId { get; set; }

    [ForeignKey("RequestedRoomId")]
    public Room? RequestedRoom { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Reason { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

    [MaxLength(450)]
    public string? ApprovedBy { get; set; } // UserId of Admin/Warden who approved
}



