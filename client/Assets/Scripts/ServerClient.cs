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
        private const string SERVER_INFO_IP_KEY = "ServerInfo_IP";
        public ServerConfigScriptObject ServerConfig;

        public string ServerIp { get; private set; } = null;
        public string ServerPort { get; private set; } = null;

        protected  override void OnInit()
        {
            LoggerEx.RegisterLogger(new ServerLogger());
            
            var ipInfo = PlayerPrefs.GetString(SERVER_INFO_IP_KEY);
            if (string.IsNullOrEmpty(ipInfo))
            {
                ServerIp = ServerConfig.SererIP;
                ServerPort = ServerConfig.Port;
            }
            else
            {
                var list = ipInfo.Split(":");
                ServerIp = list[0];
                ServerPort = list.Length > 1 ? list[1] : null;
            }
        }
        
        public void Send(string method, string content)
        {
            var url = $"http://{ServerIp}:{ServerPort}/{method}";
            var uri = new Uri(url);
            var request = new HTTPRequest(uri, HTTPMethods.Post);
            var bstr = Convert.ToBase64String(UTF8Encoding.Default.GetBytes(content));
            request.RawData = UTF8Encoding.Default.GetBytes(bstr);
            request.Send();
        }

        public void SendLog(string info)
        {
            Send(ServerConfig.LogApi, info);
        }

        public void SendPurchasingInfo(string info)
        {
            Send(ServerConfig.AcknowledgeApi, info);
        }

        public void SetNewServerInfo(string s, string s1)
        {
            ServerIp = s;
            ServerPort = s1;
            PlayerPrefs.SetString(SERVER_INFO_IP_KEY, $"{ServerIp}:{ServerPort}");
        }
    }
}