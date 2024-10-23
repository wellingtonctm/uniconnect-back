using UniConnect.Application.DTOs;
using UniConnect.Domain.Entities;

namespace UniConnect.Application.Interfaces;

public interface IEventService {
    Task<IEnumerable<MessageDto>> ListMessages();
    Task<IEnumerable<Event>> List();
    Task Create(CreateEventDto createEventDto);
    Task Update(UpdateEventDto updateEventDto);
    Task Delete(long id);
}