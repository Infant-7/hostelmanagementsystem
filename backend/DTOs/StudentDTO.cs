namespace HostelManagement.API.DTOs;

public class StudentDTO
{
    public int StudentId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string RollNumber { get; set; } = string.Empty;
    public string? RoomNumber { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
}

public class CreateStudentDTO
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string RollNumber { get; set; } = string.Empty;
    public string? RoomNumber { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
}

public class UpdateStudentDTO
{
    public string Name { get; set; } = string.Empty;
    public string? RoomNumber { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
}



