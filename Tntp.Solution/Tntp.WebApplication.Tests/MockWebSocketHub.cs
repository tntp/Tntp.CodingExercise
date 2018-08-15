using Microsoft.Web.WebSockets;
using Tntp.WebApplication.Controllers;

namespace Tntp.WebApplication.Tests
{
    public class MockWebSocketHub : IWebSocketHub
    {
        public void AddHandler(WebSocketHandler handler) { }

        public void Broadcast(string message) { }

        public void CreateHandler() { }

        public void RemoveHandler(WebSocketHandler handler) { }
    }
}