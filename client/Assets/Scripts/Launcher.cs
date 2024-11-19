using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Launcher : MonoBehaviour
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
    }

    void OnToastMessage()
    {
        PlatformNative.NativeTools.ToastMessage("Hello, Unity", 1);
    }
    
    
    void OnRestartApp()
    {
        PlatformNative.NativeTools.ToastMessage("Will Restart App");
        PlatformNative.NativeTools.RestartApp(100);
    }
    
    
    void OnReStart2BtnClicked()
    {
        Debug.Log("Restart2");
    }
    
    void OnReStart3BtnClicked()
    {
        Debug.Log("Restart2");
    }
}
