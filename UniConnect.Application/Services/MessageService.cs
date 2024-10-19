using UniConnect.Application.DTOs;
using UniConnect.Application.Interfaces;
using UniConnect.Domain.Entities;
using UniConnect.Domain.Repositories;

namespace UniConnect.Application.Services;

public class MessageService(IMessageRepository MessageRepository, IEventRepository eventRepository, IUserRepository userRepository) : IMessageService
{
    private readonly IMessageRepository MessageRepository = MessageRepository;
    private readonly IEventRepository eventRepository = eventRepository;
    private readonly IUserRepository userRepository = userRepository;

    public async Task<IEnumerable<Message>> List()
    {
        return await MessageRepository.GetAllAsync();
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
            SentAt = DateTime.Now,
            Enabled = true
        };

        await MessageRepository.AddAsync(newMessage);
    }

    public async Task Update(UpdateMessageDto updateMessageDto)
    {
        if (string.IsNullOrWhiteSpace(updateMessageDto.Content))
            throw new Exception("O conteúdo da mensagem não pode ser vazio.");

        var MessageToUpdate = await MessageRepository.GetByIdAsync(updateMessageDto.Id) ?? throw new Exception("Mensagem não encontrada.");
        MessageToUpdate.Content = updateMessageDto.Content;
        await MessageRepository.UpdateAsync(MessageToUpdate);

    }

    public async Task Delete(long id)
    {
        var MessageToDelete = await MessageRepository.GetByIdAsync(id) ?? throw new Exception("Mensagem não encontrada.");
        await MessageRepository.DeleteAsync(MessageToDelete);
    }
}