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
        LoggerEx.Debug($"PlayIntegrityManager init -------------");
        GetComponent<PlayIntegrityManager>().Init();
    }
    
    

    private string GenerateNonce()
    {
        byte[] nonceBytes = new byte[32];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(nonceBytes);
        }
        // 进行 Base64 URL Safe No Wrap 编码
        string nonce = Convert.ToBase64String(nonceBytes)
            .Replace('+', '-')  // URL Safe: 替换 +
            .Replace('/', '_')  // URL Safe: 替换 /
            .TrimEnd('=');      // No Wrap: 去除尾部 =
        return nonce;
    }
    
    void OnReStart3BtnClicked()
    {
        var nonce = GenerateNonce();
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
