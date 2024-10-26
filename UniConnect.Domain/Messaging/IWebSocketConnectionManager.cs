using System.Net.WebSockets;

namespace UniConnect.Domain.Messaging;

public interface IWebSocketConnectionManager
{
    void AddSocket(string id, WebSocket socket);
    void RemoveSocket(string id);
    WebSocket? GetSocketById(string id);
    Task SendMessageToAll(object message);
    IEnumerable<WebSocket> GetAllSockets();
}