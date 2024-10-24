using Microsoft.AspNetCore.Mvc;
using UniConnect.Application.DTOs;
using UniConnect.Application.Interfaces;

namespace UniConnect.API.Controllers;

[ApiController]
[Route("[Controller]")]
public class EventController(IEventService eventService) : ControllerBase
{
    private readonly IEventService _eventService = eventService;

    [HttpGet("Messages")]
    public async Task<IActionResult> ListMessages()
    {
        var messages = await _eventService.ListMessages();
        return Ok(messages);
    }
    
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var events = await _eventService.List();
        return Ok(events);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEventDto createEventDto)
    {
        await _eventService.Create(createEventDto);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateEventDto updateEventDto)
    {
        await _eventService.Update(updateEventDto);
        return Ok();
    }

    [HttpPut("Enable/{id}")]
    public async Task<IActionResult> Enable([FromRoute]long id)
    {
        await _eventService.Enable(id);
        return Ok();
    }

    [HttpPut("Disable/{id}")]
    public async Task<IActionResult> Disable(long id)
    {
        await _eventService.Disable(id);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _eventService.Delete(id);
        return Ok();
    }
}