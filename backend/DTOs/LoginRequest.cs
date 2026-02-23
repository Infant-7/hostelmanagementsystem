using System.ComponentModel.DataAnnotations;

namespace HostelManagement.API.DTOs;

public class LoginRequest
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = string.Empty; // Student, Admin, Warden
}



