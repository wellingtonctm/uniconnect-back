namespace UniConnect.Domain.Entities;

public class Event
{
    public long Id { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Enabled { get; set; }
}