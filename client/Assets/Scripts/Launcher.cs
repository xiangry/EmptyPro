using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Launcher : MonoBehaviour
{
    public Button restartBtn;
    
    // Start is called before the first frame update
    void Start()
    {
        restartBtn.onClick.AddListener(OnReStartBtnClicked);
    }

    void OnReStartBtnClicked()
    {
        Debug.Log("点击重启安努");
    }
}
