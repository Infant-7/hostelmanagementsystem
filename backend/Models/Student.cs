using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HostelManagement.API.Models;

public class Student
{
    [Key]
    public int StudentId { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [ForeignKey("UserId")]
    public ApplicationUser? User { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string RollNumber { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? RoomNumber { get; set; }

    [MaxLength(15)]
    public string? Phone { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    // Navigation properties
    public ICollection<Attendance>? Attendances { get; set; }
    public ICollection<GatePassRequest>? GatePassRequests { get; set; }
    public ICollection<RoomChangeRequest>? RoomChangeRequests { get; set; }
    public ICollection<Grievance>? Grievances { get; set; }
}



