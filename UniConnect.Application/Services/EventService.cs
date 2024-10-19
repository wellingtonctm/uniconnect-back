using UniConnect.Application.Interfaces;
using UniConnect.Domain.Repositories;

namespace UniConnect.Application.Services;

public class EventService(IEventRepository eventRepository): IEventService
{
    private readonly IEventRepository eventRepository = eventRepository;
}