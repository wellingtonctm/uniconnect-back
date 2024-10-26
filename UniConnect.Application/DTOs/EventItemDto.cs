namespace UniConnect.Application.DTOs;

public class EventItemDto
{
    public long? Id { get; set; }
    public string? Description { get; set; }
    public int? LayoutNumberCols { get; set; }
    public DateTime? CreatedAt { get; set; }
    public bool? Enabled { get; set; }
    public long? UsersNumber { get; set; }
    public long? MessagesNumber { get; set; }
    public string Type { get; } = "Event";
}