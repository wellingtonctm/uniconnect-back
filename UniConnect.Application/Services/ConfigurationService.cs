using UniConnect.Application.Interfaces;
using UniConnect.Domain.Entities;
using UniConnect.Domain.Repositories;

namespace UniConnect.Application.Services;

public class ConfigurationService(IConfigurationRepository configurationRepository, IEventRepository eventRepository) : IConfigurationService
{
    private readonly IConfigurationRepository configurationRepository = configurationRepository;
    private readonly IEventRepository eventRepository = eventRepository;

    public async Task<IEnumerable<Configuration>> List()
    {
        return await configurationRepository.GetAllAsync();
    }

    public async Task<Configuration> Create()
    {
        var configuration = new Configuration();
        await configurationRepository.AddAsync(configuration);
        return configuration;
    }

    public async Task Delete(long id)
    {
        var configurationToDelete = await configurationRepository.GetByIdAsync(id) ?? throw new Exception("Configuração não encontrada.");
        await configurationRepository.DeleteAsync(configurationToDelete);
    }
}