namespace UniConnect.Domain.Entities;

public class Configuration
{
    public long Id { get; set; }
    public long EventId { get; set; }
    public int MasonryLayoutCols { get; set; }
    public DateTime CreatedAt { get; set; }
}