using ShadowGroveGames.SimpleWebSocketServer.Scripts.Endpoint;
using System;
using UnityEngine;

namespace ShadowGroveGames.SimpleWebSocketServer.Scripts.Struct
{
    [Serializable]
    public struct SimpleEventWebSocketServerEndpoint
    {
        [field: SerializeField]
        [field: Tooltip("EndpointPath path i.e. /Echo")]
        public string EndpointPath { get; private set; }

        [field: SerializeField]
        [field: Tooltip("WebSocket Handler")]
        public WebSocketEndpointBehaviour Handler { get; private set; }
    }
}
