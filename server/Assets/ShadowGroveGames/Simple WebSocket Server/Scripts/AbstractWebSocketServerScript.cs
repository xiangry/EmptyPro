using ShadowGroveGames.SimpleWebSocketServer.Scripts.WebSocketServer;
using ShadowGroveGames.SimpleWebSocketServer.Scripts.Struct;
using System.Collections.Concurrent;
using UnityEngine;

namespace ShadowGroveGames.SimpleWebSocketServer.Scripts
{
    public abstract class AbstractWebSocketServerScript : MonoBehaviour
    {
        [field: Header("General Settings")]
        [field: SerializeField]
        public WebsocketServerListeningAddress ListeningAddress { get; protected set; }

        protected BaseWebsocketServer _webSocketServer;

        protected ConcurrentQueue<BaseWebsocketServerEvent> _events = new ConcurrentQueue<BaseWebsocketServerEvent>();

        /// <summary>
        /// On enable, create the server and set the events.
        /// </summary>
        protected virtual void OnEnable()
        {
            _webSocketServer = new BaseWebsocketServer(new BaseWebSocketServerSettings()
            {
                BufferSize = 65535,
                Port = ListeningAddress.Port,
                IPAddress = ListeningAddress.HostIPAddress,
            });

            _webSocketServer.WebsocketServerEvent += OnWebsocketServerEvent;
        }

        /// <summary>
        /// On disable, stop all the servers and clear the events.
        /// </summary>
        protected virtual void OnDisable()
        {
            _webSocketServer.StopAll();
            _events.Clear();
        }

        /// <summary>
        /// On destroy, call the on disable method.
        /// </summary>
        protected virtual void OnDestroy()
        {
            OnDisable();
        }

        /// <summary>
        /// Process the events that have been enqueued.
        /// </summary>
        protected abstract void Update();

        /// <summary>
        /// Enqueue the event to the event queue.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="argument"></param>
        private void OnWebsocketServerEvent(object server, WebSocketEventArg argument)
        {
            _events.Enqueue(new BaseWebsocketServerEvent((BaseWebsocketServer)server, argument));
        }

        /// <summary>
        /// Get the server instance.
        /// </summary>
        /// <returns></returns>
        public BaseWebsocketServer GetServer()
        {
            return _webSocketServer;
        }
    }
}