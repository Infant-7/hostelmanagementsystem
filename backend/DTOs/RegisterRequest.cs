using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HostelManagement.API.DTOs;

[NotMapped]
public class RegisterRequest
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = string.Empty; // Student, Admin, Warden

    // Student specific
    public string? Name { get; set; }
    public string? RollNumber { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }

    // Warden specific
    public string? Department { get; set; }
}

