using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HostelManagement.API.Models;

public class GatePassRequest
{
    [Key]
    public int RequestId { get; set; }

    [Required]
    public int StudentId { get; set; }

    [ForeignKey("StudentId")]
    public Student? Student { get; set; }

    [Required]
    public DateTime RequestDate { get; set; }

    [Required]
    [MaxLength(500)]
    public string Purpose { get; set; } = string.Empty;

    [Required]
    public DateTime OutTime { get; set; }

    public DateTime? InTime { get; set; }

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

    [MaxLength(450)]
    public string? ApprovedBy { get; set; } // UserId of Admin/Warden who approved
}



