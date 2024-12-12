using System;
using System.Net;
using BestHTTP;
using Data;
using Data.DB;
using Framework.Log;
using Game.Billing;
using Newtonsoft.Json.Linq;
using ShadowGroveGames.SimpleHttpAndRestServer.Scripts;
using ShadowGroveGames.SimpleHttpAndRestServer.Scripts.Server;
using ShadowGroveGames.SimpleHttpAndRestServer.Scripts.Server.Extensions;
using UnityEngine;

namespace Game
{
    public class PayController : MonoBehaviour
    {
        private void Start()
        {
            LoggerEx.Debug("PayController", "PayController Start ");
        }

        
        [SimpleEventServerRouting(HttpConstants.MethodPost, "/pay/order")]
        public void C2SPayOrder(HttpListenerContext context)
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
        
        
        
        // https://androidpublisher.googleapis.com/androidpublisher/v3/applications/{packageName}/purchases/subscriptions/{subscriptionId}/tokens/{token}:acknowledge
        // packageName	 string 为其购买订阅的应用程序的软件包名称（例如“com.some.thing”）。
        // subscriptionId	 string  购买的订阅 ID（例如“monthly001”）。
        // token	 string   购买订阅时向用户设备提供的令牌。
        
        
        [SimpleEventServerRouting(HttpConstants.MethodGet, "/pay/conformOrder")]
        public void C2SPayConformOrder(HttpListenerContext context)
        {
            var receiptJson = context.Request.GetStringBody();
            
            // 解析外层 JSON
            var receiptData = ReceiptParse.Parse(receiptJson);

            var payload = receiptData.Payload;
            var purchaseInfo = payload.PurchaseInfo;
            
            var packageName = "com.KnockStudio.KnockHeroes";
            var productId = purchaseInfo.ProductId;  // productId
            var token = purchaseInfo.PurchaseToken;
            var url = "https://androidpublisher.googleapis.com/androidpublisher/v3/applications";
            url = $"{url}/{packageName}/purchases/subscriptions/{productId}/tokens/{token}";
            var uri = new Uri(url);
            var request = new HTTPRequest(uri, HTTPMethods.Post);
            request.RawData = new byte[10];
            request.Callback = (originalRequest, response) =>
            {
                LoggerEx.Debug("C2SPayConformOrder", $"acknowledge:{response.IsSuccess} {response.StatusCode} {response.DataAsText}");
            };
            request.Send();

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
    }
}