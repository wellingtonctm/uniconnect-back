namespace UniConnect.Application.DTOs;

public class MessageDto
{
    public long? Id { get; set; }
    public string? Message { get; set; }
    public string? User { get; set; }
    public DateTime SentDate { get; set; }
}