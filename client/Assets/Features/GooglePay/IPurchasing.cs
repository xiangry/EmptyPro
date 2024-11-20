using System.Collections.Generic;

namespace Features.Purchasing
{
    public interface IPurchasing
    {
        public void InitializeClient(List<string> priductIds);
        public void DoQueryProductDetails(string productId);
        public void DoLaunchPurchaseFlow(string productId);
        void DoDumpProducts();
    }
}