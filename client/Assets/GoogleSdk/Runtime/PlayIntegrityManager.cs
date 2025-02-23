using UnityEngine;
using System;
using Framework.Log;

namespace GoogleSdk.Runtime
{
    public enum EIntegrityStandEvnState
    {
        UnInit = 0,
        InInit,
        Error,
        Valid,
    }

    public class PlayIntegrityManager : MonoBehaviour
    {
        private AndroidJavaObject playIntegrityHelper;
        public EIntegrityStandEvnState standEnvState { get; private set; } = EIntegrityStandEvnState.UnInit;

        public void Init()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    playIntegrityHelper = new AndroidJavaObject("com.knockgame.googlesdk.PlayIntegrityHelper", activity);
                }

                if (playIntegrityHelper == null)
                {
                    LoggerEx.Error("Failed to initialize PlayIntegrityHelper.");
                    return;
                }

                standEnvState = EIntegrityStandEvnState.InInit;
                PrepareStandardEnv(
                    success =>
                    {
                        LoggerEx.Debug("Standard prepare success");
                        standEnvState = EIntegrityStandEvnState.Valid;
                    },
                    failure =>
                    {
                        LoggerEx.Error("Standard prepare failure: " + failure);
                        standEnvState = EIntegrityStandEvnState.Error;
                    });
            }
            else
            {
                LoggerEx.Error("PlayIntegrityManager is only supported on Android platform.");
            }
        }

        public void PrepareStandardEnv(Action<string> onSuccess, Action<string> onFailure)
        {
            if (playIntegrityHelper == null)
            {
                LoggerEx.Error("PlayIntegrityHelper is not initialized.");
                onFailure?.Invoke("PlayIntegrityHelper is not initialized.");
                return;
            }

            var integrityCallback = new IntegrityCallback(onSuccess, onFailure);
            playIntegrityHelper.Call("prepareStandardEnv", integrityCallback);
        }

        public void RequestStandardToken(string requestHash, Action<string> onSuccess, Action<string> onFailure)
        {
            if (playIntegrityHelper == null)
            {
                LoggerEx.Error("PlayIntegrityHelper is not initialized.");
                onFailure?.Invoke("PlayIntegrityHelper is not initialized.");
                return;
            }

            var integrityCallback = new IntegrityCallback(onSuccess, onFailure);
            playIntegrityHelper.Call("requestStandardToken", requestHash, integrityCallback);
        }

        public void RequestIntegrityToken(string nonce, Action<string> onSuccess, Action<string> onFailure)
        {
            if (playIntegrityHelper == null)
            {
                LoggerEx.Error("PlayIntegrityHelper is not initialized.");
                onFailure?.Invoke("PlayIntegrityHelper is not initialized.");
                return;
            }

            var integrityCallback = new IntegrityCallback(onSuccess, onFailure);
            playIntegrityHelper.Call("requestIntegrityToken", nonce, integrityCallback);
        }

        private class IntegrityCallback : AndroidJavaProxy
        {
            private readonly Action<string> onSuccess;
            private readonly Action<string> onFailure;

            public IntegrityCallback(Action<string> onSuccess, Action<string> onFailure)
                : base("com.knockgame.googlesdk.IntegrityCallback")
            {
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
                    LoggerEx.Error("Integrity check failed: " + errorMessage);
                    onFailure?.Invoke(errorMessage);
                });
            }
        }
    }
}
