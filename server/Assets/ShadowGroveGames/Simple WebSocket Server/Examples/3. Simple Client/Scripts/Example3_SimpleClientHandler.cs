using ShadowGroveGames.SimpleWebSocketServer.Scripts;
using ShadowGroveGames.SimpleWebSocketServer.Scripts.WebSocketClient;
using UnityEngine;
using UnityEngine.UI;

namespace ShadowGroveGames.SimpleWebSocketServer.Examples
{
    public class Example3_SimpleClientHandler : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField]
        private ScrollRect _scrollRect;

        [SerializeField]
        private Transform _content;

        [SerializeField]
        private InputField _messageInput;

        [SerializeField]
        private GameObject _sendButton;

        [SerializeField]
        private GameObject _connectButton;

        [Header("Reference")]
        [SerializeField]
        private SimpleEventWebSocketClientScript _simpleEventWebSocketClientScript;

        [Header("Prefab")]
        [SerializeField]
        private GameObject _logEntry;

        private BaseWebSocketClient _webSocketClient;

        private void Start()
        {
            _webSocketClient = _simpleEventWebSocketClientScript.GetWebSocketClient();
        }

        public void OnConnect(BaseWebSocketClient webSocketClient)
        {
            _connectButton.SetActive(false);
            _messageInput.gameObject.SetActive(true);
            _sendButton.SetActive(true);

            AddLog("Connected");
            _webSocketClient = webSocketClient;
        }

        public void OnDisconnect()
        {
            _connectButton.SetActive(true);
            _messageInput.gameObject.SetActive(false);
            _sendButton.SetActive(false);

            AddLog("Disconnected");
        }

        public void OnTextMessage(string text)
        {
            AddLog($"Server: {text}");
        }

        public void Connect()
        {
            _simpleEventWebSocketClientScript.Connect();
        }

        public void SendMessage()
        {
            string message = _messageInput.text;
            _messageInput.text = "";

            _webSocketClient?.SendTextMessage(message);
            AddLog($"Me: {message}");
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
