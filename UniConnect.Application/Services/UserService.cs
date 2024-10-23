using UniConnect.Application.DTOs;
using UniConnect.Application.Interfaces;
using UniConnect.Domain.Entities;
using UniConnect.Domain.Messaging;
using UniConnect.Domain.Repositories;

namespace UniConnect.Application.Services;

public class UserService(IUserRepository userRepository, IEventRepository eventRepository, IMessageRepository messageRepository) : IUserService
{
    private readonly IUserRepository userRepository = userRepository;
    private readonly IEventRepository eventRepository = eventRepository;
    private readonly IMessageRepository messageRepository = messageRepository;

    public async Task<IEnumerable<MessageDto>> ListMessages(long id)
    {
        var user = await userRepository.FindAsync(x => x.Id == id && x.Enabled) ?? throw new Exception("Usuário não encontrado.");
        var messages = await messageRepository.FindAllAsync(x => x.UserId == id && x.Enabled);

        var messageDtos = new List<MessageDto>();

        foreach (var message in messages)
        {
            messageDtos.Add(new MessageDto
            {
                Id = message.Id,
                Message = message.Content,
                SentDate = message.SentAt,
                User = message.User?.Name
            });
        }

        return messageDtos.OrderBy(x => x.SentDate);
    }

    public async Task<IEnumerable<User>> List()
    {
        return await userRepository.GetAllAsync();
    }

    public async Task<User> Create(CreateUserDto createUserDto)
    {
        var enabledEvent = await eventRepository.FindAsync(x => x.Enabled == true) ?? throw new Exception("Não há eventos abertos.");

        if (string.IsNullOrWhiteSpace(createUserDto.Name))
            throw new Exception("O nome do usuário não pode ser vazio.");

        var newUser = new User
        {
            EventId = enabledEvent.Id,
            Name = createUserDto.Name,
            CreatedAt = DateTime.Now,
            Enabled = true
        };

        await userRepository.AddAsync(newUser);
        return newUser;
    }

    public async Task Update(UpdateUserDto updateUserDto)
    {
        if (string.IsNullOrWhiteSpace(updateUserDto.Name))
            throw new Exception("O nome do usuário não pode ser vazio.");

        var userToUpdate = await userRepository.GetByIdAsync(updateUserDto.Id) ?? throw new Exception("Usuário não encontrado.");
        userToUpdate.Name = updateUserDto.Name;
        await userRepository.UpdateAsync(userToUpdate);

    }

    public async Task Delete(long id)
    {
        var userToDelete = await userRepository.GetByIdAsync(id) ?? throw new Exception("Usuário não encontrado.");
        await userRepository.DeleteAsync(userToDelete);
    }
}