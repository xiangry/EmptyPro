using UnityEngine;
using System;
using Framework.Log;

namespace GoogleSdk.Runtime
{
    public class PlayIntegrityManager : MonoBehaviour
    {
        private AndroidJavaObject playIntegrityHelper;
        private IntegrityCallback integrityCallback;

        public string _callbackFailure;
        public string _callbackString;

        void Start()
        {
        }

        public void Init()
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
            _callbackFailure = obj;
            LoggerEx.Debug($"{GoogleSdkSetting.TAG}:OnIntegrityCallbackFailure {obj}");
        }

        private void OnIntegrityCallbackSuccess(string obj)
        {
            _callbackString = obj;
            LoggerEx.Debug($"{GoogleSdkSetting.TAG}:OnIntegrityCallbackSuccess {obj}");
        }

        public void RequestIntegrityToken(string nonce, Action<string> onSuccess, Action<string> onFailure)
        {
            integrityCallback = new IntegrityCallback(this, onSuccess, onFailure);
            
            // 传递 C# 监听器到 Java 代码
            playIntegrityHelper.Call("setIntegrityCallback", integrityCallback);
            playIntegrityHelper.Call("requestIntegrityToken", nonce);
        }

        // Java 回调类
        private class IntegrityCallback : AndroidJavaProxy
        {
            private PlayIntegrityManager manager;
            private Action<string> onSuccess;
            private Action<string> onFailure;

            public IntegrityCallback(PlayIntegrityManager manager, Action<string> onSuccess, Action<string> onFailure)
                : base("com.knockgame.googlesdk.IntegrityCallback")
            {
                this.manager = manager;
                this.onSuccess = onSuccess;
                this.onFailure = onFailure;
            }

            void onIntegritySuccess(string integrityToken)
            {
                UnityMainThreadDispatcher.RunOnMainThread(() =>
                {
                    LoggerEx.Debug("Received Integrity Token: " + integrityToken);
                    onSuccess?.Invoke(integrityToken);
                });
            }

            void onIntegrityFailure(string errorMessage)
            {
                UnityMainThreadDispatcher.RunOnMainThread(() =>
                {
                    LoggerEx.Debug("Integrity check failed: " + errorMessage);
                    onFailure?.Invoke(errorMessage);
                });
            }
        }
    }

}