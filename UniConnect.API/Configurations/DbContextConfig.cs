using Microsoft.EntityFrameworkCore;
using UniConnect.Infrastructure.Data;

namespace UniConnect.API.Configurations;

public static class DbContextConfig
{
    public static WebApplicationBuilder AddDbContextConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(
           options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
        );

        return builder;
    }
}
