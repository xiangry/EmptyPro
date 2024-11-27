using ShadowGroveGames.SimpleWebSocketServer.Scripts.Struct;
using System;

namespace ShadowGroveGames.SimpleWebSocketServer.Scripts
{
    [Obsolete("This class is obsolete, please use WebsocketServerListeningAddress instead.")]
    [Serializable]
    public class WebSocketBe : WebsocketServerListeningAddress
    {

    }

    [Obsolete("This class is obsolete, please use WebsocketServerListeningAddressType instead.")]
    [Serializable]
    public enum ListeningAddressType
    {
        Local,
        Any
    }
}
