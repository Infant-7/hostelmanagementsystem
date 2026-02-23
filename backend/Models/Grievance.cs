using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HostelManagement.API.Models;

public class Grievance
{
    [Key]
    public int GrievanceId { get; set; }

    [Required]
    public int StudentId { get; set; }

    [ForeignKey("StudentId")]
    public Student? Student { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Pending"; // Pending, Resolved, InProgress

    [Required]
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public DateTime? ResolvedDate { get; set; }

    [MaxLength(450)]
    public string? ResolvedBy { get; set; } // UserId of Admin/Warden who resolved
}



