using System;
using System.Text;
using BestHTTP;
using Framework.Base;
using Framework.Log;
using Newtonsoft.Json.Serialization;
using UnityEditor;
using UnityEngine;
using ILogger = Framework.Log.ILogger;

namespace DefaultNamespace
{
    public class ServerClient : MonoSingleton<ServerClient>
    {
        public ServerConfigScriptObject ServerConfig;

        public void Init()
        {
            LoggerEx.RegisterLogger(new ServerLogger());
        }
        
        public void Send(string method, string content)
        {
            var uri = new Uri($"{ServerConfig.SererIP}:{ServerConfig.Port}/{method}");
            var request = new HTTPRequest(uri, HTTPMethods.Post);
            var bstr = Convert.ToBase64String(UTF8Encoding.Default.GetBytes(content));
            request.RawData = UTF8Encoding.Default.GetBytes(bstr);
            request.Send();
        }

        public void SendLog(string info)
        {
            Send(ServerConfig.LogApi, info);
        }
    }
}