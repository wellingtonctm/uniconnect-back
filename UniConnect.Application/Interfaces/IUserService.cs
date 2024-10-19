using UniConnect.Application.DTOs;
using UniConnect.Domain.Entities;

namespace UniConnect.Application.Interfaces;

public interface IUserService {
    Task<IEnumerable<User>> List();
    Task Create(CreateUserDto createUserDto);
    Task Update(UpdateUserDto updateUserDto);
    Task Delete(long id);
}