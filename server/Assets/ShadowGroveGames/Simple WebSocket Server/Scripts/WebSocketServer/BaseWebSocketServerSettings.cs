using System.Collections.Generic;
using System.Net;

namespace ShadowGroveGames.SimpleWebSocketServer.Scripts.WebSocketServer
{
    /// <summary>
    /// Provides settings and default values for the BaseWebsocketServer Class.
    /// </summary>
    public class BaseWebSocketServerSettings
    {
        /// <summary>
        /// List with url paths where the websocket server needs to listen to. Default = "";
        /// </summary>
        public List<string> BaseUrls { get; set; } = new List<string>() { "/" };

        /// <summary>
        /// Port where the server needs to listen to. Default = Loopback;
        /// </summary>
        public IPAddress IPAddress { get; set; } = IPAddress.Loopback;

        /// <summary>
        /// Port where the server needs to listen to. Default = 80;
        /// </summary>
        public int Port { get; set; } = 80;

        /// <summary>
        /// Buffer size for receiving messages. Default = 4096; (bytes)
        /// </summary>
        public int BufferSize { get; set; } = 4096;
    }
}
