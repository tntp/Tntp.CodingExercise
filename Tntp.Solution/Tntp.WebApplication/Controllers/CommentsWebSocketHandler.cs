using Microsoft.Web.WebSockets;

namespace Tntp.WebApplication.Controllers
{
    /// <summary>
    /// The CommentsWebSocketHandler class is a subclass of WebSocketHandler used to broadcast new comments to connected users.
    /// </summary>
    public class CommentsWebSocketHandler : WebSocketHandler
    {
        private readonly IWebSocketHub _hub;

        public CommentsWebSocketHandler(IWebSocketHub hub)
        {
            _hub = hub;
        }

        public override void OnOpen()
        {
            _hub.AddHandler(this);
        }

        public override void OnClose()
        {
            _hub.RemoveHandler(this);
        }

        public override void OnMessage(string message)
        {
            _hub.Broadcast(message);
        }
    }
}