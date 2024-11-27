using System;
using System.Net;
using UnityEngine;

namespace ShadowGroveGames.SimpleWebSocketServer.Scripts.Struct
{
    [Serializable]
    public class WebsocketServerListeningAddress
    {
        /// <summary>
        /// Port on which the _webServer should listen. 1 - 49150
        /// </summary>
        [Min(1)]
        [SerializeField]
        [Tooltip("Port on which the _webServer should listen. 1 - 49150")]
        private int _port = 13100;

        /// <summary>
        /// Select who is allowed to connect to your web server.
        /// 
        /// Local: Allows connections only from the Device running the game.
        /// Any: Allows connections from any device in the local network.
        /// </summary>
        [SerializeField]
        [Tooltip("Select who is allowed to connect to your web server.\n\nLocal: Allows connections only from the Device running the game.\nLAN: Allows connections from any device in the local network.")]
        private WebsocketServerListeningAddressType _host = WebsocketServerListeningAddressType.Local;

        public int Port { get { return _port; } }

        public WebsocketServerListeningAddressType Host { get { return _host; } }

        public IPAddress HostIPAddress => (_host == WebsocketServerListeningAddressType.Local ? IPAddress.Loopback : IPAddress.Any);
    }

    [Serializable]
    public enum WebsocketServerListeningAddressType
    {
        Local,
        Any
    }
}
