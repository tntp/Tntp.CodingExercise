using Microsoft.Web.WebSockets;
using System.Web;

namespace Tntp.WebApplication.Controllers
{
    /// <summary>
    /// The CommentsWebSocketHub class is a simple implementation of IWebSocketHub that wraps a WebSocketCollection.
    /// </summary>
    public class CommentsWebSocketHub : IWebSocketHub
    {
        private readonly WebSocketCollection _webSockets = new WebSocketCollection();

        public void AddHandler(WebSocketHandler handler)
        {
            _webSockets.Add(handler);
        }

        public void Broadcast(string message)
        {
            _webSockets.Broadcast(message);
        }

        public void CreateHandler()
        {
            HttpContext.Current.AcceptWebSocketRequest(new CommentsWebSocketHandler(this));
        }

        public void RemoveHandler(WebSocketHandler handler)
        {
            _webSockets.Remove(handler);
        }
    }
}