namespace UniConnect.Domain.Entities;

public class User
{
    public long Id { get; set; }
    public long EventId { get; set; }
    public Event? Event { get; set; }
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Enabled { get; set; }
}