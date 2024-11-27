namespace ShadowGroveGames.SimpleWebSocketServer.Scripts.WebSocketServer
{
    public struct BaseWebsocketServerEvent
    {
        public BaseWebsocketServer Server { get; private set; }

        public WebSocketEventArg Argument { get; private set; }

        public BaseWebsocketServerEvent(BaseWebsocketServer _webSocketSevrer, WebSocketEventArg _webSocketEventArg)
        {
            Server = _webSocketSevrer;
            Argument = _webSocketEventArg;
        }
    }
}
