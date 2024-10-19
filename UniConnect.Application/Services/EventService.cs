using UniConnect.Application.Interfaces;
using UniConnect.Domain.Entities;
using UniConnect.Domain.Repositories;

namespace UniConnect.Application.Services;

public class EventService(IEventRepository eventRepository): IEventService
{
    private readonly IEventRepository eventRepository = eventRepository;

    public async Task Create(string description) {
        var newEvent = new Event {
            Description = description,
            CreatedAt = DateTime.Now,
            Enabled = false
        };

        await eventRepository.AddAsync(newEvent);
    }

    public async Task Delete(long id) {
        var eventToDelete = await eventRepository.GetByIdAsync(id) ?? throw new Exception("Evento não encontrado.");
        await eventRepository.DeleteAsync(eventToDelete);
    }
}