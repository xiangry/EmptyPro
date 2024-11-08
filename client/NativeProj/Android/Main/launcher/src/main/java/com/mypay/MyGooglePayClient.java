package com.mypay;


import android.app.Activity;
import android.content.Context;


import androidx.annotation.NonNull;

import com.android.billingclient.api.AcknowledgePurchaseParams;
import com.android.billingclient.api.AcknowledgePurchaseResponseListener;
import com.android.billingclient.api.BillingClient;
import com.android.billingclient.api.BillingClient.*;
import com.android.billingclient.api.BillingClientStateListener;
import com.android.billingclient.api.BillingConfig;
import com.android.billingclient.api.BillingConfigResponseListener;
import com.android.billingclient.api.BillingFlowParams;
import com.android.billingclient.api.BillingResult;
import com.android.billingclient.api.ConsumeParams;
import com.android.billingclient.api.ConsumeResponseListener;
import com.android.billingclient.api.GetBillingConfigParams;
import com.android.billingclient.api.ProductDetails;
import com.android.billingclient.api.ProductDetailsResponseListener;
import com.android.billingclient.api.Purchase;
import com.android.billingclient.api.PurchasesResponseListener;
import com.android.billingclient.api.PurchasesUpdatedListener;
import com.android.billingclient.api.QueryProductDetailsParams;
import com.android.billingclient.api.BillingFlowParams.ProductDetailsParams;
import com.android.billingclient.api.QueryPurchasesParams;

import java.util.List;

public class MyGooglePayClient {

    private PurchasesUpdatedListener purchasesUpdatedListener;
    AcknowledgePurchaseResponseListener acknowledgePurchaseResponseListener;

    private BillingClient billingClient;

    // 初始化 BillingClient https://developer.android.com/google/play/billing/integrate?hl=zh-cn#initialize
    public void InitGoogleClient(Context context){
        purchasesUpdatedListener = new MyPurchasesUpdatedListener();

        billingClient = BillingClient.newBuilder(context)
                .setListener(purchasesUpdatedListener)
                // Configure other settings.
                .build();

        acknowledgePurchaseResponseListener = new AcknowledgePurchaseResponseListener() {
            @Override
            public void onAcknowledgePurchaseResponse(@NonNull BillingResult billingResult) {

            }
        };
    }

    // 连接到 Google Play https://developer.android.com/google/play/billing/integrate?hl=zh-cn#connect_to_google_play
    public void ConnectGooglePay(){
        billingClient.startConnection(new BillingClientStateListener() {
            @Override
            public void onBillingSetupFinished(BillingResult billingResult) {
                if (billingResult.getResponseCode() ==  BillingClient.BillingResponseCode.OK) {
                    // The BillingClient is ready. You can query purchases here.
                }
            }
            @Override
            public void onBillingServiceDisconnected() {
                // Try to restart the connection on the next request to
                // Google Play by calling the startConnection() method.
            }
        });
    }

    // 展示可供购买的商品
    // https://developer.android.com/google/play/billing/integrate?hl=zh-cn#show-products
    public void QueryProductDetails(){
        QueryProductDetailsParams queryProductDetailsParams =
                QueryProductDetailsParams.newBuilder()
                        .setProductList(List.of(
                                QueryProductDetailsParams.Product.newBuilder()
                                        .setProductId("product_id_example")
                                        .setProductType(BillingClient.ProductType.SUBS)
                                        .build()))
                        .build();

        billingClient.queryProductDetailsAsync(
                queryProductDetailsParams,
                new ProductDetailsResponseListener() {
                    public void onProductDetailsResponse(BillingResult billingResult,
                                                         List<ProductDetails> productDetailsList) {
                        // check billingResult
                        // process returned productDetailsList
                    }
                }
        );
    }


    // 启动购买流程 https://developer.android.com/google/play/billing/integrate?hl=zh-cn#launch
    public void StartPay(Context context, String selectedOfferToken){
        ProductDetails productDetails = null;
        Activity activity = (Activity) context;
        List<BillingFlowParams.ProductDetailsParams> productDetailsParamsList =
                List.of(
                        ProductDetailsParams.newBuilder()
                                // retrieve a value for "productDetails" by calling queryProductDetailsAsync()
                                .setProductDetails(productDetails)
                                // For one-time products, "setOfferToken" method shouldn't be called.
                                // For subscriptions, to get an offer token, call
                                // ProductDetails.subscriptionOfferDetails() for a list of offers
                                // that are available to the user.
                                .setOfferToken(selectedOfferToken)
                                .build()
                );

        BillingFlowParams billingFlowParams = BillingFlowParams.newBuilder()
                .setProductDetailsParamsList(productDetailsParamsList)
                .build();

        // Launch the billing flow
        BillingResult billingResult = billingClient.launchBillingFlow(activity, billingFlowParams);

    }

    // 处理够买交易 https://developer.android.com/google/play/billing/integrate?hl=zh-cn#process

    // 消耗型商品 https://developer.android.com/google/play/billing/integrate?hl=zh-cn#process
    void handlePurchase(Purchase purchase) {
        // Purchase retrieved from BillingClient#queryPurchasesAsync or your PurchasesUpdatedListener.

        // Verify the purchase.
        // Ensure entitlement was not already granted for this purchaseToken.
        // Grant entitlement to the user.

        ConsumeParams consumeParams =
                ConsumeParams.newBuilder()
                        .setPurchaseToken(purchase.getPurchaseToken())
                        .build();

        ConsumeResponseListener listener = new ConsumeResponseListener() {
            @Override
            public void onConsumeResponse(BillingResult billingResult, String purchaseToken) {
                if (billingResult.getResponseCode() == BillingClient.BillingResponseCode.OK) {
                    // Handle the success of the consume operation.
                }
            }
        };

        billingClient.consumeAsync(consumeParams, listener);
    }

    // 非消耗型商品 https://developer.android.com/google/play/billing/integrate?hl=zh-cn#non-consumable-products
    private void handleNotConsumePurchase(Purchase purchase){
        if (purchase.getPurchaseState() == Purchase.PurchaseState.PURCHASED) {
            if (!purchase.isAcknowledged()) {
                AcknowledgePurchaseParams acknowledgePurchaseParams =
                        AcknowledgePurchaseParams.newBuilder()
                                .setPurchaseToken(purchase.getPurchaseToken())
                                .build();
                billingClient.acknowledgePurchase(acknowledgePurchaseParams, acknowledgePurchaseResponseListener);
            }
        }
    }


    // 提取够买交易 https://developer.android.com/google/play/billing/integrate?hl=zh-cn#fetch
    private void QueryPurchaseAsync(){
        billingClient.queryPurchasesAsync(
                QueryPurchasesParams.newBuilder()
                        .setProductType(ProductType.SUBS)
                        .build(),
                new PurchasesResponseListener() {
                    public void onQueryPurchasesResponse(BillingResult billingResult, List<Purchase> purchases) {
                        // check billingResult
                        // process returned purchase list, e.g. display the plans user owns

                    }
                }
        );
    }


    // 查询用户的结算配置 https://developer.android.com/google/play/billing/integrate?hl=zh-cn#mq
    private void RequestBillingConfigInfo(){
        // Use the default GetBillingConfigParams.
        GetBillingConfigParams getBillingConfigParams = GetBillingConfigParams.newBuilder().build();
        billingClient.getBillingConfigAsync(getBillingConfigParams,
                new BillingConfigResponseListener() {
                    public void onBillingConfigResponse(
                            BillingResult billingResult, BillingConfig billingConfig) {
                        if (billingResult.getResponseCode() == BillingResponseCode.OK
                                && billingConfig != null) {
                            String countryCode = billingConfig.getCountryCode();


                        } else {
                            // TODO: Handle errors
                        }
                    }
                });
    }
}
