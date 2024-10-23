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
                    .WithOrigins("https://uniconnect-front.up.railway.app")
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        return builder;
    }
}
