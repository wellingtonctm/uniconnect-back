using UniConnect.Domain.Entities;
using UniConnect.Domain.Repositories;
using UniConnect.Infrastructure.Data;

namespace UniConnect.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : BaseRepository<User>(context), IUserRepository
{
}
