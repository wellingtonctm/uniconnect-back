namespace UniConnect.API.Configurations;

public static class CorsConfig
{
    public static WebApplicationBuilder AddCorsConfig(this WebApplicationBuilder builder)
    {
        var allowedOrigins = builder.Configuration.GetValue<string>("CorsAllowedOrigins");

        if (string.IsNullOrWhiteSpace(allowedOrigins))
            allowedOrigins = "*";

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                if (allowedOrigins == "*")
                    policy.AllowAnyOrigin();
                else
                    policy.WithOrigins(allowedOrigins.Split(','));

                policy.AllowAnyMethod().AllowAnyHeader();
            });
        });

        Console.WriteLine($"CorsAllowedOrigins: {allowedOrigins}");
        return builder;
    }
}
