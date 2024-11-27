using System;
using Data.DB;
using Framework.Log;
using UnityEngine;

namespace Launcher
{
    public class Launcher : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            LoggerEx.Debug($"Launcher", "Init DBManager");
            DBManager.Instance.Init();
        }
    }
}