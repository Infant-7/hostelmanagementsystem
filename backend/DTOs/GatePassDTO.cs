namespace HostelManagement.API.DTOs;

public class GatePassDTO
{
    public int RequestId { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string RollNumber { get; set; } = string.Empty;
    public DateTime RequestDate { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public DateTime OutTime { get; set; }
    public DateTime? InTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ApprovedBy { get; set; }
}

public class CreateGatePassDTO
{
    public DateTime RequestDate { get; set; } = DateTime.Now;
    public string Purpose { get; set; } = string.Empty;
    public DateTime OutTime { get; set; }
    public DateTime? InTime { get; set; }
}

public class ApproveGatePassDTO
{
    public string Status { get; set; } = "Approved"; // Approved or Rejected
}



