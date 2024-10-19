using System.Net;

namespace UniConnect.API.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, Serilog.ILogger logger)
{
    private readonly RequestDelegate _next = next;
    private readonly Serilog.ILogger _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var result = new
        {
            context.Response.StatusCode,
            Message = "Internal Server Error. Please try again later.",
            Detailed = ex.Message
        };

        return context.Response.WriteAsJsonAsync(result);
    }
}
