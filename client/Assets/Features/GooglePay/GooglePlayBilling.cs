using System;
using UnityEngine;

namespace Features.GooglePay
{
    public class GooglePlayBilling : IDisposable
    {
        public static GooglePlayBilling _instance;

        public static GooglePlayBilling Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GooglePlayBilling();
                }
                return _instance;
            }
        }
        private AndroidJavaObject billingManager;

        // 初始化 BillingManager
        public void Initialize()
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            billingManager = new AndroidJavaObject($"com.kks.pay.ggpay.PayClient", activity);
        }

        // 查询商品详情
        public void QueryProductDetails(string productId)
        {
            if (billingManager != null)
            {
                billingManager.Call("QueryProductDetails", productId);
            }
        }

        // 发起购买
        public void LaunchPurchaseFlow(string productId)
        {
            if (billingManager != null)
            {
                billingManager.Call("StartPay", productId);
            }
        }
        
        public void Dispose()
        {
            // TODO release managed resources here
        }

        public void DoDumpProducts()
        {
            if (billingManager != null)
            {
                billingManager.Call("DumpAllProducts");
            }
        }
    }
}