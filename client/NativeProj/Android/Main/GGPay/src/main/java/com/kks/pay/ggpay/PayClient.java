package com.kks.pay.ggpay;

import android.app.Activity;
import android.util.Log;


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

import java.util.ArrayList;
import java.util.List;

public class PayClient {

    private PurchasesUpdatedListener purchasesUpdatedListener;
    AcknowledgePurchaseResponseListener acknowledgePurchaseResponseListener;

    private BillingClient billingClient;

    private Activity activity;

    public PayClient(Activity activity){
        this.activity = activity;

        InitGoogleClient();
    }

    private int InitClientCode = -1;
    private int ConnectClientCode = -1;

    private static String TAG = "GGPay Billing";

    // 初始化 BillingClient https://developer.android.com/google/play/billing/integrate?hl=zh-cn#initialize
    public void InitGoogleClient(){
        Log.i(TAG, "Start InitGoogleClient step 0" );
        purchasesUpdatedListener = new MyPurchasesUpdatedListener();

        Log.i(TAG, "Start InitGoogleClient step 1" );
        Builder builder = BillingClient.newBuilder(activity);
        builder.setListener(purchasesUpdatedListener);
        builder.enablePendingPurchases();
        billingClient = builder
                // Configure other settings.
                .build();

        Log.i(TAG, "Start InitGoogleClient step 2" );
        acknowledgePurchaseResponseListener = new AcknowledgePurchaseResponseListener() {
            @Override
            public void onAcknowledgePurchaseResponse(@NonNull BillingResult billingResult) {
                Log.i(TAG, "Start InitGoogleClient onAcknowledgePurchaseResponse " + billingResult.toString());
                InitClientCode = 1;
            }
        };

        Log.i(TAG, "Start InitGoogleClient step 3" );
        ConnectGooglePay();
    }

    // 连接到 Google Play https://developer.android.com/google/play/billing/integrate?hl=zh-cn#connect_to_google_play
    public void ConnectGooglePay(){
        Log.i(TAG, "Start ConnectGooglePay " );
        billingClient.startConnection(new BillingClientStateListener() {
            @Override
            public void onBillingSetupFinished(BillingResult billingResult) {
                Log.i(TAG, "Start ConnectGooglePay onBillingSetupFinished " + billingResult.toString());
                if (billingResult.getResponseCode() ==  BillingClient.BillingResponseCode.OK) {
                    // The BillingClient is ready. You can query purchases here.
                    ConnectClientCode = 1;
                }
            }
            @Override
            public void onBillingServiceDisconnected() {
                // Try to restart the connection on the next request to
                // Google Play by calling the startConnection() method.
                Log.i(TAG, "Start ConnectGooglePay onBillingServiceDisconnected ");
                ConnectClientCode = -2;
            }
        });
    }

    // 展示可供购买的商品
    // https://developer.android.com/google/play/billing/integrate?hl=zh-cn#show-products
    public void QueryProductDetails(String productId){
        Log.i(TAG, "Start QueryProductDetails one " + productId.toString());
        QueryProductDetailsParams queryProductDetailsParams =
                QueryProductDetailsParams.newBuilder()
                        .setProductList(List.of(
                                QueryProductDetailsParams.Product.newBuilder()
                                        .setProductId(productId)
                                        .setProductType(ProductType.INAPP)
                                        .build()))
                        .build();

        billingClient.queryProductDetailsAsync(
                queryProductDetailsParams,
                new ProductDetailsResponseListener() {
                    public void onProductDetailsResponse(BillingResult billingResult,
                                                         List<ProductDetails> productDetailsList) {
                        // check billingResult
                        // process returned productDetailsList
                        Log.i(TAG, "OnResponse QueryProductDetails  one billingResult " + billingResult.toString() + " details:" + productDetailsList.size());
                        for (int i = 0; i < productDetailsList.size(); i++) {
                            Log.i(TAG, "OnResponse QueryProductDetails  one [i]" + productDetailsList.get(i).toString());
                        }
                    }
                }
        );
    }


    // 查询商品详情
    public void QueryProductsDetails(List<String> productIds) {
        Log.i(TAG, "Start QueryProductDetails" + productIds.toString());
        List<QueryProductDetailsParams.Product> products = new ArrayList<>();
        for (String id : productIds) {
            products.add(QueryProductDetailsParams.Product.newBuilder()
                    .setProductId(id)
                    .setProductType(BillingClient.ProductType.INAPP)
                    .build());
        }

        billingClient.queryProductDetailsAsync(
                QueryProductDetailsParams.newBuilder().setProductList(products).build(),
                (billingResult, productDetailsList) -> {
                    Log.i(TAG, "OnResponse QueryProductDetails billingResult " + billingResult.toString() + " details:" + productDetailsList.size());
                    for (int i = 0; i < productDetailsList.size(); i++) {
                        Log.i(TAG, "OnResponse QueryProductDetails  [i]" + productDetailsList.get(i).toString());
                    }
                    if (billingResult.getResponseCode() == BillingClient.BillingResponseCode.OK) {
                    }
                }
        );
    }



    // 启动购买流程 https://developer.android.com/google/play/billing/integrate?hl=zh-cn#launch
    public void StartPay(String selectedOfferToken){
        Log.i(TAG, "StartPay" + selectedOfferToken.toString());
        ProductDetails productDetails = null;
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
                        .setProductType(ProductType.INAPP)
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
