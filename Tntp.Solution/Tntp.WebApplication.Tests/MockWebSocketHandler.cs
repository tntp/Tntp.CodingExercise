using Microsoft.Web.WebSockets;

namespace Tntp.WebApplication.Tests
{
    public class MockWebSocketHandler : WebSocketHandler
    {
        public override void OnOpen() { }
        public override void OnMessage(string message) { }
        public override void OnClose() { }
    }
}