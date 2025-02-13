using UnityEngine;
using System;

namespace GoogleSdk.Runtime
{
    public class PlayIntegrityManager : MonoBehaviour
    {
        private AndroidJavaObject playIntegrityHelper;
        private IntegrityCallback integrityCallback;
        
        

        void Start()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    playIntegrityHelper = new AndroidJavaObject("com.knockgame.googlesdk.PlayIntegrityHelper", activity);
                }

                // 传递 C# 监听器到 Java 代码
                playIntegrityHelper.Call("setIntegrityCallback", new IntegrityCallback(this, OnIntegrityCallbackFailure, OnIntegrityCallbackSuccess));
            }
        }

        private void OnIntegrityCallbackFailure(string obj)
        {
            Debug.Log($"{GoogleSdkSetting.TAG}:OnIntegrityCallbackFailure {obj}");
        }

        private void OnIntegrityCallbackSuccess(string obj)
        {
            Debug.Log($"{GoogleSdkSetting.TAG}:OnIntegrityCallbackSuccess {obj}");
        }

        public void RequestIntegrityToken(Action<string> onSuccess, Action<string> onFailure)
        {
            integrityCallback = new IntegrityCallback(this, onSuccess, onFailure);
            playIntegrityHelper.Call("requestIntegrityToken");
        }

        // Java 回调类
        private class IntegrityCallback : AndroidJavaProxy
        {
            private PlayIntegrityManager manager;
            private Action<string> onSuccess;
            private Action<string> onFailure;

            public IntegrityCallback(PlayIntegrityManager manager, Action<string> onSuccess, Action<string> onFailure)
                : base("com.example.unityplayintegrity.IntegrityCallback")
            {
                this.manager = manager;
                this.onSuccess = onSuccess;
                this.onFailure = onFailure;
            }

            void onIntegritySuccess(string integrityToken)
            {
                Debug.Log("Received Integrity Token: " + integrityToken);
                onSuccess?.Invoke(integrityToken);
            }

            void onIntegrityFailure(string errorMessage)
            {
                Debug.LogError("Integrity check failed: " + errorMessage);
                onFailure?.Invoke(errorMessage);
            }
        }
    }

}