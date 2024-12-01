#if USE_IN_APP_PURCHASING
using System;
using System.Collections.Generic;using System.Globalization;
using Framework.Base;
using Framework.Log;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Features.Purchasing
{
    public class PurchasingManager : MonoSingleton<PurchasingManager>, IPurchasing, IDetailedStoreListener, IPurchasingState
    {
        private const string TAG = "PurchasingManager";
        
        IStoreController _storeController; // The Unity Purchasing system.
        
        Action<MyPurchasingEventResult> _purchasingResultCallBack;

        private bool _isInitCall = false;

        private MyPurchasingState initResult;

        public void InitializeClient(List<string> productIds, Action<MyPurchasingEventResult> callFunc = null)
        {
            if (_isInitCall)
            {
                LoggerEx.Error(TAG, "re call InitializeClient");
                return;
            }

            _purchasingResultCallBack = callFunc;
            // StandardPurchasingModule.Instance().useFakeStoreAlways = true;
            // StandardPurchasingModule.Instance().useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
            
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            foreach (var productId in productIds)
            {
                builder.AddProduct(productId, ProductType.Consumable);
            }
            UnityPurchasing.Initialize(this, builder);
        }

        public void SetCallBack(Action<MyPurchasingEventResult> callBack)
        {
            _purchasingResultCallBack = callBack;
        }

        public void AddAdditionalProduct(List<string> productIds, ProductType productType = ProductType.Consumable)
        {
            LoggerEx.Debug(TAG, $"AddAdditionalProduct {productIds}");
            if (!CheckPurchaseState(out var reason))
            {
                return;
            }

            var additional = new HashSet<ProductDefinition>();
            foreach (var productId in productIds)
            {
                additional.Add(new ProductDefinition(productId, ProductType.Consumable));
            };

            Action onSuccess = () => {
                LoggerEx.Debug(TAG, "Fetched successfully!");
                // The additional products are added to the set of
                // previously retrieved products and are browseable
                // and purchasable.
                DoDumpProducts();
            };

            Action<InitializationFailureReason, string> onFailure = (i, r) => {
                LoggerEx.Debug(TAG, $"Fetching failed for the specified reason: {i} | {r} ");
                _purchasingResultCallBack.Invoke(new MyPurchasingEventResult(MyPurchasingEventType.AddProduct, false, i.ToString(), r.ToString()));
            };

            _storeController.FetchAdditionalProducts(additional, onSuccess, onFailure);
        }

        public void DoQueryProductDetails(string productId)
        {
            var product = _storeController.products.WithID(productId);
            LoggerEx.Debug(TAG, $"DoQueryProductDetails {productId} : {product.ConvertToString()}");
        }

        public void DoLaunchPurchaseFlow(string productId, string payload)
        {
            var product = _storeController.products.WithID(productId);
            var define = product.definition;
            var productMetadata = product.metadata;
            // var product = new Product(productDefine, productMetadata);
            _storeController.InitiatePurchase(product, payload);
        }

        public void DoDumpProducts()
        {
            LoggerEx.Debug(TAG, $"Start DoDumpProducts {_storeController.products.all.Length}");
            foreach (var one in _storeController.products.all)
            {
                LoggerEx.Debug(TAG, $"  => {one.ConvertToString()} ");
            }
        }


        #region 商店监听器
        
        public void QueryProductDetail()
        {
            LoggerEx.Debug(TAG, "QueryProductDetail");
            if (!CheckPurchaseState(out var state))
            {
                LoggerEx.Debug(TAG, $"Purchasing not prepare:{state}");
                return;
            }

            var allProducts = _storeController.products.all;
            for (int i = 0; i < allProducts.Length; i++)
            {
                var one = allProducts[i];
                LoggerEx.Debug(TAG, $"\t product {i}:{one.ConvertToString()}");
            }
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _storeController = controller;
            var result = new MyPurchasingEventResult(MyPurchasingEventType.Init, true);
            LoggerEx.Debug(TAG, $"OnInitialized: {controller} : {extensions}");
            _purchasingResultCallBack.Invoke(result);
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            initResult = new MyPurchasingState()
            {
                result = false,
                reason = error.ToString(),
            };
            var result = new MyPurchasingEventResult(MyPurchasingEventType.Init, false, error.ToString());
            LoggerEx.Debug(TAG, $"OnInitializeFailed: {error}");
            _purchasingResultCallBack?.Invoke(result);
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            initResult = new MyPurchasingState()
            {
                result = false,
                reason = error.ToString(),
            };
            LoggerEx.Debug(TAG, $"OnInitializeFailed: {error} : {message}");
            var result = new MyPurchasingEventResult(MyPurchasingEventType.Init, false, error.ToString(), message);
            _purchasingResultCallBack?.Invoke(result);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            LoggerEx.Debug(TAG, $"ProcessPurchase: - Product{purchaseEvent.purchasedProduct.ConvertToString()}");
            var result = new MyPurchasingEventResult(MyPurchasingEventType.PurchasingSuccess, true);
            result.SetPurchasingInfo(purchaseEvent.purchasedProduct.receipt);
            _purchasingResultCallBack.Invoke(result);
            return PurchaseProcessingResult.Pending;
            // if(processPurchaseCallback == null) 
            //     return PurchaseProcessingResult.Complete;
            //     
            // return processPurchaseCallback.Invoke(purchaseEvent);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            LoggerEx.Debug(TAG, $"OnPurchaseFailed: {product.ConvertToString()} : failureReason:{failureReason}");
            var result = new MyPurchasingEventResult(MyPurchasingEventType.Purchasing, false, $"{failureReason}");
            _purchasingResultCallBack?.Invoke(result);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            LoggerEx.Debug(TAG, $"OnPurchaseFailed: {product.ConvertToString()} : failureDescription:{failureDescription}");
            var result = new MyPurchasingEventResult(MyPurchasingEventType.Purchasing, false);
            _purchasingResultCallBack?.Invoke(result);
        }

        #endregion


        #region Purchase State  
        public bool CheckPurchaseState(out string reason)
        {
            if (_storeController == null)
            {
                reason = string.Empty;
                return false;
            }
            
            if (!_isInitCall)
            {
                reason = EPurchasingCode.NotCallInit.ToString();
                return false;
            }

            if (initResult == null)
            {
                reason = EPurchasingCode.InitNotResponse.ToString();
                return false;
            }

            if (initResult.result == false)
            {
                reason = initResult.reason;
                return false;
            }
            
            reason = EPurchasingCode.UnKnown.ToString();
            return false;
        }

        public bool CheckProductState(string productId, out string reason)
        {
            if (!CheckPurchaseState(out reason))
                return false;

            return true;
        }
        #endregion
    }
}

public class MyPurchasingState
{
    public bool result = false;
    public string reason = string.Empty;
}

public enum MyPurchasingEventType
{
    None = 0,
    Init = 1,
    AddProduct = 2,
    Purchasing = 3,
    PurchasingSuccess = 3,
}

public class MyPurchasingEventResult
{
    private MyPurchasingEventType eventType = MyPurchasingEventType.None;
    public bool result = false;
    public string reason = string.Empty;
    public string message = string.Empty;

    #region 订单信息

    public string receipt;
    #endregion
    
    private MyPurchasingEventResult(){}

    public MyPurchasingEventResult(MyPurchasingEventType eventType, bool result = true, string reason = null, string message = null)
    {
        this.eventType = eventType;
        this.result = result;
        this.reason = reason;
        this.message = message;
    }

    public void SetPurchasingInfo(string receipt)
    {
        this.receipt = receipt;
    }

    public override string ToString()
    {
        return $"{eventType}-{result}-{reason}-{message}";
    }
}

public static class ProductExt {
    public static string ConvertToString(this Product product)
    {
        return
            $"definition:{product.definition.ConvertToString()}|metadata:{product.metadata.ConvertToString()}|receipt:{product.receipt}|availableToPurchase:{product.availableToPurchase}|transactionID:{product.transactionID}|hasReceipt:{product.hasReceipt}";
    }
    
    public static string ConvertToString(this ProductDefinition definition)
    {
        return $"[{definition.id}-enabled:{definition.enabled}-{definition.type}-{definition.storeSpecificId}]";
    }
    
    public static string ConvertToString(this ProductMetadata metadata)
    {
        var googleMeta = metadata.GetAppleProductMetadata();
        return $"[{metadata.localizedTitle}-{metadata.localizedPriceString}-{metadata.localizedPrice}-{googleMeta}]";
    }
}


public enum EPurchasingCode
{
    UnKnown = 0,
    NotCallInit = 1,
    InitNotResponse = 2,
}
#endif