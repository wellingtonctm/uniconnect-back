namespace UniConnect.API.Configurations;

public static class CorsConfig
{
    public static WebApplicationBuilder AddCorsConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Development", builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

            options.AddPolicy("Production", builder =>
                builder
                    .WithOrigins("https://localhost:9000")
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        return builder;
    }
}
