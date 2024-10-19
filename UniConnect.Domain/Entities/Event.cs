namespace UniConnect.Domain.Entities;

public class Event
{
    public required long Id { get; set; }
    public required string Description { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required bool Enabled { get; set; }
}