using Microsoft.AspNetCore.Identity;

namespace HostelManagement.API.Models;

public class ApplicationUser : IdentityUser
{
    public string Role { get; set; } = string.Empty;
}



