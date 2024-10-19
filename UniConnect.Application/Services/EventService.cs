using UniConnect.Application.DTOs;
using UniConnect.Application.Interfaces;
using UniConnect.Domain.Entities;
using UniConnect.Domain.Repositories;

namespace UniConnect.Application.Services;

public class EventService(IEventRepository eventRepository): IEventService
{
    private readonly IEventRepository eventRepository = eventRepository;

    public async Task<IEnumerable<Event>> List() {
        return await eventRepository.GetAllAsync();
    }

    public async Task Create(CreateEventDto createEventDto) {
        if (string.IsNullOrWhiteSpace(createEventDto.Description))
            throw new Exception("A descrição do evento não pode ser vazia.");
        
        var newEvent = new Event {
            Description = createEventDto.Description,
            CreatedAt = DateTime.Now,
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
        await eventRepository.UpdateAsync(eventToUpdate);

    }

    public async Task Delete(long id) {
        var eventToDelete = await eventRepository.GetByIdAsync(id) ?? throw new Exception("Evento não encontrado.");
        await eventRepository.DeleteAsync(eventToDelete);
    }
}