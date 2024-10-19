using Serilog;

namespace UniConnect.API.Configurations;

public static class SerilogConfig
{
    public static WebApplicationBuilder AddSerilogConfig(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, lc) => lc
            .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
            .WriteTo.File("Logs/log.txt",
                Serilog.Events.LogEventLevel.Information,
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true,
                retainedFileCountLimit: 5,
                fileSizeLimitBytes: 5000000));

        return builder;
    }
}
