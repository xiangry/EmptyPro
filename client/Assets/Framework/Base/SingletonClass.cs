using System;
using UnityEngine;

namespace Framework.Base
{
    public abstract class SingletonClass<T> where T: SingletonClass<T>, new()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T(); 
                    _instance.Init();
                }

                return _instance;
            }
        }

        private void Init()
        {
            try
            {
                OnInit();
            }
            catch (Exception e)
            {
                Debug.LogError($"Init {this.GetType()} failed.{e}");
            }
        }

        abstract protected void OnInit();
    }
}