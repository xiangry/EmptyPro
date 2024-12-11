using System;
using UnityEngine;

namespace Framework.Base
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T:MonoSingleton<T>, new()
    {
        private static T _instance;
        private static readonly object _lock = new object();
        private static bool _applicationIsQuitting = false;

        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting)
                {
                    Debug.LogWarning($"[MonoSingleton] Instance of {typeof(T)} is already destroyed. Returning null.");
                    return default(T);
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        // 查找场景中的现有实例
                        _instance = FindObjectOfType<T>();

                        // 如果场景中没有，创建一个新对象
                        if (_instance == null)
                        {
                            GameObject singletonObject = new GameObject(typeof(T).Name);
                            _instance = singletonObject.AddComponent<T>();
                            DontDestroyOnLoad(singletonObject); // 防止对象在场景切换时被销毁
                        }
                        _instance.Init();
                    }
                    return _instance;
                }
            }
        }

        protected abstract void OnInit();
        public void Init()
        {
            try
            {
                OnInit();
            }
            catch (Exception e)
            {
                Debug.LogError($"Init Error {this}");
            }
        }
        
        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                this.Init();
                DontDestroyOnLoad(gameObject); // 防止对象在场景切换时被销毁
            }
            else if (_instance != this)
            {
                Destroy(gameObject); // 如果已有实例，销毁重复的对象
            }
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        protected virtual void OnApplicationQuit()
        {
            _applicationIsQuitting = true;
        }
    }
}