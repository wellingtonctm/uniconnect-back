namespace UniConnect.Domain.Entities;

public class User
{
    public required long Id { get; set; }
    public required long EventId { get; set; }
    public Event? Event { get; set; }
    public required string Name { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required bool Enabled { get; set; }
}