using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using UniConnect.Domain.Messaging;

namespace UniConnect.Infrastructure.Messaging;

public class WebSocketConnectionManager: IWebSocketConnectionManager
{
    private readonly ConcurrentDictionary<string, WebSocket> _sockets = new();

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

    public async Task SendMessageToAll(string message)
    {
        var messageBytes = Encoding.UTF8.GetBytes(message);
        var tasks = _sockets.Values.Select(socket => socket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None));
        await Task.WhenAll(tasks);
    }

    public IEnumerable<WebSocket> GetAllSockets() => _sockets.Values;
}
