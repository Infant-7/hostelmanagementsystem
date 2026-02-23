using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HostelManagement.API.Models;

public class Warden
{
    [Key]
    public int WardenId { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [ForeignKey("UserId")]
    public ApplicationUser? User { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(15)]
    public string? Phone { get; set; }

    [MaxLength(100)]
    public string? Department { get; set; }
}



