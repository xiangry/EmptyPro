using ShadowGroveGames.SimpleWebSocketServer.Scripts.Struct;
using ShadowGroveGames.SimpleWebSocketServer.Scripts.WebSocketClient;
using UnityEngine;

namespace ShadowGroveGames.SimpleWebSocketServer.Scripts
{
    public abstract class AbstractWebSocketClientScript : MonoBehaviour
    {
        [field: Header("General Settings")]
        [field: SerializeField]
        public WebsocketClientAddress WebsocketClientAddress { get; protected set; }

        /// <summary>
        /// Base websocket client
        /// </summary>
        protected BaseWebSocketClient _webSockeClient;

        /// <summary>
        /// Create the websocket client and subscribe to the events
        /// </summary>
        protected virtual void OnEnable()
        {
            _webSockeClient = new BaseWebSocketClient(WebsocketClientAddress.Uri);
            _webSockeClient.OnConnectEvent += OnConnectEvent;
            _webSockeClient.OnMessageEvent += OnMessageEvent;
            _webSockeClient.OnTextMessageEvent += OnTextMessageEvent;
            _webSockeClient.OnBinaryMessageEvent += OnBinaryMessageEvent;
            _webSockeClient.OnErrorEvent += OnErrorEvent;
            _webSockeClient.OnCloseEvent += OnCloseEvent;
        }

        /// <summary>
        /// Connect to the websocket server
        /// </summary>
        public virtual void Connect()
        {
            _webSockeClient.Connect();
        }

        /// <summary>
        /// On open connection event
        /// </summary>
        protected abstract void OnConnectEvent();

        /// <summary>
        /// On received message event
        /// </summary>
        /// <param name="message"></param>
        protected abstract void OnMessageEvent(WebSocketClientMessage message);

        /// <summary>
        /// On received text message event
        /// </summary>
        /// <param name="text"></param>
        protected abstract void OnTextMessageEvent(string text);

        /// <summary>
        /// On received binary message event
        /// </summary>
        /// <param name="data"></param>
        protected abstract void OnBinaryMessageEvent(byte[] data);

        /// <summary>
        /// On error event
        /// </summary>
        /// <param name="errorMsg"></param>
        protected abstract void OnErrorEvent(string errorMsg);

        /// <summary>
        /// On close connection event
        /// </summary>
        /// <param name="closeCode"></param>
        protected abstract void OnCloseEvent(WebSocketClientCloseCode closeCode);

        /// <summary>
        /// Need to process the message queue
        /// </summary>
        protected virtual void Update()
        {
            _webSockeClient?.ProcessMessageQueue();
        }

        /// <summary>
        /// Close the connection on disable
        /// </summary>
        protected virtual void OnDisable()
        {
            _webSockeClient?.Close();
        }

        /// <summary>
        /// Get the websocket client
        /// </summary>
        /// <returns></returns>
        public BaseWebSocketClient GetWebSocketClient()
        {
            return _webSockeClient;
        }
    }
}