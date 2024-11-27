using System;
using System.Text;
using System.Threading.Tasks;

namespace ShadowGroveGames.SimpleWebSocketServer.Scripts.WebSocketServer
{
    /// <summary>
    /// Middleware class for interfacing with the WebSocketBehaviour and WebSocketHttpServer class.
    /// </summary>
    public class BaseWebsocketServer
    {
        /// <summary>
        /// Interface for the WebSocketBehaviour class.
        /// </summary>
        private readonly IWebSocketHandler _WebsocketHandler;
        /// <summary>
        /// Interface for the WebSocketHttpServer class.
        /// </summary>
        private readonly IWebSocketHttpServer _WebSocketHttpServer;

        public readonly BaseWebSocketServerSettings Settings;

        /// <summary>
        /// Eventhandler for when _events such as receiving messages and errors from the websocket server happen.
        /// </summary>
        public event EventHandler<WebSocketEventArg> WebsocketServerEvent;

        /// <summary>
        /// Constructor for setting up the Library.
        /// </summary>
        /// <param name="settings">Settings with default values.</param>
        public BaseWebsocketServer(BaseWebSocketServerSettings settings)
        {
            Settings = settings;
            _WebsocketHandler = new WebSocketHandler(settings.BufferSize);
            _WebsocketHandler.WebsocketEvent += OnWebsocketServerEvent;
            _WebSocketHttpServer = new WebSocketHttpServer(_WebsocketHandler, settings);
        }

        /// <summary>
        /// Overload constructor for setting up library with default values.
        /// </summary>
        public BaseWebsocketServer() : this(new BaseWebSocketServerSettings()) { }

        /// <summary>
        /// Event handler for receiving websocket messages from the websocket handler.
        /// </summary>
        /// <param name="sender">Instance of firing class.</param>
        /// <param name="arg">Arguments for event.</param>
        private void OnWebsocketServerEvent(object sender, WebSocketEventArg arg)
        {
            WebsocketServerEvent?.Invoke(this, arg);
        }

        /// <summary>
        /// Starts running the server async.
        /// </summary>
        public async Task StartServerAsync()
        {
            if (_WebSocketHttpServer.IsRunning())
                return;

            await _WebSocketHttpServer.RunServer();
        }

        /// <summary>
        /// Starts running the server async.
        /// </summary>
        public void StartServer()
        {
            Task.Run(async () => await _WebSocketHttpServer.RunServer());
        }

        /// <summary>
        /// Stops the server and closes all connections async.
        /// </summary>
        public void StopServer()
        {
            _WebSocketHttpServer.Stop();
        }

        /// <summary>
        /// Overload method for sending text messages to a specific client, using it's id, asynchronous.
        /// </summary>
        /// <param name="messageToSend"></param>
        /// <param name="clientId"></param>
        /// <returns>True when send succesfully.</returns>
        public async Task<bool> SendTextMessageAsync(string messageToSend, string clientId = null)
        {
            WebSocketMessageContainer message = new WebSocketMessageContainer()
            {
                data = Encoding.UTF8.GetBytes(messageToSend),
                isText = true
            };

            if (clientId != null)
            {
                return await _WebsocketHandler.SendMessage(message, clientId);
            }

            return await _WebsocketHandler.SendMessage(message);
        }

        /// <summary>
        /// Overload method for sending text messages to a specific client, using it's id, synchronous.
        /// </summary>
        /// <param name="messageToSend"></param>
        /// <param name="clientId"></param>
        /// <returns>True when send succesfully.</returns>
        public bool SendTextMessage(string messageToSend, string clientId = null)
        {
            WebSocketMessageContainer message = new WebSocketMessageContainer()
            {
                data = Encoding.UTF8.GetBytes(messageToSend),
                isText = true
            };

            if (clientId != null)
            {
                return Task.Run(async () => await _WebsocketHandler.SendMessage(message, clientId)).GetAwaiter().GetResult();
            }

            return Task.Run(async () => await _WebsocketHandler.SendMessage(message)).GetAwaiter().GetResult();
        }


        /// <summary>
        /// Sends a binary "message" to a specific client asynchronous.
        /// </summary>
        /// <param name="messageToSend">Message in binary format (byte array).</param>
        /// <param name="clientId">Client to send to.</param>
        /// <returns>True when succesfully send.</returns>
        public async Task<bool> SendBinaryMessageAsync(byte[] messageToSend, string clientId = null)
        {
            WebSocketMessageContainer message = new WebSocketMessageContainer()
            {
                data = messageToSend,
                isBinary = true
            };

            if (clientId != null)
            {
                return await _WebsocketHandler.SendMessage(message, clientId);
            }

            return await _WebsocketHandler.SendMessage(message);
        }

        /// <summary>
        /// Sends a binary "message" to a specific client synchronous.
        /// </summary>
        /// <param name="messageToSend">Message in binary format (byte array).</param>
        /// <param name="clientId">Client to send to.</param>
        /// <returns>True when succesfully send.</returns>
        public bool SendBinaryMessage(byte[] messageToSend, string clientId = null)
        {
            WebSocketMessageContainer message = new WebSocketMessageContainer()
            {
                data = messageToSend,
                isBinary = true
            };

            if (clientId != null)
            {
                return Task.Run(async () => await _WebsocketHandler.SendMessage(message, clientId)).GetAwaiter().GetResult();
            }

            return Task.Run(async () => await _WebsocketHandler.SendMessage(message)).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Sends a ping message to all clients asynchronous, can have message.
        /// </summary>
        /// <param name="messageToSend">Possible extra message to send along ping.</param>
        /// <param name="clientId">Set clientId in case you want to send to a specific client.</param>
        /// <returns>True when succesfully send.</returns>
        public async Task<bool> SendPingMessageAsync(string messageToSend = "", string clientId = null)
        {
            WebSocketMessageContainer message = new WebSocketMessageContainer()
            {
                data = Encoding.UTF8.GetBytes(messageToSend),
                isPing = true
            };

            if (clientId != null)
            {
                return await _WebsocketHandler.SendMessage(message, clientId);
            }
            return await _WebsocketHandler.SendMessage(message);
        }

        /// <summary>
        /// Sends a ping message to all clients synchronous, can have message.
        /// </summary>
        /// <param name="messageToSend">Possible extra message to send along ping.</param>
        /// <param name="clientId">Set clientId in case you want to send to a specific client.</param>
        /// <returns>True when succesfully send.</returns>
        public bool SendPingMessage(string messageToSend = "", string clientId = null)
        {
            WebSocketMessageContainer message = new WebSocketMessageContainer()
            {
                data = Encoding.UTF8.GetBytes(messageToSend),
                isPing = true
            };

            if (clientId != null)
            {
                return Task.Run(async () => await _WebsocketHandler.SendMessage(message, clientId)).GetAwaiter().GetResult();
            }
            return Task.Run(async () => await _WebsocketHandler.SendMessage(message)).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send pong message to all available clients asynchronous, can have message.
        /// </summary>
        /// <param name="messageToSend">Possible extra message to send along ping.</param>
        /// <param name="clientId">Set clientId in case you want to send to a specific client.</param>
        /// <returns>True when succesfully send.</returns>
        public async Task<bool> SendPongMessageAsync(string messageToSend = "", string clientId = null)
        {
            WebSocketMessageContainer message = new WebSocketMessageContainer()
            {
                data = Encoding.UTF8.GetBytes(messageToSend),
                isPong = true
            };

            if (clientId != null)
            {
                return await _WebsocketHandler.SendMessage(message, clientId);
            }
            return await _WebsocketHandler.SendMessage(message);
        }

        /// <summary>
        /// Send pong message to all available clients synchronous, can have message.
        /// </summary>
        /// <param name="messageToSend">Possible extra message to send along ping.</param>
        /// <param name="clientId">Set clientId in case you want to send to a specific client.</param>
        /// <returns>True when succesfully send.</returns>
        public bool SendPongMessage(string messageToSend = "", string clientId = null)
        {
            WebSocketMessageContainer message = new WebSocketMessageContainer()
            {
                data = Encoding.UTF8.GetBytes(messageToSend),
                isPong = true
            };

            if (clientId != null)
            {
                return Task.Run(async () => await _WebsocketHandler.SendMessage(message, clientId)).GetAwaiter().GetResult();
            }
            return Task.Run(async () => await _WebsocketHandler.SendMessage(message)).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Stops a connection with a specific client asynchronous. Can provide reason.
        /// </summary>
        /// <param name="clientId">Specific client to close connection with.</param>
        /// <param name="reason">Possible reason to close the connection.</param>
        /// <returns>True when succesfully closed.</returns>
        public async Task<bool> StopClientAsync(string clientId, string reason = "")
        {
            if (reason.Length > 0)
            {
                return await _WebsocketHandler.StopClient(clientId, reason);
            }
            else
            {
                return await _WebsocketHandler.StopClient(clientId);
            }
        }

        /// <summary>
        /// Stops a connection with a specific client synchronous. Can provide reason.
        /// </summary>
        /// <param name="clientId">Specific client to close connection with.</param>
        /// <param name="reason">Possible reason to close the connection.</param>
        /// <returns>True when succesfully closed.</returns>
        public bool StopClient(string clientId, string reason = "")
        {
            if (reason.Length > 0)
            {
                return Task.Run(async () => await _WebsocketHandler.StopClient(clientId, reason)).GetAwaiter().GetResult();
            }
            else
            {
                return Task.Run(async () => await _WebsocketHandler.StopClient(clientId)).GetAwaiter().GetResult();
            }
        }


        /// <summary>
        /// Stops all connections with all clients asynchronous. Can provide reason.
        /// </summary>
        /// <param name="reason">Possible reason to close the connection.</param>
        /// <returns>True when succesfully closed.</returns>
        public async Task<bool> StopAllAsync(string reason = "")
        {
            if (reason.Length > 0)
            {
                return await _WebsocketHandler.StopAll(reason);
            }
            else
            {
                return await _WebsocketHandler.StopAll();
            }
        }

        /// <summary>
        /// Stops all connections with all clients synchronous. Can provide reason.
        /// </summary>
        /// <param name="reason">Possible reason to close the connection.</param>
        /// <returns>True when succesfully closed.</returns>
        public bool StopAll(string reason = "")
        {
            if (!string.IsNullOrEmpty(reason))
                return Task.Run(async () => await _WebsocketHandler.StopAll(reason)).GetAwaiter().GetResult();

            return Task.Run(async () => await _WebsocketHandler.StopAll()).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Is server running?
        /// </summary>
        /// <returns></returns>
        public bool IsRunning()
        {
            return _WebSocketHttpServer.IsRunning();
        }
    }
}
