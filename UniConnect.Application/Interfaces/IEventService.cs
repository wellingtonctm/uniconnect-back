using UniConnect.Application.DTOs;
using UniConnect.Domain.Entities;

namespace UniConnect.Application.Interfaces;

public interface IEventService {
    Task<IEnumerable<UserItemDto>> ListUsers(long id);
    Task<EventItemDto> Get(long id);
    Task<IEnumerable<MessageItemDto>> ListMessages();
    Task<IEnumerable<EventItemDto>> List();
    Task Create(CreateEventDto createEventDto);
    Task Update(UpdateEventDto updateEventDto);
    Task Enable(long id);
    Task Disable(long id);
    Task Delete(long id);
}