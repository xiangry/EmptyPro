using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DefaultNamespace;
using Framework.Log;
using GoogleSdk.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class IntegrityTest : MonoBehaviour
{
    public Button toastBtn;
    public Button restartBtn1;
    public Button restartBtn2;
    public Button restartBtn3;
    // Start is called before the first frame update
    
    
    
    void Start()
    {
        toastBtn.onClick.AddListener(OnToastMessage);
        restartBtn1.onClick.AddListener(OnRestartApp);
        restartBtn2.onClick.AddListener(OnReStart2BtnClicked);
        restartBtn3.onClick.AddListener(OnReStart3BtnClicked);
        
        LoggerEx.RegisterLogger(new UnityLogger());
        // ServerClient.Instance.Init();
        
        GetComponent<PlayIntegrityManager>().Init();
    }

    void OnToastMessage()
    {
        LoggerEx.Debug($"OnToastMessage -------------");
        PlatformNative.NativeTools.ToastMessage("Hello, Unity", 1);
    }
    
    void OnRestartApp()
    {
        LoggerEx.Debug($"OnRestartApp -------------");
        PlatformNative.NativeTools.ToastMessage("Will Restart App");
        PlatformNative.NativeTools.RestartApp(100);
    }
    
    
    void OnReStart2BtnClicked()
    {
        var requestHash = FakeIntegrityVerifierServer.GenerateNonce(42);
        LoggerEx.Debug($"RequestStandardToken ------------- {requestHash}");
        GetComponent<PlayIntegrityManager>().RequestStandardToken(requestHash,(success) =>
        {
            LoggerEx.Debug($"RequestStandardToken success:{success}");
        }, failure =>
        {
            LoggerEx.Debug($"RequestStandardToken success:{failure}");
        } );
    }
    
    void OnReStart3BtnClicked()
    {
        var nonce = FakeIntegrityVerifierServer.GenerateNonce(42);
        LoggerEx.Debug($"RequestIntegrityToken ------------- {nonce}");
        GetComponent<PlayIntegrityManager>().RequestIntegrityToken(nonce,(success) =>
        {
            LoggerEx.Debug($"RequestIntegrityToken success:{success}");
        }, failure =>
        {
            LoggerEx.Debug($"RequestIntegrityToken success:{failure}");
        } );
    }
}
