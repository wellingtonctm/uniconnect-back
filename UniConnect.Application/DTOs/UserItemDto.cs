namespace UniConnect.Application.DTOs;

public class UserItemDto
{
    public long? Id { get; set; }
    public long? EventId { get; set; }
    public string? Name { get; set; }
    public DateTime? CreatedAt { get; set; }
    public bool? Enabled { get; set; }
    public long? MessagesNumber { get; set; }
    public string Type { get; } = "User";
}