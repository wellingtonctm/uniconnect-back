using Microsoft.EntityFrameworkCore;
using UniConnect.Infrastructure.Data;

namespace UniConnect.API.Configurations;

public static class DbContextConfig
{
    public static WebApplicationBuilder AddDbContextConfig(this WebApplicationBuilder builder)
    {
        var host = Environment.GetEnvironmentVariable("PGHOST");
        var port = Environment.GetEnvironmentVariable("PGPORT");
        var user = Environment.GetEnvironmentVariable("PGUSER");
        var password = Environment.GetEnvironmentVariable("PGPASSWORD");
        var database = Environment.GetEnvironmentVariable("PGDATABASE");

        var connectionString = $"Host={host};Port={port};Username={user};Password={password};Database={database};";

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        return builder;
    }
}
