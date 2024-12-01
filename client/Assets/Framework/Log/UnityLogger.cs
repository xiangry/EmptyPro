using System.Collections.Generic;
using UnityEngine;

namespace Framework.Log
{
    public class UnityLogger : ILogger
    {
        public void Debug(string info)
        {
            UnityEngine.Debug.Log($"[{Time.realtimeSinceStartup}]:{info}");
        }
        
        public void Debug(string tag, string info)
        {
            Debug($"[{tag}]:{info}");
        }
        
        public void Error(string info)
        {
            UnityEngine.Debug.LogError($"[{Time.realtimeSinceStartup}]:{info}");
        }
        
        public void Error(string tag, string info)
        {
            Error($"[{tag}]:{info}");
        }
    }
}