using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HostelManagement.API.Models;

public class Attendance
{
    [Key]
    public int AttendanceId { get; set; }

    [Required]
    public int StudentId { get; set; }

    [ForeignKey("StudentId")]
    public Student? Student { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Present"; // Present, Absent

    [MaxLength(450)]
    public string? MarkedBy { get; set; } // UserId of Admin/Warden who marked
}



