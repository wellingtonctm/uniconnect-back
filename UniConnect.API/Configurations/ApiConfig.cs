using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace UniConnect.API.Configurations;

public static class ApiConfig
{
    public static WebApplicationBuilder AddApiConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

        builder.Services.AddHttpClient("UniConnectClient")
                        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                        { ServerCertificateCustomValidationCallback = ValidateServerCertification });

        return builder;
    }

    private static bool ValidateServerCertification(HttpRequestMessage requestMessage, X509Certificate2? certificate, X509Chain? chain, SslPolicyErrors sslErrors)
    {
        return true;
    }
}