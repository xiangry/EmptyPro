using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace Framework.Log
{
    public class ServerLogger : ILogger
    {
        public void Debug(string info)
        {
            ServerClient.Instance.SendLog($"[Info][{Time.realtimeSinceStartup}]:{info}");
        }
        
        public void Debug(string tag, string info)
        {
            Debug($"[{tag}]:{info}");
        }
        
        public void Error(string info)
        {
            ServerClient.Instance.SendLog($"[Error][{Time.realtimeSinceStartup}]:{info}");
        }
        
        public void Error(string tag, string info)
        {
            Error($"[{tag}]:{info}");
        }
    }
}