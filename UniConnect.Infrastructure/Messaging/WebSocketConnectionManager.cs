using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using UniConnect.Domain.Messaging;

namespace UniConnect.Infrastructure.Messaging;

public class WebSocketConnectionManager : IWebSocketConnectionManager
{
    private readonly ConcurrentDictionary<string, WebSocket> _sockets = new();
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public void AddSocket(string id, WebSocket socket)
    {
        _sockets.TryAdd(id, socket);
    }

    public void RemoveSocket(string id)
    {
        _sockets.TryRemove(id, out _);
    }

    public WebSocket? GetSocketById(string id)
    {
        _sockets.TryGetValue(id, out var socket);
        return socket;
    }

    public async Task SendMessageToAll(object message)
    {
        var messageJson = JsonSerializer.Serialize(message, _jsonOptions);
        var messageBytes = Encoding.UTF8.GetBytes(messageJson);
        var tasks = _sockets.Values.Select(socket => socket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None));
        await Task.WhenAll(tasks);
    }

    public IEnumerable<WebSocket> GetAllSockets() => _sockets.Values;
}
