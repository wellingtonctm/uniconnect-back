using UniConnect.API.Configurations;
using UniConnect.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiConfig()
       .AddCorsConfig()
       .AddDbContextConfig()
       .AddRepositoriesConfig()
       .AddServicesConfig()
       .AddSwaggerConfig()
       .AddSerilogConfig()
;

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.UseCors("Development");
}
else
{
    app.UseCors("Production");
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseWebSockets(new WebSocketOptions {
    KeepAliveInterval = TimeSpan.FromMinutes(2)
});

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();