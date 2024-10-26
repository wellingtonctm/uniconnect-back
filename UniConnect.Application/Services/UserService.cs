using UniConnect.Application.DTOs;
using UniConnect.Application.Interfaces;
using UniConnect.Domain.Entities;
using UniConnect.Domain.Messaging;
using UniConnect.Domain.Repositories;

namespace UniConnect.Application.Services;

public class UserService(IUserRepository userRepository, IEventRepository eventRepository, IMessageRepository messageRepository, IWebSocketConnectionManager connectionManager) : IUserService
{
    private readonly IUserRepository userRepository = userRepository;
    private readonly IEventRepository eventRepository = eventRepository;
    private readonly IMessageRepository messageRepository = messageRepository;
    private readonly IWebSocketConnectionManager _connectionManager = connectionManager;

    public async Task<UserItemDto> Get(long id)
    {
        var user = await userRepository.GetByIdAsync(id) ?? throw new Exception("Usuário não encontrado.");

        var userDto = new UserItemDto
        {
            Id = user.Id,
            CreatedAt = user.CreatedAt,
            Enabled = user.Enabled,
            EventId = user.EventId,
            Name = user.Name
        };

        return userDto;
    }

    public async Task<IEnumerable<MessageItemDto>> ListMessages(long id)
    {
        var user = await userRepository.FindAsync(x => x.Id == id) ?? throw new Exception("Usuário não encontrado.");
        var messages = await messageRepository.FindAllAsync(x => x.UserId == id);

        var messageDtos = new List<MessageItemDto>();

        foreach (var message in messages)
        {
            messageDtos.Add(new MessageItemDto
            {
                Id = message.Id,
                Content = message.Content,
                SentAt = message.SentAt,
                UserName = message.User?.Name,
                Enabled = message.Enabled,
                UserId = message.UserId
            });
        }

        return messageDtos.OrderByDescending(x => x.SentAt);
    }

    public async Task<IEnumerable<UserItemDto>> List()
    {
        var users = await userRepository.GetAllAsync();
        var items = new List<UserItemDto>();

        foreach (var user in users)
        {
            var messages = await messageRepository.FindAllAsync(x => x.UserId == user.Id);

            items.Add(new UserItemDto
            {
                Id = user.Id,
                CreatedAt = user.CreatedAt,
                Enabled = user.Enabled,
                EventId = user.EventId,
                Name = user.Name,
                MessagesNumber = messages.Count()
            });
        }

        return items.OrderByDescending(x => x.CreatedAt);
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
            CreatedAt = DateTime.UtcNow,
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

    public async Task Enable(long id)
    {
        var userToEnable = await userRepository.GetByIdAsync(id) ?? throw new Exception("Usuário não encontrado.");
        userToEnable.Enabled = true;
        await userRepository.UpdateAsync(userToEnable);

        var userDto = new UserItemDto
        {
            Id = userToEnable.Id,
            CreatedAt = userToEnable.CreatedAt,
            Enabled = userToEnable.Enabled,
            EventId = userToEnable.EventId,
            Name = userToEnable.Name,
        };

        await _connectionManager.SendMessageToAll(userDto);
    }

    public async Task Disable(long id)
    {
        var userToDisable = await userRepository.GetByIdAsync(id) ?? throw new Exception("Usuário não encontrado.");
        userToDisable.Enabled = false;
        await userRepository.UpdateAsync(userToDisable);

        var userDto = new UserItemDto
        {
            Id = userToDisable.Id,
            CreatedAt = userToDisable.CreatedAt,
            Enabled = userToDisable.Enabled,
            EventId = userToDisable.EventId,
            Name = userToDisable.Name,
        };

        await _connectionManager.SendMessageToAll(userDto);
    }

    public async Task Delete(long id)
    {
        var userToDelete = await userRepository.GetByIdAsync(id) ?? throw new Exception("Usuário não encontrado.");
        await userRepository.DeleteAsync(userToDelete);
    }
}