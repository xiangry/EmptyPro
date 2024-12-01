using System.Collections.Generic;
using UnityEngine;

namespace Framework.Log
{
    public static class LoggerEx
    {
        private static List<ILogger> _curLoggers = new List<ILogger>();

        public static void RegisterLogger(ILogger logger)
        {
            _curLoggers.Add(logger);
        }
        
        public static void Debug(string info)
        {
            foreach (var one in _curLoggers)
            {
                one.Debug(info);
            }
        }
        
        public static void Debug(string tag, string info)
        {
            foreach (var one in _curLoggers)
            {
                one.Debug(tag, info);
            }
        }
        
        public static void Error(string info)
        {
            foreach (var one in _curLoggers)
            {
                one.Error(info);
            }
        }
        
        public static void Error(string tag, string info)
        {
            foreach (var one in _curLoggers)
            {
                one.Error(tag, info);
            }
        }
    }
}