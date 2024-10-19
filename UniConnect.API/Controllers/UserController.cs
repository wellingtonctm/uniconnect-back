using Microsoft.AspNetCore.Mvc;
using UniConnect.Application.DTOs;
using UniConnect.Application.Interfaces;

namespace UniConnect.API.Controllers;

[ApiController]
[Route("[Controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var users = await _userService.List();
        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDto createUserDto)
    {
        await _userService.Create(createUserDto);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateUserDto updateUserDto)
    {
        await _userService.Update(updateUserDto);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _userService.Delete(id);
        return Ok();
    }
}