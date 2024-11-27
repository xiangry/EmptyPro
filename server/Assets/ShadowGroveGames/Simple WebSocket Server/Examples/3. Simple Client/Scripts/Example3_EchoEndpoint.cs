using ShadowGroveGames.SimpleWebSocketServer.Scripts.Endpoint;
using ShadowGroveGames.SimpleWebSocketServer.Scripts.WebSocketServer;
using UnityEngine;
using UnityEngine.UI;

namespace ShadowGroveGames.SimpleWebSocketServer.Examples
{
    public class Example3_EchoEndpoint : WebSocketEndpointBehaviour
    {
        [Header("UI")]
        [SerializeField]
        private ScrollRect _scrollRect;

        [SerializeField]
        private Transform _content;

        [Header("Prefab")]
        [SerializeField]
        private GameObject _logEntry;

        public override void OnServerReady()
        {
            AddLog($"Server started at ws://{Server.Settings.IPAddress}:{Server.Settings.Port}/{EndpointPath}");
        }

        public override void OnServerShutdown()
        {
            AddLog($"Server shutdown!");
        }

        public override void OnConnect(WebSocketEventArg argument)
        {
            string shortUserId = argument.clientId.Substring(0, 7);

            AddLog($"Client connected {shortUserId}");
            Server.SendPingMessage();
        }

        public override void OnDisconnect(WebSocketEventArg argument)
        {
            string shortUserId = argument.clientId.Substring(0, 7);

            AddLog($"Client disconnected {shortUserId}");
        }

        public override void OnTextData(string text, WebSocketEventArg argument)
        {
            string shortUserId = argument.clientId.Substring(0, 7);

            AddLog($"{shortUserId}: {text}");
            Server.SendTextMessage(text, argument.clientId);
        }

        private void AddLog(string log)
        {
            if (_content == null)
                return;

            var textObject = Instantiate(_logEntry, _content);
            var text = textObject.GetComponent<Text>();
            text.text = $"{log}\n";

            // Scroll down
            if (_scrollRect != null)
                _scrollRect.normalizedPosition = new Vector2(0, -0.5f);
        }
    }
}
