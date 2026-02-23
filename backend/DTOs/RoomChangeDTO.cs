namespace HostelManagement.API.DTOs;

public class RoomChangeDTO
{
    public int RequestId { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string RollNumber { get; set; } = string.Empty;
    public int CurrentRoomId { get; set; }
    public string CurrentRoomNumber { get; set; } = string.Empty;
    public int RequestedRoomId { get; set; }
    public string RequestedRoomNumber { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? ApprovedBy { get; set; }
}

public class CreateRoomChangeDTO
{
    public int CurrentRoomId { get; set; }
    public int RequestedRoomId { get; set; }
    public string Reason { get; set; } = string.Empty;
}

public class ApproveRoomChangeDTO
{
    public string Status { get; set; } = "Approved"; // Approved or Rejected
}



