using System.Net.WebSockets;

namespace ShadowGroveGames.SimpleWebSocketServer.Scripts.WebSocketClient
{
    public class WebSocketClientMessage
    {
        /// <summary>
        /// Message type.
        /// </summary>
        public WebSocketMessageType Type;

        /// <summary>
        /// Data.
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// Get the data as a string.
        /// </summary>
        public string Text => System.Text.Encoding.UTF8.GetString(Data);

        /// <summary>
        /// Length of the data.
        /// </summary>
        public int Length => Data.Length;

        public WebSocketClientMessage(WebSocketMessageType type, byte[] data)
        {
            Type = type;
            Data = data;
        }
    }
}
