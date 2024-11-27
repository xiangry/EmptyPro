using ShadowGroveGames.SimpleWebSocketServer.Scripts.WebSocketServer;
using System;
using UnityEngine;

namespace ShadowGroveGames.SimpleWebSocketServer.Scripts.Endpoint
{
    public abstract class WebSocketEndpointBehaviour : MonoBehaviour
    {

        public BaseWebsocketServer Server { get; private set; } = null;
        public string EndpointPath { get; private set; } = null;

        public void SetServer(BaseWebsocketServer server, string endpointPath)
        {
            if (Server != null && server != null)
                throw new Exception("This websocket endpoint behaviour is already in use!");

            Server = server;
            EndpointPath = (endpointPath != null ? endpointPath.Trim('/') : null);
        }

        public virtual void OnServerReady() { }
        public virtual void OnServerShutdown() { }
        public virtual void OnConnect(WebSocketEventArg argument) { }
        public virtual void OnPing(WebSocketEventArg argument) { }
        public virtual void OnPong(WebSocketEventArg argument) { }
        public virtual void OnTextData(string text, WebSocketEventArg argument) { }
        public virtual void OnData(byte[] data, WebSocketEventArg argument) { }
        public virtual void OnDisconnect(WebSocketEventArg argument) { }
    }
}
