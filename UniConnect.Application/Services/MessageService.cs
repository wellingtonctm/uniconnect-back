using UniConnect.Application.DTOs;
using UniConnect.Application.Interfaces;
using UniConnect.Domain.Entities;
using UniConnect.Domain.Messaging;
using UniConnect.Domain.Repositories;

namespace UniConnect.Application.Services;

public class MessageService(IMessageRepository messageRepository, IEventRepository eventRepository, IUserRepository userRepository, IWebSocketConnectionManager connectionManager) : IMessageService
{
    private readonly IMessageRepository messageRepository = messageRepository;
    private readonly IEventRepository eventRepository = eventRepository;
    private readonly IUserRepository userRepository = userRepository;
    private readonly IWebSocketConnectionManager _connectionManager = connectionManager;

    public async Task<IEnumerable<MessageItemDto>> List()
    {
        var messages = await messageRepository.GetAllAsync();
        var items = new List<MessageItemDto>();

        foreach (var message in messages)
        {
            var user = await userRepository.GetByIdAsync(message.UserId);

            items.Add(new MessageItemDto
            {
                Content = message.Content,
                Enabled = message.Enabled,
                Id = message.Id,
                SentAt = message.SentAt,
                UserId = user?.Id,
                UserName = user?.Name
            });
        }

        return items.OrderByDescending(x => x.SentAt);
    }

    public async Task Create(CreateMessageDto createMessageDto)
    {
        var enabledEvent = await eventRepository.FindAsync(x => x.Enabled == true) ?? throw new Exception("Não há eventos abertos.");
        var user = await userRepository.GetByIdAsync(createMessageDto.UserId) ?? throw new Exception("usuário não encontrado.");

        if (user.EventId != enabledEvent.Id)
            throw new Exception("Usuário não pertence ao evento em aberto.");

        if (string.IsNullOrWhiteSpace(createMessageDto.Content))
            throw new Exception("O conteúdo da mensagem não pode ser vazio.");

        var newMessage = new Message
        {
            UserId = createMessageDto.UserId,
            Content = createMessageDto.Content,
            SentAt = DateTime.UtcNow,
            Enabled = true
        };

        await messageRepository.AddAsync(newMessage);

        var messageDto = new MessageItemDto
        {
            Id = newMessage.Id,
            Content = newMessage.Content,
            SentAt = newMessage.SentAt,
            UserName = newMessage.User?.Name,
            Enabled = newMessage.Enabled,
            UserId = newMessage.UserId
        };

        await _connectionManager.SendMessageToAll(messageDto);
    }

    public async Task Update(UpdateMessageDto updateMessageDto)
    {
        if (string.IsNullOrWhiteSpace(updateMessageDto.Content))
            throw new Exception("O conteúdo da mensagem não pode ser vazio.");

        var MessageToUpdate = await messageRepository.GetByIdAsync(updateMessageDto.Id) ?? throw new Exception("Mensagem não encontrada.");
        MessageToUpdate.Content = updateMessageDto.Content;
        await messageRepository.UpdateAsync(MessageToUpdate);
    }

    public async Task Enable(long id)
    {
        var messageToEnable = await messageRepository.GetByIdAsync(id) ?? throw new Exception("Mensagem não encontrada.");
        messageToEnable.Enabled = true;
        await messageRepository.UpdateAsync(messageToEnable);

        var messageDto = new MessageItemDto
        {
            Id = messageToEnable.Id,
            Content = messageToEnable.Content,
            SentAt = messageToEnable.SentAt,
            UserName = (await userRepository.GetByIdAsync(messageToEnable.UserId))?.Name,
            Enabled = messageToEnable.Enabled,
            UserId = messageToEnable.UserId
        };

        await _connectionManager.SendMessageToAll(messageDto);
    }

    public async Task Disable(long id)
    {
        var messageToDisable = await messageRepository.GetByIdAsync(id) ?? throw new Exception("Mensagem não encontrada.");
        messageToDisable.Enabled = false;
        await messageRepository.UpdateAsync(messageToDisable);

        var messageDto = new MessageItemDto
        {
            Id = messageToDisable.Id,
            Content = messageToDisable.Content,
            SentAt = messageToDisable.SentAt,
            UserName = (await userRepository.GetByIdAsync(messageToDisable.UserId))?.Name,
            Enabled = messageToDisable.Enabled,
            UserId = messageToDisable.UserId
        };

        await _connectionManager.SendMessageToAll(messageDto);
    }

    public async Task Delete(long id)
    {
        var MessageToDelete = await messageRepository.GetByIdAsync(id) ?? throw new Exception("Mensagem não encontrada.");
        await messageRepository.DeleteAsync(MessageToDelete);
    }
}