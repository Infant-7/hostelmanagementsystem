using System.ComponentModel.DataAnnotations;

namespace HostelManagement.API.Models;

public class Room
{
    [Key]
    public int RoomId { get; set; }

    [Required]
    [MaxLength(20)]
    public string RoomNumber { get; set; } = string.Empty;

    [Required]
    public int Floor { get; set; }

    [Required]
    public int Capacity { get; set; }

    public int Occupied { get; set; } = 0;

    [MaxLength(50)]
    public string Status { get; set; } = "Available"; // Available, Occupied, Maintenance
}



