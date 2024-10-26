using Microsoft.AspNetCore.Mvc;
using UniConnect.Application.DTOs;
using UniConnect.Application.Interfaces;

namespace UniConnect.API.Controllers;

[ApiController]
[Route("[Controller]")]
public class MessageController(IMessageService messageService) : ControllerBase
{
    private readonly IMessageService _messageService = messageService;

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var messages = await _messageService.List();
        return Ok(messages);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMessageDto createMessageDto)
    {
        await _messageService.Create(createMessageDto);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateMessageDto updateMessageDto)
    {
        await _messageService.Update(updateMessageDto);
        return Ok();
    }

    [HttpPut("Enable/{id}")]
    public async Task<IActionResult> Enable(long id)
    {
        await _messageService.Enable(id);
        return Ok();
    }

    [HttpPut("Disable/{id}")]
    public async Task<IActionResult> Disable(long id)
    {
        await _messageService.Disable(id);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _messageService.Delete(id);
        return Ok();
    }
}