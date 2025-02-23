using System;
using System.Net;
using System.Text;
using BestHTTP;
using Data;
using Data.DB;
using Framework.Log;
using Game.Billing;
using MongoDB.Bson.IO;
using Newtonsoft.Json.Linq;
using ShadowGroveGames.SimpleHttpAndRestServer.Scripts;
using ShadowGroveGames.SimpleHttpAndRestServer.Scripts.Server;
using ShadowGroveGames.SimpleHttpAndRestServer.Scripts.Server.Extensions;
using UnityEngine;
using Newtonsoft.Json;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace Game
{
    public class TestStates : MonoBehaviour
    {
        private void Start()
        {
            var url = "https://usewebhook.com/f2c9081f5a89c9421fae895495528c81";
            var uri = new Uri(url);
            var request = new HTTPRequest(uri);
            request.Callback += (originalRequest, response) =>
            {

                Debug.Log($"test webhook:{response.IsSuccess}|{response.StatusCode}|{response.DataAsText}");
            };
            request.Send();
        }


        [SimpleEventServerRouting(HttpConstants.MethodGet, "/test/dashboard")]
        public void DashboardStates(HttpListenerContext context)
        {
            var inputParams = InputUrlUtils.ParseGetUrlParams(context.Request.Url.ToString());
            var jObj = new JObject()
            {
                new JProperty("launcher", true),
                new JProperty("db", $"{DBManager.Instance}"),
            };
            foreach (var kv in inputParams)
            {
                jObj.Add(new JProperty(kv.Key, kv.Value));
            }
            context.Response.JsonResponse(jObj);
        }
        
        [SimpleEventServerRouting(HttpConstants.MethodPost, "/log")]
        public void LogData(HttpListenerContext context)
        {
            var data = Convert.FromBase64String(context.Request.GetStringBody());
            var str = Encoding.UTF8.GetString(data);
            LoggerEx.Debug($"[ClientLog]:{str}");
            
            var list = str.Split("|");
            for (int i = 0; i < list.Length; i++)
            {
                if(!list[i].StartsWith("receipt:"))
                    continue;
                
                // LoggerEx.Debug("PRODUCT",$"   ProcessPurchase {i}:{list[i]}");
                //
                // var receiptJson = list[i].Replace("receipt:", "");
                // receiptJson = receiptJson.TrimStart('"');
                // receiptJson = receiptJson.TrimEnd('"');
                // DumpProductInfo(receiptJson);
            }
        }

        private void DumpProductInfo(string receiptJson)
        {
            // 解析外层 JSON
            var receiptData = ReceiptParse.Parse(receiptJson);

            var payload = receiptData.Payload;
            var purchaseInfo = payload.PurchaseInfo;
            // 输出结果
            LoggerEx.Debug("PRODUCT", "PurchaseToken: " + purchaseInfo.PurchaseToken);
            LoggerEx.Debug("PRODUCT", "Acknowledged: " + purchaseInfo.Acknowledged);
            LoggerEx.Debug("PRODUCT", "PurchaseState: " + purchaseInfo.PurchaseState);

            // 解析 skuDetails (示例只处理第一个 SKU)
            var skuDetails = JsonConvert.DeserializeObject<SkuDetails>(payload.SkuDetails[0]);
            LoggerEx.Debug("PRODUCT", "SKU Title: " + skuDetails.Title);
            LoggerEx.Debug("PRODUCT", "SKU Price: " + skuDetails.Price);
        }
        
        
        
        [SimpleEventServerRouting(HttpConstants.MethodPost, "/conformOrder")]
        public void C2SPayConformOrder(HttpListenerContext context)
        {
            var data = Convert.FromBase64String(context.Request.GetStringBody());
            var receiptJson = Encoding.UTF8.GetString(data);
            
            // 解析外层 JSON
            var receiptData = ReceiptParse.Parse(receiptJson);

            var payload = receiptData.Payload;
            var purchaseInfo = payload.PurchaseInfo;
            
            // var packageName = "com.KnockStudio.KnockHeroes";
            // var productId = purchaseInfo.ProductId;  // productId
            // var token = purchaseInfo.PurchaseToken;
            // var url = "https://androidpublisher.googleapis.com/androidpublisher/v3/applications";
            // url = $"{url}/{packageName}/purchases/subscriptions/{productId}/tokens/{token}";
            // var uri = new Uri(url);
            // var request = new HTTPRequest(uri, HTTPMethods.Post);
            // request.RawData = new byte[10];
            // request.Callback = (originalRequest, response) =>
            // {
            //     LoggerEx.Debug("C2SPayConformOrder", $"acknowledge:{response.IsSuccess} {response.StatusCode} {response.DataAsText}");
            // };
            // request.Send();

            DoValidate(receiptData);
            //
            //
            // var inputParams = InputUrlUtils.ParseGetUrlParams(context.Request.Url.ToString());
            // var jObj = new JObject()
            // {
            //     new JProperty("launcher", true),
            //     new JProperty("db", $"{DBManager.Instance}"),
            // };
            // foreach (var kv in inputParams)
            // {
            //     jObj.Add(new JProperty(kv.Key, kv.Value));
            // }
            // context.Response.JsonResponse(jObj);
        }

        private async void DoValidate(ReceiptData receiptData)
        {
            var payload = receiptData.Payload;
            var purchaseInfo = payload.PurchaseInfo;
            var productId = purchaseInfo.ProductId;  // productId
            var token = purchaseInfo.PurchaseToken;
            await GooglePlayBilling.ValidatePurchase(token, productId);
        }
        

        [SimpleEventServerRouting(HttpConstants.MethodPost, "/conformOrder")]
        public void ParseToken(HttpListenerContext context)
        {
            var data = Convert.FromBase64String(context.Request.GetStringBody());
            var tokenData = Encoding.UTF8.GetString(data);
        }
    }
}

