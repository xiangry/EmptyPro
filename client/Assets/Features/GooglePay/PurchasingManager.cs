#if USE_IN_APP_PURCHASING
using System;
using System.Collections.Generic;
using Framework.Base;
using Framework.Log;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Features.Purchasing
{
    public class PurchasingManager : MonoSingleton<PurchasingManager>, IPurchasing, IDetailedStoreListener
    {
        private const string TAG = "PurchasingManager";
        
        IStoreController _storeController; // The Unity Purchasing system.

        public void InitializeClient(List<string> productIds)
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            foreach (var productId in productIds)
            {
                builder.AddProduct(productId, ProductType.Consumable);
            }
            UnityPurchasing.Initialize(this, builder);
        }

        public void DoQueryProductDetails(string productId)
        {
            throw new PurchasingException("Not supported DoQueryProductDetail");
        }

        public void DoLaunchPurchaseFlow(string productId)
        {
            _storeController.InitiatePurchase(productId);
        }

        public void DoDumpProducts()
        {
            throw new PurchasingException("Not supported DoQueryProductDetail");
        }


        #region 商店监听器
        
        public void QueryProductDetail()
        {
            LoggerEx.Debug(TAG, "QueryProductDetail");
        }

        public void LaunchPurchaseFlow()
        {
            LoggerEx.Debug(TAG, "QueryProductDetail");
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            LoggerEx.Debug(TAG, $"OnInitializeFailed: {error}");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            LoggerEx.Debug(TAG, $"OnInitializeFailed: {error} : {message}");
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            LoggerEx.Debug(TAG, $"ProcessPurchase: {purchaseEvent.purchasedProduct}");
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            LoggerEx.Debug(TAG, $"OnPurchaseFailed: {product} : {failureReason}");
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _storeController = controller;
            LoggerEx.Debug(TAG, $"OnInitialized: {controller} : {extensions}");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            LoggerEx.Debug(TAG, $"OnPurchaseFailed: {product} : {failureDescription}");
        }

        #endregion
    }
}
#endif