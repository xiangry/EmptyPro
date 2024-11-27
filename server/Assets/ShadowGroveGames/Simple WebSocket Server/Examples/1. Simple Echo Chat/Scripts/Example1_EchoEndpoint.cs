using ShadowGroveGames.SimpleWebSocketServer.Scripts.WebSocketServer;
using ShadowGroveGames.SimpleWebSocketServer.Scripts.Endpoint;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ShadowGroveGames.SimpleWebSocketServer.Examples
{
    public class Example1_EchoEndpoint : WebSocketEndpointBehaviour
    {
        private const string CLIENT_ASSET_GUID = "a484a638f643aaf40ac9115b5b55a5b8";

        [Header("UI")]
        [SerializeField]
        private ScrollRect _scrollRect;

        [SerializeField]
        private Transform _content;

        [SerializeField]
        private GameObject _clientButton;

        [Header("Prefab")]
        [SerializeField]
        private GameObject _logEntry;

        public override void OnServerReady()
        {
            // The "Open Client" function only works in the editor!
            // But you can copy the file Client.html to your desktop and open it there with a double click.
            if (Application.isEditor)
                _clientButton.gameObject.SetActive(true);

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
            _clientButton.gameObject.SetActive(false);
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

        public void OpenClient()
        {
#if UNITY_EDITOR
            string relativePath = AssetDatabase.GUIDToAssetPath(CLIENT_ASSET_GUID);
            if (string.IsNullOrEmpty(relativePath))
                return;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
            if (!File.Exists(filePath))
                return;

            Application.OpenURL(filePath);
#else
            Debug.LogWarning("This function only works in the editor!");
#endif
        }
    }
}
