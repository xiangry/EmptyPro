using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShadowGroveGames.SimpleWebSocketServer.Scripts.WebSocketServer
{
    /// <summary>
    /// Interface for WebSocketHttpServer class.
    /// </summary>
    public interface IWebSocketHttpServer
    {
        /// <summary>
        /// Starts a server within a task.
        /// </summary>
        /// <returns></returns>
        Task RunServer();

        /// <summary>
        /// Sets a global value within the class that tells the server to stop running.
        /// </summary>
        void Stop();

        /// <summary>
        /// Is the server running?
        /// </summary>
        /// <returns></returns>
        bool IsRunning();
    }

    /// <summary>
    /// Runs the http server where a client needs to connect to.
    /// </summary>
    public class WebSocketHttpServer : IWebSocketHttpServer
    {
        /// <summary>
        /// Contains the ipaddress to listen to.
        /// </summary>
        private readonly IPAddress _IPAddress;

        /// <summary>
        /// Contains the Port to listen to.
        /// </summary>
        private readonly int _Port;

        /// <summary>
        /// Contains the handler for websocket servers.
        /// </summary>
        private readonly IWebSocketHandler _WebsocketHandler;

        /// <summary>
        /// Contains the list with baseurl paths to listen to.
        /// </summary>
        private readonly List<string> _BaseUrls;

        /// <summary>
        /// Tells the server task to stop and disconnect.
        /// </summary>
        private bool _stop = true;

        /// <summary>
        /// Connection cancellation token source
        /// </summary>
        private CancellationTokenSource _tokenSource = null;

        /// <summary>
        /// Connection cancellation token
        /// </summary>
        private CancellationToken _cancellationToken;

        /// <summary>
        /// TcpListener instance.
        /// </summary>
        private TcpListener _tcpListener = null;

        /// <summary>
        /// TcpClient instance.
        /// </summary>
        private TcpClient _tcpClient = null;

        /// <summary>
        /// Base constructor for setting up a HttpServer to provide the endpoint to where a WebSocket can connect to.
        /// </summary>
        /// <param name="websocketHandler">WebSocket Handler instance.</param>
        /// <param name="settings">Settings for the http server.</param>
        public WebSocketHttpServer(IWebSocketHandler websocketHandler, BaseWebSocketServerSettings settings)
        {
            _BaseUrls = settings.BaseUrls;
            _IPAddress = settings.IPAddress;
            _Port = settings.Port;
            _WebsocketHandler = websocketHandler;
        }

        /// <summary>
        /// Checks if given url path is one of the base paths, it ignores it otherwise.
        /// </summary>
        /// <param name="toCheck">Path to check.</param>
        /// <returns>String with the basepath if given string is the same, otherwise null.</returns>
        public string CheckIfBaseUrl(string toCheck)
        {
            foreach (string baseUrl in _BaseUrls)
            {
                if (toCheck.Contains(baseUrl))
                {
                    return baseUrl;
                }
            }

            return null;
        }

        /// <summary>
        /// Checks if the url used by the client contains more than the baseurl, which will be used as a customid.
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="fullBaseUrl"></param>
        /// <returns>string with id if there is one, else null</returns>
        public string CheckIfCustomIdIsSet(string baseUrl, string fullBaseUrl)
        {
            try
            {

                string id = "";
                if (baseUrl.Length == 1)
                {
                    id = fullBaseUrl.Substring(baseUrl.Length);
                }
                else
                {
                    id = fullBaseUrl.Substring(baseUrl.Length + 1);
                }

                if (id.Length > 0)
                {
                    return id;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// Generates the response header for a websocket upgrade.
        /// </summary>
        /// <param name="webSocketKey">Key from client.</param>
        /// <returns>Byte array with response header.</returns>
        public byte[] GenerateAcceptResponse(string webSocketKey)
        {
            string concatenatedKey = string.Concat(webSocketKey, "258EAFA5-E914-47DA-95CA-C5AB0DC85B11");
            string hashedKey = Hash(concatenatedKey);
            string headerToSendBack = "HTTP/1.1 101 Switching Protocols\r\n" +
                                      "Upgrade: websocket\r\n" +
                                      "Connection: Upgrade\r\n" +
                                      "Sec-WebSocket-Accept: " + hashedKey +
                                      "\r\n\r\n";

            return Encoding.ASCII.GetBytes(headerToSendBack);
        }

        /// <summary>
        /// Runs the http server inside a async task.
        /// </summary>
        /// <returns></returns>
        public async Task RunServer()
        {
            _tcpListener = new TcpListener(_IPAddress, _Port);
            _tcpListener.Start();
            byte[] buffer = new byte[1024];
            _stop = false;

            _tokenSource = new CancellationTokenSource();
            _cancellationToken = _tokenSource.Token;

            while (!_stop)
            {
                try
                {
                    _tcpClient = _tcpListener.AcceptTcpClient();
                } catch (SocketException)
                {
                    _tokenSource?.Cancel();
                    _stop = true;
                    return;
                }

                NetworkStream stream = _tcpClient.GetStream();
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, _cancellationToken);
                string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                try
                {
                    string baseUrlEncoded = received.Split(new string[] { "GET " }, StringSplitOptions.None)[1].Split(new string[] { " HTTP" }, StringSplitOptions.None)[0];
                    string baseUrlDecoded = WebUtility.UrlDecode(baseUrlEncoded);
                    string baseUrl = CheckIfBaseUrl(baseUrlDecoded);

                    if (received.Contains("Sec-WebSocket-Key:") && baseUrl != null)
                    {
                        string webSocketKey = received.Split(new string[] { "Sec-WebSocket-Key:" }, StringSplitOptions.None)[1].Split(new string[] { "\r\n" }, StringSplitOptions.None)[0].Trim();

                        byte[] acceptMessage = GenerateAcceptResponse(webSocketKey);
                        stream.Write(acceptMessage, 0, acceptMessage.Length);

                        _WebsocketHandler.StartConnection(_tcpClient, Guid.NewGuid().ToString("N"), baseUrlDecoded);
                    }
                    else
                    {
                        string headerToSendBack = "HTTP/1.1 403 Forbidden" + "\r\n\r\n";
                        byte[] msg = Encoding.ASCII.GetBytes(headerToSendBack);
                        stream.Write(msg, 0, msg.Length);
                    }
                }
                catch (Exception ex)
                {
                    string headerToSendBack = $"HTTP/1.1 500 {ex.Message}" + "\r\n\r\n";
                    byte[] msg = Encoding.ASCII.GetBytes(headerToSendBack);
                    stream.Write(msg, 0, msg.Length);
                }
            }

            _tokenSource.Cancel();
            _stop = true;
        }

        /// <summary>
        /// Cancel the WebSocket read connection
        /// </summary>
        private void CancelRead()
        {
            _tokenSource?.Cancel();
        }

        /// <summary>
        /// Stops the http server.
        /// </summary>
        public void Stop()
        {
            _tcpClient?.Dispose();
            _tcpListener?.Stop();
            CancelRead();
            _stop = true;
        }

        /// <summary>
        /// Is the server running?
        /// </summary>
        /// <returns></returns>
        public bool IsRunning()
        {
            return !_stop;
        }

        /// <summary>
        /// Generates a hash used for generating the response header when accepting a websocket connection.
        /// </summary>
        /// <param name="input">String to hash.</param>
        /// <returns>SHA1 hashed string.</returns>
        private string Hash(string input)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(hash);
            }
        }
    }
}
