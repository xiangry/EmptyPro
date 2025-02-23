namespace SDK.Google.PlayIntegrity
{
    public class Logger
    {
        public static void Debug(string info)
        {
            UnityEngine.Debug.Log($"PlayIntegrityManager:{info}");
        }
        
        public static void Error(string info)
        {
            UnityEngine.Debug.LogError($"PlayIntegrityManager:{info}");
        }
    }
}