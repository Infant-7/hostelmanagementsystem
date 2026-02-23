namespace HostelManagement.API.DTOs;

public class RoomDTO
{
    public int RoomId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public int Floor { get; set; }
    public int Capacity { get; set; }
    public int Occupied { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class CreateRoomDTO
{
    public string RoomNumber { get; set; } = string.Empty;
    public int Floor { get; set; }
    public int Capacity { get; set; }
    public string Status { get; set; } = "Available";
}

public class UpdateRoomDTO
{
    public int Floor { get; set; }
    public int Capacity { get; set; }
    public int Occupied { get; set; }
    public string Status { get; set; } = "Available";
}



