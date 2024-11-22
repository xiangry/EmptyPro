using Newtonsoft.Json.Linq;
using ShadowGroveGames.SimpleWebSocketServer.Scripts.WebSocketServer;
using UnityEngine;

namespace ShadowGroveGames.SimpleWebSocketServer.Scripts.Endpoint
{
    public static class BaseWebsocketServerExtension
    {
        /// <summary>
        /// Overload method for sending json objects to a specific client, using it's id, synchronous.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="clientId"></param>
        /// <returns>True when send succesfully.</returns>
        public static bool SendJsonMessage(this BaseWebsocketServer server, JObject data, string clientId = null)
        {
            string json = data.ToString();

            return server.SendPongMessage(json, clientId);
        }

        /// <summary>
        /// Overload method for sending objects messages to a specific client, using it's id, synchronous.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="clientId"></param>
        /// <returns>True when send succesfully.</returns>
        public static bool SendJsonMessage<T>(this BaseWebsocketServer server, T data, string clientId = null)
        {
            string json = JsonUtility.ToJson(data);

            return server.SendTextMessage(json, clientId);
        }
    }
}
