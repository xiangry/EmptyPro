using System;
using UnityEngine;

namespace Features.GooglePay
{
    public class GooglePlayBilling : IDisposable
    {

        public static GooglePlayBilling _instance;

        private string[] allProductIds = new[]
        {
            "android.test.purchased", //  模拟购买成功的场景。支付完成后，返回成功的购买状态
            "android.test.canceled", // 模拟用户在支付界面取消购买的场景 测试用户取消支付时的逻辑，例如提示或回退操作。
            "android.test.refunded", // 模拟支付完成后商品被退款的场景 验证退款后应用的逻辑处理，例如撤销奖励
            "android.test.item_unavailable", // 模拟商品不可购买的场景，例如商品 ID 无效或未激活。 测试商品不可用时的提示逻辑
        };
        
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

        public void FetchProducts()
        {
            if (billingManager != null)
            {
                billingManager.Call("QueryProductsDetails", allProductIds);
            }
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
    }
}