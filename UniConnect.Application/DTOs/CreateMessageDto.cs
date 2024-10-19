namespace UniConnect.Application.DTOs;

public class CreateMessageDto
{
    public long UserId { get; set; }
    public string? Content { get; set; }
}