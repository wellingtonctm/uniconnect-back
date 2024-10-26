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

    public async Task<IEnumerable<UserItemDto>> ListUsers(long id)
    {
        var e = await eventRepository.GetByIdAsync(id) ?? throw new Exception("Evento não encontrado.");
        var users = await userRepository.FindAllAsync(x => x.EventId == e.Id);
        var items = new List<UserItemDto>();

        foreach (var user in users)
        {
            items.Add(new UserItemDto
            {
                Id = user.Id,
                CreatedAt = user.CreatedAt,
                Enabled = user.Enabled,
                EventId = user.EventId,
                Name = user.Name
            });
        }


        return items.OrderByDescending(x => x.CreatedAt);
    }

    public async Task<EventItemDto> Get(long id)
    {
        var e = await eventRepository.GetByIdAsync(id) ?? throw new Exception("Evento não encontrado.");

        var eventDto = new EventItemDto
        {
            Id = e.Id,
            CreatedAt = e.CreatedAt,
            Description = e.Description,
            Enabled = e.Enabled,
            LayoutNumberCols = e.LayoutNumberCols
        };

        return eventDto;
    }

    public async Task<IEnumerable<MessageItemDto>> ListMessages()
    {
        var enabledEvent = await eventRepository.FindAsync(x => x.Enabled == true) ?? throw new Exception("Não há eventos abertos.");
        var userIds = (await userRepository.FindAllAsync(x => x.EventId == enabledEvent.Id && x.Enabled)).Select(x => x.Id);
        var messages = await messageRepository.FindAllAsync(x => userIds.Contains(x.UserId) && x.Enabled);

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

    public async Task<IEnumerable<EventItemDto>> List()
    {
        var events = await eventRepository.GetAllAsync();
        var items = new List<EventItemDto>();

        foreach (var e in events)
        {
            var usersIds = (await userRepository.FindAllAsync(x => x.EventId == e.Id)).Select(x => x.Id);
            var messages = await messageRepository.FindAllAsync(x => usersIds.Contains(x.UserId));

            items.Add(new EventItemDto
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                Description = e.Description,
                Enabled = e.Enabled,
                LayoutNumberCols = e.LayoutNumberCols,
                MessagesNumber = messages.Count(),
                UsersNumber = usersIds.Count()
            });
        }


        return items.OrderByDescending(x => x.CreatedAt);
    }

    public async Task Create(CreateEventDto createEventDto)
    {
        if (string.IsNullOrWhiteSpace(createEventDto.Description))
            throw new Exception("A descrição do evento não pode ser vazia.");

        var newEvent = new Event
        {
            Description = createEventDto.Description,
            LayoutNumberCols = createEventDto.LayoutNumberCols,
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
        eventToUpdate.LayoutNumberCols = updateEventDto.LayoutNumberCols;

        if (updateEventDto.Enabled)
        {
            var enabledEvent = await eventRepository.FindAsync(x => x.Id != updateEventDto.Id && x.Enabled == true);

            if (enabledEvent is not null)
                throw new Exception("Outro evento já está aberto.");
        }

        eventToUpdate.Enabled = updateEventDto.Enabled;
        await eventRepository.UpdateAsync(eventToUpdate);

    }

    public async Task Enable(long id)
    {
        var eventToEnable = await eventRepository.GetByIdAsync(id) ?? throw new Exception("Evento não encontrado.");
        var enabledEvent = await eventRepository.FindAsync(x => x.Id != eventToEnable.Id && x.Enabled == true);

        if (enabledEvent is not null)
            throw new Exception("Outro evento já está aberto.");

        eventToEnable.Enabled = true;
        await eventRepository.UpdateAsync(eventToEnable);
    }

    public async Task Disable(long id)
    {
        var eventToDisable = await eventRepository.GetByIdAsync(id) ?? throw new Exception("Evento não encontrado.");
        eventToDisable.Enabled = false;
        await eventRepository.UpdateAsync(eventToDisable);
    }

    public async Task Delete(long id)
    {
        var eventToDelete = await eventRepository.GetByIdAsync(id) ?? throw new Exception("Evento não encontrado.");
        await eventRepository.DeleteAsync(eventToDelete);
    }
}