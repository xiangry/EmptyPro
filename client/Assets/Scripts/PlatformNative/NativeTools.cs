using UnityEngine;

namespace PlatformNative
{
    public static class NativeTools
    {

        private const string ANDROID_NATIVE_TOOL_CLASS_PACKAGE = "com.ggxx.nativetools.NativeTools";
        
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="toastType">0:SHORT 1:LONG</param>
        public static void ToastMessage(string message, int toastType = 0)
        {
#if DEBUG
            Debug.Log($"[Unity]:Toast Message({toastType}):{message}");
#endif
            
#if UNITY_ANDROID
            // 获取 Unity 的当前 Activity
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            // 调用 Java 类中的 showToast 方法
            using (AndroidJavaClass myClass = new AndroidJavaClass(ANDROID_NATIVE_TOOL_CLASS_PACKAGE))
            {
                myClass.CallStatic("ToastMessage", currentActivity, message, toastType);
            }
#endif
        }
        
        public static void RestartApp(int delayMillis)
        {
#if DEBUG
            Debug.Log($"[Unity]:Native Android call RestartApp delay {delayMillis}");
#endif
            
#if UNITY_ANDROID
            // 获取 Unity 的当前 Activity
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            // 调用 Java 类中的 showToast 方法
            using (AndroidJavaClass myClass = new AndroidJavaClass(ANDROID_NATIVE_TOOL_CLASS_PACKAGE))
            {
                myClass.CallStatic("RestartApp", currentActivity, delayMillis);
            }
#endif
        }
    }
}