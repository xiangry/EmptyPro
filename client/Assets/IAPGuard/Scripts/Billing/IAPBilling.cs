using System.Collections.Generic;
using FLOBUK.IAPGUARD;
using FLOBUK.IAPGUARD.Demo;
using Framework.Base;
using Framework.Log;
using SimpleJSON;
using UnityEngine;

namespace Billing
{
    public class IAPBilling : SingletonClass<IAPBilling>
    {
        private IAPManager instance;

        protected override void OnInit()
        {
            //get instance
            instance = IAPManager.GetInstance();
            if (!instance) return;

            //subscribe to callbacks
            IAPGuard.inventoryCallback += InventoryRetrieved;
            IAPManager.purchaseCallback += PurchaseResult;
            IAPManager.debugCallback += PrintMessage;
        }

        #region 外部调用方法
        //try to get inventory
        public void GetInventory()
        {
            if(!IAPGuard.Instance.CanRequestInventory())
            {
                PrintMessage("Inventory call is not possible. If you are on a paid plan, check your selected Inventory Request Type.");
                return;
            }

            IAPGuard.Instance.RequestInventory();
        }
        
        //buy method triggering Unity IAP
        void DoBuy(string productId, string payload)
        {
            PrintMessage( $"Do Purchase product:{productId}-{payload}");
            instance.controller.InitiatePurchase(productId, payload);
        }
        #endregion
        
        

        //IAPGuard.inventoryCallback
        void InventoryRetrieved()
        {
            PrintMessage("Inventory retrieved.");

            var inventory = IAPGuard.Instance.GetInventory();
            foreach (string productID in inventory.Keys)
                PrintMessage(productID + ": " + inventory[productID].ToString());
        }
        
        //IAPManagerDemo.purchaseCallback
        //result is JSONNode or null
        void PurchaseResult(bool success, JSONNode result)
        {
            //Log output
            switch (success)
            {
                case true:
                    PrintMessage("Purchase validation success!");
                    break;

                case false:
                    PrintMessage("Purchase validation failed.");
                    break;
            }

            if (result != null)
            {
                PrintMessage("Raw: " + result.ToString());
                PrintMessage("Product purchase: " + result["data"]["productId"]);
                PrintMessage("Purchase result: " + success);
                PrintMessage("See Log for more information!");
            }
            else
                PrintMessage("Purchase cancelled.");
        }

        //message display
        void PrintMessage(string text)
        {
            LoggerEx.Debug("IAPBilling", text);
        }

        private void PrintMessage(Color arg1, string arg2)
        {
            LoggerEx.Debug("IAPBilling", arg2);
        }
    }
}