namespace UniConnect.Domain.Entities;

public class Message
{
    public required long Id { get; set; }
    public required long UserId { get; set; }
    public User? User { get; set; }
    public required string Content { get; set; }
    public required DateTime SentAt { get; set; }
    public required bool Enabled { get; set; }
}