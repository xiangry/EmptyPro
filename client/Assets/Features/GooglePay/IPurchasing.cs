using System;
using System.Collections.Generic;
using UnityEngine.Purchasing;

namespace Features.Purchasing
{
    public interface IPurchasingState
    {
        public bool CheckPurchaseState(out string reason);

        public bool CheckProductState(string product, out string reason);
    }
    
    public interface IPurchasing
    {
        public void InitializeClient(List<string> productIds, Action<MyPurchasingEventResult> callFunc);
        public void SetCallBack(Action<MyPurchasingEventResult> callFunc);
        public void AddAdditionalProduct(List<string> productIds, ProductType productType = ProductType.Consumable);
        public void DoQueryProductDetails(string productId);
        public void DoLaunchPurchaseFlow(string productId, string payload);
        void DoDumpProducts();

    }
}