namespace HostelManagement.API.DTOs;

public class GrievanceDTO
{
    public int GrievanceId { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string RollNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? ResolvedDate { get; set; }
    public string? ResolvedBy { get; set; }
}

public class CreateGrievanceDTO
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class ResolveGrievanceDTO
{
    public string Status { get; set; } = "Resolved"; // Resolved or InProgress
}



