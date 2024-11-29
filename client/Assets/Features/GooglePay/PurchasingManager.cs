#if USE_IN_APP_PURCHASING
using System;
using System.Collections.Generic;
using Framework.Base;
using Framework.Log;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Features.Purchasing
{
    public class PurchasingManager : MonoSingleton<PurchasingManager>, IPurchasing, IDetailedStoreListener
    {
        private const string TAG = "PurchasingManager";
        
        IStoreController _storeController; // The Unity Purchasing system.
        
        Func<PurchaseEventArgs, PurchaseProcessingResult> processPurchaseCallback;
        Action<Product, PurchaseFailureDescription> purchaseFailedCallback;
        Action<InitializationFailureReason, string> initializedCallback;

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
            var product = _storeController.products.WithID(productId);
            var define = product.definition;
            var productMetadata = product.metadata;
            // var product = new Product(productDefine, productMetadata);
            _storeController.InitiatePurchase(product, "productId_1234567890");
        }

        public void DoDumpProducts()
        {
            LoggerEx.Debug(TAG, $"Start DoDumpProducts {_storeController.products.all.Length}");
            foreach (var one in _storeController.products.all)
            {
                LoggerEx.Debug(TAG, $"  => {one.definition.id} : {one.metadata.localizedPriceString}|{one.metadata}|{one}");
            }
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
            initializedCallback?.Invoke(error, "null");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            LoggerEx.Debug(TAG, $"OnInitializeFailed: {error} : {message}");
            initializedCallback?.Invoke(error, message);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            LoggerEx.Debug(TAG, $"ProcessPurchase: - Product{purchaseEvent.purchasedProduct}");

            return PurchaseProcessingResult.Pending;
            // if(processPurchaseCallback == null) 
            //     return PurchaseProcessingResult.Complete;
            //     
            // return processPurchaseCallback.Invoke(purchaseEvent);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            LoggerEx.Debug(TAG, $"OnPurchaseFailed: {product} : {failureReason}");
            purchaseFailedCallback?.Invoke(product, new PurchaseFailureDescription("unknown", failureReason, "unknown"));
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _storeController = controller;
            LoggerEx.Debug(TAG, $"OnInitialized: {controller} : {extensions}");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            LoggerEx.Debug(TAG, $"OnPurchaseFailed: {product} : {failureDescription}");
            purchaseFailedCallback?.Invoke(product, failureDescription);
        }

        #endregion
    }
}
#endif