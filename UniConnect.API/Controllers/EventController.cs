using Microsoft.AspNetCore.Mvc;
using UniConnect.Application.Interfaces;

namespace UniConnect.API.Controllers;

[ApiController]
[Route("[Controller]")]
public class EventController(IEventService eventService) : ControllerBase
{
    private readonly IEventService _eventService = eventService;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] string description)
    {
        await _eventService.Create(description);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _eventService.Delete(id);
        return Ok();
    }
}