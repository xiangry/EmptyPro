using System;
using System.Net;
using BestHTTP;
using Data;
using Data.DB;
using Framework.Log;
using Newtonsoft.Json.Linq;
using ShadowGroveGames.SimpleHttpAndRestServer.Scripts;
using ShadowGroveGames.SimpleHttpAndRestServer.Scripts.Server;
using ShadowGroveGames.SimpleHttpAndRestServer.Scripts.Server.Extensions;
using UnityEngine;

namespace Game
{
    public class PayController : MonoBehaviour
    {
        [SimpleEventServerRouting(HttpConstants.MethodGet, "/pay/order")]
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
            var packageName = "com.KnockStudio.KnockHeroes";
            var subscriptionId = "google_product_6";  // productId
            var token = "token";
            var url = "https://androidpublisher.googleapis.com/androidpublisher/v3/applications";
            url = $"{url}/{packageName}/purchases/subscriptions/{subscriptionId}/tokens/{token}";
            var uri = new Uri(url);
            var request = new HTTPRequest(uri, HTTPMethods.Post);
            request.RawData = new byte[10];
            request.Callback = (originalRequest, response) =>
            {
                LoggerEx.Debug($"acknowledge:{response.IsSuccess} {response.StatusCode} {response.DataAsText}");
            };
            request.Send();
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
        
    }
}