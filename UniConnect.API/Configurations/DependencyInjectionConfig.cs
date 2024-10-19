using UniConnect.Application.Interfaces;
using UniConnect.Application.Services;
using UniConnect.Domain.Repositories;
using UniConnect.Infrastructure.Repositories;

namespace UniConnect.API.Configurations;

public static class DependencyInjectionConfig
{
    public static WebApplicationBuilder AddRepositoriesConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IEventRepository, EventRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IMessageRepository, MessageRepository>();
        return builder;
    }

    public static WebApplicationBuilder AddServicesConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IEventService, EventService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IMessageService, MessageService>();
        return builder;
    }
}
