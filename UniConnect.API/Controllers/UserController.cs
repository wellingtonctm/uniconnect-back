using Microsoft.AspNetCore.Mvc;
using UniConnect.Application.DTOs;
using UniConnect.Application.Interfaces;

namespace UniConnect.API.Controllers;

[ApiController]
[Route("[Controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("{id}/Messages")]
    public async Task<IActionResult> List(long id)
    {
        var messages = await _userService.ListMessages(id);
        return Ok(messages);
    }
    
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var users = await _userService.List();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var users = await _userService.Get(id);
        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDto createUserDto)
    {
        var user = await _userService.Create(createUserDto);
        return Ok(user);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateUserDto updateUserDto)
    {
        await _userService.Update(updateUserDto);
        return Ok();
    }

    [HttpPut("Enable/{id}")]
    public async Task<IActionResult> Enable(long id)
    {
        await _userService.Enable(id);
        return Ok();
    }

    [HttpPut("Disable/{id}")]
    public async Task<IActionResult> Disable(long id)
    {
        await _userService.Disable(id);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _userService.Delete(id);
        return Ok();
    }
}