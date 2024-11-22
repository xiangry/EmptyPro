using ShadowGroveGames.SimpleWebSocketServer.Scripts.Struct;
using ShadowGroveGames.SimpleWebSocketServer.Scripts.WebSocketServer;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ShadowGroveGames.SimpleWebSocketServer.Scripts
{
    public class SimpleEventWebSocketServerScript : AbstractWebSocketServerScript
    {
        /// <summary>
        /// Auto connect to the websocket server on startup
        /// </summary>
        [Space(10)]
        [SerializeField]
        [Tooltip("Auto connect to the websocket server on startup")]
        private bool _autoStartServer = true;

        [Header("Routing Options")]
        [SerializeField]
        private bool _caseSensitiveRouting = true;

        [Header("Events")]
        [SerializeField]
        private List<SimpleEventWebSocketServerEndpoint> _endpointHandlers;

        protected virtual void Start()
        {
            if (!_autoStartServer)
                return;

            StartServer();
        }

        /// <summary>
        /// Start the server and listen for incoming connections.
        /// </summary>
        public void StartServer()
        {
            if (_webSocketServer.IsRunning())
                return;

            foreach (SimpleEventWebSocketServerEndpoint endpointHandler in _endpointHandlers)
            {
                endpointHandler.Handler.SetServer(_webSocketServer, endpointHandler.EndpointPath);
                endpointHandler.Handler.OnServerReady();
            }

            Task.Run(async () => await _webSocketServer.StartServerAsync()).ContinueWith(server =>
            {
                if (server.IsFaulted)
                {
                    Debug.LogException(server.Exception, gameObject);
                    gameObject.SetActive(false);
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// Stop the server and close all the connections.
        /// </summary>
        /// <param name="reason"></param>
        public void StopServer()
        {
            foreach (SimpleEventWebSocketServerEndpoint endpointHandler in _endpointHandlers)
            {
                endpointHandler.Handler.OnServerShutdown();
                endpointHandler.Handler.SetServer(null, null);
            }

            _webSocketServer.StopAll("Server shutting down");
            _webSocketServer.StopServer();
        }

        /// <summary>
        /// On disable, stop all the servers and clear the events.
        /// </summary>
        protected override void OnDisable()
        {
            StopServer();
            base.OnDisable();
        }

        /// <summary>
        /// In order to process the requests that have been accepted in the Async mode, we need to process them here one by one
        /// </summary>
        protected override void Update()
        {
            if (_events == null || _events.Count == 0)
                return;

            if (!_events.TryDequeue(out BaseWebsocketServerEvent serverEvent))
                return;

            foreach (SimpleEventWebSocketServerEndpoint endpointHandler in _endpointHandlers)
            {
                var incomingRoute = serverEvent.Argument.clientBaseUrl.Trim('/');
                var endpointRoute = endpointHandler.EndpointPath.Trim('/');

                if (!_caseSensitiveRouting)
                {
                    incomingRoute = incomingRoute.ToLower();
                    endpointRoute = endpointRoute.ToLower();
                }

                if (incomingRoute != endpointRoute)
                    continue;

                // On Connect
                if (serverEvent.Argument.isOpen)
                {
                    endpointHandler.Handler.OnConnect(serverEvent.Argument);
                    continue;
                }

                // On Disconnect
                if (serverEvent.Argument.isClosed)
                {
                    endpointHandler.Handler.OnDisconnect(serverEvent.Argument);
                    continue;
                }

                // On Ping
                if (serverEvent.Argument.isPing)
                {
                    endpointHandler.Handler.OnPing(serverEvent.Argument);
                    continue;
                }

                // On Pong
                if (serverEvent.Argument.isPong)
                {
                    endpointHandler.Handler.OnPong(serverEvent.Argument);
                    continue;
                }

                // On Text Data
                if (serverEvent.Argument.data != null && serverEvent.Argument.isText)
                {
                    string text = Encoding.UTF8.GetString(serverEvent.Argument.data);
                    endpointHandler.Handler.OnTextData(text, serverEvent.Argument);
                    endpointHandler.Handler.OnData(serverEvent.Argument.data, serverEvent.Argument);
                    continue;
                }

                // On Binary Data
                if (serverEvent.Argument.data != null)
                {
                    endpointHandler.Handler.OnData(serverEvent.Argument.data, serverEvent.Argument);
                    continue;
                }

                Debug.LogError("Recived unknown websocket message!");
            }
        }
    }
}