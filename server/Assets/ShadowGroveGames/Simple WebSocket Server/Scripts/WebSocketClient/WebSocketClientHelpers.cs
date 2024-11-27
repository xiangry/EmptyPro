using System;

namespace ShadowGroveGames.SimpleWebSocketServer.Scripts.WebSocketClient
{
    [Serializable]
    public enum WebSocketClientCloseCode
    {
        NotSet = 0,
        Normal = 1000,
        Away = 1001,
        ProtocolError = 1002,
        UnsupportedData = 1003,
        Undefined = 1004,
        NoStatus = 1005,
        Abnormal = 1006,
        InvalidData = 1007,
        PolicyViolation = 1008,
        TooBig = 1009,
        MandatoryExtension = 1010,
        ServerError = 1011,
        TlsHandshakeFailure = 1015
    }

    public static class WebSocketClientHelpers
    {
        public static WebSocketClientCloseCode ParseCloseCodeEnum(int closeCode)
        {

            if (WebSocketClientCloseCode.IsDefined(typeof(WebSocketClientCloseCode), closeCode))
                return (WebSocketClientCloseCode)closeCode;

            return WebSocketClientCloseCode.Undefined;
        }

        public static WebSocketException GetErrorMessageFromCode(int errorCode, Exception inner)
        {
            switch (errorCode)
            {
                case -1:
                    return new WebSocketUnexpectedException("WebSocket instance not found.", inner);
                case -2:
                    return new WebSocketInvalidStateException("WebSocket is already connected or in connecting state.", inner);
                case -3:
                    return new WebSocketInvalidStateException("WebSocket is not connected.", inner);
                case -4:
                    return new WebSocketInvalidStateException("WebSocket is already closing.", inner);
                case -5:
                    return new WebSocketInvalidStateException("WebSocket is already closed.", inner);
                case -6:
                    return new WebSocketInvalidStateException("WebSocket is not in open state.", inner);
                case -7:
                    return new WebSocketInvalidArgumentException("Cannot close WebSocket. An invalid code was specified or reason is too long.", inner);
                default:
                    return new WebSocketUnexpectedException("Unknown error.", inner);
            }
        }
    }
}
