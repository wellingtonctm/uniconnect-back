using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using UniConnect.Domain.Messaging;

namespace UniConnect.API.Controllers;

[ApiController]
public class WebSocketController : ControllerBase
{
    private readonly Serilog.ILogger _logger;
    private readonly IWebSocketConnectionManager _connectionManager;

    public WebSocketController(Serilog.ILogger logger, IWebSocketConnectionManager connectionManager)
    {
        _logger = logger;
        _connectionManager = connectionManager;
    }

    [HttpGet("/ws")]
    public async Task Get()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var socketId = Guid.NewGuid().ToString(); // Unique ID for the socket
            _connectionManager.AddSocket(socketId, webSocket);
            await HandleWebSocketConnection(socketId, webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    private async Task HandleWebSocketConnection(string socketId, WebSocket webSocket)
    {
        try
        {
            var buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!receiveResult.CloseStatus.HasValue)
            {
                receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
            _connectionManager.RemoveSocket(socketId);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An unhandled exception occurred.");
        }
        finally
        {
            _connectionManager.RemoveSocket(socketId);
        }
    }

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}
