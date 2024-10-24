namespace UniConnect.Application.DTOs;

public class UpdateEventDto
{
    public long Id { get; set; }
    public string? Description { get; set; }
    public int? LayoutNumberCols { get; set; }
    public bool Enabled { get; set; }
}