using UniConnect.Domain.Entities;
using UniConnect.Domain.Repositories;
using UniConnect.Infrastructure.Data;

namespace UniConnect.Infrastructure.Repositories;

public class EventRepository(AppDbContext context) : BaseRepository<Event>(context), IEventRepository
{
}
