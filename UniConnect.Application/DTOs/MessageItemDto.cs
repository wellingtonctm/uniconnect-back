namespace UniConnect.Application.DTOs;

public class MessageItemDto
{
    public long? Id { get; set; }
    public long? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Content { get; set; }
    public DateTime? SentAt { get; set; }
    public bool? Enabled { get; set; }
    public string Type { get; } = "Message";
}