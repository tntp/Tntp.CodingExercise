using Microsoft.Web.WebSockets;

namespace Tntp.WebApplication.Controllers
{
    /// <summary>
    /// The IWebSocketHub interface is a light abstraction for making it easier to use the Microsoft.WebSockets API with dependency injection.
    /// </summary>
    public interface IWebSocketHub
    {
        void CreateHandler();
        void AddHandler(WebSocketHandler handler);
        void RemoveHandler(WebSocketHandler handler);
        void Broadcast(string message);
    }
}