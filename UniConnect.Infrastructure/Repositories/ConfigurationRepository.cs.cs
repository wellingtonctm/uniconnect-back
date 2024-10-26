using UniConnect.Domain.Entities;
using UniConnect.Domain.Repositories;
using UniConnect.Infrastructure.Data;

namespace UniConnect.Infrastructure.Repositories;

public class ConfigurationRepository(AppDbContext context) : BaseRepository<Configuration>(context), IConfigurationRepository
{
}
