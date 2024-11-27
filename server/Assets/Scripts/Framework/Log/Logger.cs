using UnityEngine;

namespace Framework.Log
{
    public static class LoggerEx
    {
        public static void Debug(string info)
        {
            UnityEngine.Debug.Log($"[{Time.realtimeSinceStartup}]:{info}");
        }
        
        public static void Debug(string tag, string info)
        {
            Debug($"[{tag}]:{info}");
        }
        
        public static void Error(string info)
        {
            UnityEngine.Debug.LogError($"[{Time.realtimeSinceStartup}]:{info}");
        }
        
        public static void Error(string tag, string info)
        {
            Error($"[{tag}]:{info}");
        }
    }
}