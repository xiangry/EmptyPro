using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Launcher : MonoBehaviour
{
    public Button toastBtn;
    public Button restartBtn1;
    public Button restartBtn2;
    
    // Start is called before the first frame update
    void Start()
    {
        toastBtn.onClick.AddListener(OnReStartBtnClicked);
        restartBtn1.onClick.AddListener(OnReStart1BtnClicked);
        restartBtn2.onClick.AddListener(OnReStart2BtnClicked);
    }

    void OnReStartBtnClicked()
    {
        Debug.Log("ToastInfo");

#if UNITY_ANDROID
        // 获取 Unity 的当前 Activity
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        // 调用 Java 类中的 showToast 方法
        using (AndroidJavaClass myClass = new AndroidJavaClass("com.example.mylibrary.MyAndroidClass"))
        {
            myClass.CallStatic("ToastMessage", currentActivity, $"From Unity: {Time.realtimeSinceStartup}");
        }
#endif
    }
    
    
    void OnReStart1BtnClicked()
    {
        Debug.Log("Restart1");

#if UNITY_ANDROID
        // 获取 Unity 的当前 Activity
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        // 调用 Java 类中的 showToast 方法
        using (AndroidJavaClass myClass = new AndroidJavaClass("com.example.mylibrary.MyAndroidClass"))
        {
            myClass.CallStatic("ForceRestart", currentActivity);
        }
#endif
    }
    
    
    void OnReStart2BtnClicked()
    {
        Debug.Log("Restart2");

#if UNITY_ANDROID
        // 获取 Unity 的当前 Activity
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        // 调用 Java 类中的 showToast 方法
        using (AndroidJavaClass myClass = new AndroidJavaClass("com.example.mylibrary.MyAndroidClass"))
        {
            myClass.CallStatic("ForceRestart2", currentActivity);
        }
#endif
    }
}
