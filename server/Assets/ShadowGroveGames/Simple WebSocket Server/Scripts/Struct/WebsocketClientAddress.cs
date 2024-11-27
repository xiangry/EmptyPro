using System;
using UnityEngine;

namespace ShadowGroveGames.SimpleWebSocketServer.Scripts.Struct
{
    [Serializable]
    public class WebsocketClientAddress
    {
        /// <summary>
        /// Host of the server to connect to. For example: 192.168.178.55
        /// </summary>
        [Tooltip("Address of the server to connect to. For example: 192.168.178.55")]
        public string Host = "";

        /// <summary>
        /// Path of the server to connect to. For example: /echo
        /// </summary>
        [Tooltip("Path of the server to connect to. For example: /echo")]
        public string Path = "";

        /// <summary>
        /// Port on which the client should connect. For example: 13100
        /// </summary>
        [Min(1)]
        [Tooltip("Port on which the client should connect.")]
        public int Port = 13100;

        /// <summary>
        /// Get the address of the server to connect to.
        /// </summary>
        public string Address => $"ws://{Host}:{Port}/{Path.TrimStart('/')}";

        /// <summary>
        /// Get the Uri of the server to connect to.
        /// </summary>
        public Uri Uri => new Uri(Address);
    }
}
