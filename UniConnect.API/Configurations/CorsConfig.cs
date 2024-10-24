namespace UniConnect.API.Configurations;

public static class CorsConfig
{
    public static WebApplicationBuilder AddCorsConfig(this WebApplicationBuilder builder)
    {
        var allowedOrigins = builder.Configuration.GetValue<string>("CorsAllowedOrigins");

        Console.WriteLine($"CorsAllowedOrigins: {allowedOrigins}");

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                if (string.IsNullOrWhiteSpace(allowedOrigins))
                    policy.AllowAnyOrigin();
                else
                    policy.WithOrigins(allowedOrigins.Split(','));

                policy.AllowAnyMethod().AllowAnyHeader();
            });
        });

        return builder;
    }
}
