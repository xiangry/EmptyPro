using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Game
{
    public class ReceiptData
    {
        public PayloadData Payload { get; set; }

        public string Store { get; set; }

        public string TransactionId { get; set; }
    }

    public class PayloadData
    {
        public PurchaseInfo PurchaseInfo { get; set; }

        public string Signature { get; set; }

        public List<string> SkuDetails { get; set; }
    }
    

    public class SkuDetails
    {
        public string ProductId { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public long PriceAmountMicros { get; set; }
        public string PriceCurrencyCode { get; set; }
        public string SkuDetailsToken { get; set; }
    }

    public class PayloadBase
    {
        [JsonProperty("json")] public string Json { get; set; }

        [JsonProperty("signature")] public string Signature { get; set; }

        [JsonProperty("skuDetails")] public List<string> SkuDetails { get; set; }
    }

    public class ReceiptBase
    {
        [JsonProperty("Payload")] public string Payload { get; set; }

        [JsonProperty("Store")] public string Store { get; set; }

        [JsonProperty("TransactionID")] public string TransactionId { get; set; }
    }

    public class PurchaseInfo
    {
        [JsonProperty("orderId")] public string OrderId { get; set; }

        [JsonProperty("packageName")] public string PackageName { get; set; }

        [JsonProperty("productId")] public string ProductId { get; set; }

        [JsonProperty("purchaseTime")] public long PurchaseTime { get; set; }

        [JsonProperty("purchaseState")] public int PurchaseState { get; set; }

        [JsonProperty("purchaseToken")] public string PurchaseToken { get; set; }

        [JsonProperty("quantity")] public int Quantity { get; set; }

        [JsonProperty("acknowledged")] public bool Acknowledged { get; set; }
    }

    public static class ReceiptParse
    {
        public static ReceiptData Parse(string receiptJson)
        {
            // 解析外层 JSON
            var receipt = JsonConvert.DeserializeObject<ReceiptBase>(receiptJson);

            var payload = JsonConvert.DeserializeObject<PayloadBase>(receipt.Payload);
            
            // 解析嵌套的 Payload.Json 字段
            var purchasingInfo = JsonConvert.DeserializeObject<PurchaseInfo>(payload.Json);

            var payloadData = new PayloadData()
            {
                Signature = payload.Signature,
                SkuDetails = payload.SkuDetails,
                PurchaseInfo = purchasingInfo
            };

            var receiptData = new ReceiptData()
            {
                Store = receipt.Store,
                TransactionId = receipt.TransactionId,
                Payload = payloadData,
            };
            return receiptData;
        }
    }
}