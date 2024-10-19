using UniConnect.Domain.Entities;
using UniConnect.Domain.Repositories;
using UniConnect.Infrastructure.Data;

namespace UniConnect.Infrastructure.Repositories;

public class MessageRepository(AppDbContext context) : BaseRepository<Message>(context), IMessageRepository
{
}
