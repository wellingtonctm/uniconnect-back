using UniConnect.Application.DTOs;
using UniConnect.Application.Interfaces;
using UniConnect.Domain.Entities;
using UniConnect.Domain.Repositories;

namespace UniConnect.Application.Services;

public class EventService(IEventRepository eventRepository, IUserRepository userRepository, IMessageRepository messageRepository) : IEventService
{
    private readonly IEventRepository eventRepository = eventRepository;
    private readonly IUserRepository userRepository = userRepository;
    private readonly IMessageRepository messageRepository = messageRepository;

    public async Task<IEnumerable<MessageDto>> ListMessages()
    {
        var enabledEvent = await eventRepository.FindAsync(x => x.Enabled == true) ?? throw new Exception("Não há eventos abertos.");
        var userIds = (await userRepository.FindAllAsync(x => x.EventId == enabledEvent.Id && x.Enabled)).Select(x => x.Id);
        var messages = await messageRepository.FindAllAsync(x => userIds.Contains(x.UserId) && x.Enabled);

        var messageDtos = new List<MessageDto>();

        foreach (var message in messages) {
            messageDtos.Add(new MessageDto {
                Id = message.Id,
                Message = message.Content,
                SentDate = message.SentAt,
                User = message.User?.Name
            });
        }

        return messageDtos;
    }

    public async Task<IEnumerable<Event>> List()
    {
        return await eventRepository.GetAllAsync();
    }

    public async Task Create(CreateEventDto createEventDto)
    {
        if (string.IsNullOrWhiteSpace(createEventDto.Description))
            throw new Exception("A descrição do evento não pode ser vazia.");

        var newEvent = new Event
        {
            Description = createEventDto.Description,
            CreatedAt = DateTime.UtcNow,
            Enabled = false
        };

        await eventRepository.AddAsync(newEvent);
    }

    public async Task Update(UpdateEventDto updateEventDto)
    {
        if (string.IsNullOrWhiteSpace(updateEventDto.Description))
            throw new Exception("A descrição do evento não pode ser vazia.");

        var eventToUpdate = await eventRepository.GetByIdAsync(updateEventDto.Id) ?? throw new Exception("Evento não encontrado.");
        eventToUpdate.Description = updateEventDto.Description;

        if (updateEventDto.Enabled)
        {
            var enabledEvent = await eventRepository.FindAsync(x => x.Id != updateEventDto.Id && x.Enabled == true);

            if (enabledEvent is not null)
                throw new Exception("Outro evento já está aberto.");
        }

        eventToUpdate.Enabled = updateEventDto.Enabled;
        await eventRepository.UpdateAsync(eventToUpdate);

    }

    public async Task Delete(long id)
    {
        var eventToDelete = await eventRepository.GetByIdAsync(id) ?? throw new Exception("Evento não encontrado.");
        await eventRepository.DeleteAsync(eventToDelete);
    }
}