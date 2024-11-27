using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace ShadowGroveGames.SimpleWebSocketServer.Scripts.WebSocketClient
{
    public class BaseWebSocketClient
    {
        /// <summary>
        /// Event that is triggered when the WebSocket connection is opened
        /// </summary>
        public event WebSocketClientConnectEvent OnConnectEvent;

        /// <summary>
        /// Event that is triggered when the WebSocket connection receives a message
        /// </summary>
        public event WebSocketClientMessageEvent OnMessageEvent;

        /// <summary>
        /// Event that is triggered when the WebSocket connection receives a text message
        /// </summary>
        public event WebSocketClientTextMessageEvent OnTextMessageEvent;

        /// <summary>
        /// Event that is triggered when the WebSocket connection receives a binary message
        /// </summary>
        public event WebSocketClientBinaryMessageEvent OnBinaryMessageEvent;

        /// <summary>
        /// Event that is triggered when the WebSocket connection encounters an error
        /// </summary>
        public event WebSocketClientErrorEvent OnErrorEvent;

        /// <summary>
        /// Event that is triggered when the WebSocket connection is closed
        /// </summary>
        public event WebSocketClientCloseEvent OnCloseEvent;

        /// <summary>
        /// Connection state
        /// </summary>
        public WebSocketState State
        {
            get
            {
                switch (_socket.State)
                {
                    case System.Net.WebSockets.WebSocketState.Connecting:
                        return WebSocketState.Connecting;

                    case System.Net.WebSockets.WebSocketState.Open:
                        return WebSocketState.Open;

                    case System.Net.WebSockets.WebSocketState.CloseSent:
                    case System.Net.WebSockets.WebSocketState.CloseReceived:
                        return WebSocketState.Closing;

                    case System.Net.WebSockets.WebSocketState.Closed:
                        return WebSocketState.Closed;

                    default:
                        return WebSocketState.Closed;
                }
            }
        }

        private WebSocketState _lastState = WebSocketState.Closed;

        /// <summary>
        /// Connection uri
        /// </summary>
        private Uri _uri;

        /// <summary>
        /// Connection headers
        /// </summary>
        private Dictionary<string, string> _headers = new Dictionary<string, string>();

        /// <summary>
        /// Connection subprotocols
        /// </summary>
        private List<string> _subProtocols = new List<string>();

        /// <summary>
        /// Connection socket
        /// </summary>
        private ClientWebSocket _socket = new ClientWebSocket();

        /// <summary>
        /// Connection cancellation token source
        /// </summary>
        private CancellationTokenSource _tokenSource = null;

        /// <summary>
        /// Connection cancellation token
        /// </summary>
        private CancellationToken _cancellationToken;

        /// <summary>
        /// Send bytes queue
        /// </summary>
        private List<ArraySegment<byte>> _sendQueueBytes = new List<ArraySegment<byte>>();

        /// <summary>
        /// Send text queue
        /// </summary>
        private List<ArraySegment<byte>> _sendQueueText = new List<ArraySegment<byte>>();

        /// <summary>
        /// Sending status
        /// </summary>
        private bool _isSending = false;

        /// <summary>
        /// Stop client loop indicator
        /// </summary>
        private bool _stopClientLoop = false;

        /// <summary>
        /// Message lock for outgoing messages
        /// </summary>
        private readonly object _messageLockOutgoing = new object();

        /// <summary>
        /// Message lock for incoming messages
        /// </summary>
        private readonly object _messageLockIncoming = new object();

        /// <summary>
        /// Message list
        /// </summary>
        private List<WebSocketClientMessage> _messageList = new List<WebSocketClientMessage>();

        /// <summary>
        /// Create a new WebSocketClient with the given url and headers
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="headers"></param>
        public BaseWebSocketClient(Uri uri, Dictionary<string, string> headers = null)
        {
            _uri = uri;
            if (headers != null)
                _headers = headers;

            ValidateConnectionData();
        }

        /// <summary>
        /// Create a new WebSocketClient with the given url, subprotocol and headers
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="subProtocol"></param>
        /// <param name="headers"></param>
        public BaseWebSocketClient(Uri uri, string subProtocol, Dictionary<string, string> headers = null)
        {
            _uri = uri;
            if (headers != null)
                _headers = headers;

            _subProtocols = new List<string> { subProtocol };

            ValidateConnectionData();
        }

        /// <summary>
        /// Create a new WebSocketClient with the given url, subprotocols and headers
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="subProtocols"></param>
        /// <param name="headers"></param>
        public BaseWebSocketClient(Uri uri, List<string> subProtocols, Dictionary<string, string> headers = null)
        {
            _uri = uri;
            if (headers != null)
                this._headers = headers;

            if (subProtocols != null)
                _subProtocols = subProtocols;

            _subProtocols = subProtocols;

            ValidateConnectionData();
        }

        /// <summary>
        /// Validate connection data
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        private void ValidateConnectionData()
        {
            string protocol = _uri.Scheme;
            if (!protocol.Equals("ws") && !protocol.Equals("wss"))
                throw new ArgumentException("Unsupported protocol: " + protocol);
        }

        /// <summary>
        /// Open the WebSocket connection
        /// </summary>
        /// <returns></returns>
        public async Task ConnectAsync()
        {
            try
            {
                _tokenSource = new CancellationTokenSource();
                _cancellationToken = _tokenSource.Token;
                _socket = new ClientWebSocket();

                foreach (var header in _headers)
                {
                    _socket.Options.SetRequestHeader(header.Key, header.Value);
                }

                foreach (string subprotocol in _subProtocols)
                {
                    _socket.Options.AddSubProtocol(subprotocol);
                }

                _stopClientLoop = false;
                await _socket.ConnectAsync(_uri, _cancellationToken);
                await ReceiveAsync();
            }
            catch (Exception ex)
            {
                OnErrorEvent?.Invoke(ex.Message);
                OnCloseEvent?.Invoke(WebSocketClientCloseCode.Abnormal);
            }
            finally
            {
                if (_socket != null)
                {
                    _tokenSource.Cancel();
                    _socket.Dispose();
                }
            }
        }

        /// <summary>
        /// Open the WebSocket connection
        /// </summary>
        /// <returns></returns>
        public void Connect()
        {
            Task.Run(async () => await ConnectAsync());
        }

        /// <summary>
        /// Send a async binary message to the WebSocket server
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public async Task SendBinaryMessageAsync(byte[] bytes)
        {
            await SendMessage(_sendQueueBytes, WebSocketMessageType.Binary, new ArraySegment<byte>(bytes));
        }

        /// <summary>
        /// Send a binary message to the WebSocket server
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public void SendBinaryMessage(byte[] bytes)
        {
            Task.Run(async () => await SendBinaryMessageAsync(bytes)).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send a async text message to the WebSocket server
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendTextMessageAsync(string message)
        {
            var encoded = Encoding.UTF8.GetBytes(message);
            await SendMessage(_sendQueueText, WebSocketMessageType.Text, new ArraySegment<byte>(encoded, 0, encoded.Length));
        }

        /// <summary>
        /// Send a text message to the WebSocket server
        /// </summary>
        /// <param name="message"></param>
        public void SendTextMessage(string message)
        {
            Task.Run(async () => await SendTextMessageAsync(message)).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send a message to the WebSocket server
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="messageType"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private async Task SendMessage(List<ArraySegment<byte>> queue, WebSocketMessageType messageType, ArraySegment<byte> buffer)
        {
            // Make sure we have data.
            if (buffer.Count == 0)
            {
                return;
            }

            // The state of the connection is contained in the context Items dictionary.
            bool sending;

            lock (_messageLockOutgoing)
            {
                sending = _isSending;

                // If not, we are now.
                if (!_isSending)
                    _isSending = true;
            }

            if (!sending)
            {
                // Lock with a timeout, just in case.
                if (!Monitor.TryEnter(_socket, 1000))
                {
                    // If we couldn't obtain exclusive access to the socket in one second, something is wrong.
                    await _socket.CloseAsync(WebSocketCloseStatus.InternalServerError, string.Empty, _cancellationToken);
                    return;
                }

                try
                {
                    // Send the message synchronously.
                    _socket
                        .SendAsync(buffer, messageType, true, _cancellationToken)
                        .Wait(_cancellationToken);
                }
                finally
                {
                    Monitor.Exit(_socket);
                }

                // Note that we've finished sending.
                lock (_messageLockOutgoing)
                {
                    _isSending = false;
                }

                // Handle any queued messages.
                await HandleQueue(queue, messageType);
            }
            else
            {
                // Add the message to the queue.
                lock (_messageLockOutgoing)
                {
                    queue.Add(buffer);
                }
            }
        }

        private async Task HandleQueue(List<ArraySegment<byte>> queue, WebSocketMessageType messageType)
        {
            var buffer = new ArraySegment<byte>();
            lock (_messageLockOutgoing)
            {
                // Check for an item in the queue.
                if (queue.Count > 0)
                {
                    // Pull it off the top.
                    buffer = queue[0];
                    queue.RemoveAt(0);
                }
            }

            // Send that message.
            if (buffer.Count > 0)
                await SendMessage(queue, messageType, buffer);
        }

        // simple dispatcher for queued messages.
        public void ProcessMessageQueue()
        {
            if (_lastState != State)
            {
                _lastState = State;
                switch (State)
                {
                    case WebSocketState.Open:
                        OnConnectEvent?.Invoke();
                        break;

                    case WebSocketState.Closed:
                        OnCloseEvent?.Invoke(WebSocketClientCloseCode.Normal);
                        break;
                }
            }

            if (_messageList.Count == 0)
                return;

            List<WebSocketClientMessage> messageListCopy;

            lock (_messageLockIncoming)
            {
                messageListCopy = new List<WebSocketClientMessage>(_messageList);
                _messageList.Clear();
            }

            var len = messageListCopy.Count;
            for (int i = 0; i < len; i++)
            {
                OnMessageEvent?.Invoke(messageListCopy[i]);

                if (messageListCopy[i].Type == WebSocketMessageType.Text)
                    OnTextMessageEvent?.Invoke(messageListCopy[i].Text);

                if (messageListCopy[i].Type == WebSocketMessageType.Binary)
                    OnBinaryMessageEvent?.Invoke(messageListCopy[i].Data);
            }
        }

        /// <summary>
        /// Receive Messages
        /// </summary>
        /// <returns></returns>
        private async Task ReceiveAsync()
        {
            WebSocketClientCloseCode closeCode = WebSocketClientCloseCode.Abnormal;

            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[8192]);
            try
            {
                while (!_stopClientLoop && State == WebSocketState.Open)
                {
                    WebSocketReceiveResult result = null;

                    using (var ms = new MemoryStream())
                    {
                        do
                        {
                            result = Task.Run(async () => await _socket.ReceiveAsync(buffer, _cancellationToken)).GetAwaiter().GetResult();
                            ms.Write(buffer.Array, buffer.Offset, result.Count);
                        }
                        while (!result.EndOfMessage);

                        ms.Seek(0, SeekOrigin.Begin);

                        if (result.MessageType != WebSocketMessageType.Close)
                        {
                            lock (_messageLockIncoming)
                            {
                                _messageList.Add(new WebSocketClientMessage(result.MessageType, ms.ToArray()));
                            }
                        }
                        else if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, _cancellationToken);
                            closeCode = WebSocketClientHelpers.ParseCloseCodeEnum((int)result.CloseStatus);
                        }
                    }
                }
            }
            catch (Exception)
            {
                _tokenSource.Cancel();
            }
        }

        /// <summary>
        /// Cancel the WebSocket connection
        /// </summary>
        private void CancelConnection()
        {
            _stopClientLoop = true;
            _tokenSource?.Cancel();
        }

        /// <summary>
        /// Close the WebSocket connection
        /// </summary>
        /// <returns></returns>
        public async Task CloseAsync()
        {
            CancelConnection();
            if (State == WebSocketState.Open)
                await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, _cancellationToken);
        }

        public void Close()
        {
            Task.Run(async () => await CloseAsync()).GetAwaiter().GetResult();
        }

        public enum WebSocketState
        {
            Connecting,
            Open,
            Closing,
            Closed
        }

        public delegate void WebSocketClientConnectEvent();
        public delegate void WebSocketClientMessageEvent(WebSocketClientMessage message);
        public delegate void WebSocketClientTextMessageEvent(string text);
        public delegate void WebSocketClientBinaryMessageEvent(byte[] data);
        public delegate void WebSocketClientErrorEvent(string errorMsg);
        public delegate void WebSocketClientCloseEvent(WebSocketClientCloseCode closeCode);
    }
}
