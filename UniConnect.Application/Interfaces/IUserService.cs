using UniConnect.Application.DTOs;
using UniConnect.Domain.Entities;

namespace UniConnect.Application.Interfaces;

public interface IUserService
{
    Task<UserItemDto> Get(long id);
    Task<IEnumerable<MessageItemDto>> ListMessages(long id);
    Task<IEnumerable<UserItemDto>> List();
    Task<User> Create(CreateUserDto createUserDto);
    Task Update(UpdateUserDto updateUserDto);
    Task Enable(long id);
    Task Disable(long id);
    Task Delete(long id);
}