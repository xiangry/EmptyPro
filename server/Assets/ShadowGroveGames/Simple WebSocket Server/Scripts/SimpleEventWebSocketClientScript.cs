using ShadowGroveGames.SimpleWebSocketServer.Scripts.WebSocketClient;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace ShadowGroveGames.SimpleWebSocketServer.Scripts
{
    public class SimpleEventWebSocketClientScript : AbstractWebSocketClientScript
    {
        /// <summary>
        /// Auto connect to the websocket server on startup
        /// </summary>
        [Space(10)]
        [SerializeField]
        [Tooltip("Auto connect to the websocket server on startup")]
        private bool _autoConnectOnStartup = true;

        [Header("Events")]
        [Tooltip("Event that is called when the websocket client is opened")]
        public UnityEventOnConnectEvent OnConnect = new UnityEventOnConnectEvent();

        [Tooltip("Event that is called when the websocket client receives a message")]
        public UnityEventOnMessageEvent OnMessage = new UnityEventOnMessageEvent();

        [Tooltip("Event that is called when the websocket client receives a text message")]
        public UnityEventOnTextMessageEvent OnTextMessage = new UnityEventOnTextMessageEvent();

        [Tooltip("Event that is called when the websocket client receives a binary message")]
        public UnityEventOnBinaryMessageEvent OnBinaryMessage = new UnityEventOnBinaryMessageEvent();

        [Tooltip("Event that is called when the websocket client encounters an error")]
        public UnityEventOnErrorEvent OnError = new UnityEventOnErrorEvent();

        [Tooltip("Event that is called when the websocket client is closed")]
        public UnityEventOnCloseEvent OnClose = new UnityEventOnCloseEvent();

        void Start()
        {
            if (_autoConnectOnStartup)
                Connect();
        }

        /// <summary>
        /// On open connection event
        /// </summary>
        protected override void OnConnectEvent()
        {
            OnConnect?.Invoke(_webSockeClient);
        }

        /// <summary>
        /// On received message event
        /// </summary>
        /// <param name="message"></param>
        protected override void OnMessageEvent(WebSocketClientMessage message)
        {
            OnMessage?.Invoke(message);
        }

        /// <summary>
        /// On received text message event
        /// </summary>
        /// <param name="text"></param>
        protected override void OnTextMessageEvent(string text)
        {
            OnTextMessage?.Invoke(text);
        }

        /// <summary>
        /// On received binary message event
        /// </summary>
        /// <param name="data"></param>
        protected override void OnBinaryMessageEvent(byte[] data)
        {
            OnBinaryMessage?.Invoke(data);
        }

        /// <summary>
        /// On error event
        /// </summary>
        /// <param name="errorMsg"></param>
        protected override void OnErrorEvent(string errorMsg)
        {
            OnError?.Invoke(errorMsg);
        }

        /// <summary>
        /// Need to process the message queue
        /// </summary>
        protected override void OnCloseEvent(WebSocketClientCloseCode closeCode)
        {
            OnClose?.Invoke(closeCode);
        }

        [Serializable]
        public class UnityEventOnConnectEvent : UnityEvent<BaseWebSocketClient>
        {
        }

        [Serializable]
        public class UnityEventOnMessageEvent : UnityEvent<WebSocketClientMessage>
        {
        }

        [Serializable]
        public class UnityEventOnTextMessageEvent : UnityEvent<string>
        {
        }

        [Serializable]
        public class UnityEventOnBinaryMessageEvent : UnityEvent<byte[]>
        {
        }

        [Serializable]
        public class UnityEventOnErrorEvent : UnityEvent<string>
        {
        }

        [Serializable]
        public class UnityEventOnCloseEvent : UnityEvent<WebSocketClientCloseCode>
        {
        }
    }
}
