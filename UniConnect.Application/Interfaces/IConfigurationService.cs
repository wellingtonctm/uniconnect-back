using UniConnect.Domain.Entities;

namespace UniConnect.Application.Interfaces;

public interface IConfigurationService
{
    Task<IEnumerable<Configuration>> List();
    Task<Configuration> Create();
    Task Delete(long id);
}