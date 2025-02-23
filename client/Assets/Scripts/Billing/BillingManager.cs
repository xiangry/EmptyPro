using System;
using UnityEngine;
using Unity.Services.Core;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;


namespace Billing
{
    /// <summary>
    /// Unity IAP Demo Implementation.
    /// </summary>
    public class BillingManager : MonoBehaviour, IDetailedStoreListener
    {
        private static BillingManager instance;
        public static event Action<Color, string> debugCallback;
        public static event Action<bool, string> purchaseCallback;

        public ProductScriptableObject productConfig;

        //Unity IAP references
        public IStoreController controller;
        IExtensionProvider extensions;
        ConfigurationBuilder builder;


        //return the instance of this script.
        public static BillingManager GetInstance()
        {
            return instance;
        }


        //create a persistent script instance
        void Awake()
        {
            if (instance)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(this);

            instance = this;
            Initialize();
        }


        //initialize Unity IAP with demo products
        public async void Initialize()
        {
            //initialized already
            if (controller != null) return;

            try
            {
                //Unity Gaming Services are required first
                // await UnityServices.InitializeAsync();

                builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

                foreach (var productId in productConfig.ConsumableProducts)
                {
                    builder.AddProduct(productId, ProductType.Consumable);
                }
                foreach (var productId in productConfig.NonConsumableProducts)
                {
                    builder.AddProduct(productId, ProductType.NonConsumable);
                }
                foreach (var productId in productConfig.SubscriptionProducts)
                {
                    builder.AddProduct(productId, ProductType.Subscription);
                }

                //initialize Unity IAP
                UnityPurchasing.Initialize(this, builder);
            }
            catch (Exception)
            {
                OnInitializeFailed(InitializationFailureReason.PurchasingUnavailable);
            }
        }


        //fired when Unity IAP initialization completes successfully
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            DebugLogText(Color.green, "In-App Purchasing successfully initialized");
            this.controller = controller;
            this.extensions = extensions;

        }


        //fired when Unity IAP receives a purchase which is then ready for local and server-side validation
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            Product product = args.purchasedProduct;

            return PurchaseProcessingResult.Complete;
        }




        //fired when Unity IAP failed to initialize.
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            DebugLogText(Color.red, $"In-App Purchasing initialize failed: {error}");
        }


        //fired when Unity IAP failed to initialize.
        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            DebugLogText(Color.red, $"In-App Purchasing initialize failed: {message}");
        }


        //fired when Unity IAP failed to process a purchase.
        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            DebugLogText(Color.red, $"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
            purchaseCallback(false, null);
        }


        //fired when Unity IAP failed to process a purchase.
        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            DebugLogText(Color.red, $"Purchase failed - Product: '{product.definition.id}', PurchaseFailureDescription: {failureDescription.reason}, {failureDescription.message}");
            purchaseCallback(false, null);
        }


        //you would do your custom purchase handling by subscribing to the purchaseCallback event
        //for example when not making use of user inventory, save the purchase on device for offline mode
        //unlock the reward in your UI, activate something for the user, or anything else you want it to do!
        //since purchase callbacks can happen or complete anywhere, your purchase handler should also be
        //present in every scene or have DontDestroyOnLoad active as well
        public void OnPurchaseResult(bool success, string data)
        {
            purchaseCallback(success, data);
        }


        //callback for UI display purposes
        void DebugLogText(Color color, string text)
        {
            debugCallback?.Invoke(color, text);
        }
    }
}