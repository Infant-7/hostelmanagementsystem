namespace HostelManagement.API.DTOs;

public class AttendanceDTO
{
    public int AttendanceId { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string RollNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? MarkedBy { get; set; }
}

public class CreateAttendanceDTO
{
    public int StudentId { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; } = "Present";
}

public class UpdateAttendanceDTO
{
    public DateTime Date { get; set; }
    public string Status { get; set; } = "Present";
}



