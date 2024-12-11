using System;
using System.Net;
using System.Text;
using Data;
using Data.DB;
using Framework.Log;
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
            LoggerEx.Debug($"ReceiveLog:{str}");
            
            var list = str.Split("|");
            for (int i = 0; i < list.Length; i++)
            {
                if(!list[i].StartsWith("receipt:"))
                    continue;
                
                LoggerEx.Debug($"   ProcessPurchase {i}:{list[i]}");

                var receiptJson = list[i].Replace("receipt:", "");
                receiptJson = receiptJson.TrimStart('"');
                receiptJson = receiptJson.TrimEnd('"');
                DumpProductInfo(receiptJson);
            }
        }

        private void DumpProductInfo(string receiptJson)
        {
            // 解析外层 JSON
            var receipt = JsonConvert.DeserializeObject<Receipt>(receiptJson);

            var payload = JsonConvert.DeserializeObject<Payload>(receipt.Payload);
            
            // 解析嵌套的 Payload.Json 字段
            var payloadJson = JsonConvert.DeserializeObject<PurchaseInfo>(payload.Json);

            // 输出结果
            LoggerEx.Debug("PRODUCT", "PurchaseToken: " + payloadJson.PurchaseToken);
            LoggerEx.Debug("PRODUCT", "Acknowledged: " + payloadJson.Acknowledged);
            LoggerEx.Debug("PRODUCT", "PurchaseState: " + payloadJson.PurchaseState);

            // 解析 skuDetails (示例只处理第一个 SKU)
            var skuDetails = JsonConvert.DeserializeObject<SkuDetails>(payload.SkuDetails[0]);
            LoggerEx.Debug("PRODUCT", "SKU Title: " + skuDetails.Title);
            LoggerEx.Debug("PRODUCT", "SKU Price: " + skuDetails.Price);
        }
        
//         [3564.871]:ReceiveLog:[Info][613.9085]:[PurchasingManager]:ProcessPurchase: - Productdefinition:[google_product_12-enabled:True-Consumable-google_product_12]|metadata:[google_product_12 (KnockHeroes)-$65.00-65-]|receipt:{"Payload":"{\"json\":\"{\\\"orderId\\\":\\\"GPA.3353-4426-7909-27101\\\",\\\"packageName\\\":\\\"com.KnockStudio.KnockHeroes\\\",\\\"productId\\\":\\\"google_product_12\\\",\\\"purchaseTime\\\":1733844146194,\\\"purchaseState\\\":0,\\\"purchaseToken\\\":\\\"pabgcinfblfbfohgddeipaek.AO-J1OxtaPcgT-hhwGn3K6EiLslYdDXcGsp41xl8QIZjjcYZoZrIFHP6dq-Rny3CReRRxAFJr85YhgGEiiBjU6W-4GuYUIci312iDsbTGpWi74aaDLG6210\\\",\\\"quantity\\\":1,\\\"acknowledged\\\":false}\",\"signature\":\"h7nEmFe3XYq9WHfvJOoUu/pXNt6O1QVuxEdXcHTSK/wTCbZG7nt8rVF2Zzpkt5tnZOOxfgD9UFJCQLfUJqJHZbgjJrVl49M6IoOc7YQECUh2ujdMUc1/7uZXfJbcNtlbls1ABIS2b0B4jWJXpAk0QzkZp8gNebTPSiTQzyo9AoUD4dmVOlZlU1owUfXuW0F6+L7oEEqHUdqd+FCnfWzm8msZMqPM/tymLJYd1Ys9D7FLkOZo+XmDKZOWg1dD8rWmVZk7sVYKtQGWeEtM0fryDj/kNLhxZoODE+SIb3C91GNSXnbD8hFcnbPDKtKNbMQ/1u7PZGmr/fmk8JdO5Xw7zA==\",\"skuDetails\":[\"{\\\"productId\\\":\\\"google_product_12\\\",\\\"type\\\":\\\"inapp\\\",\\\"title\\\":\\\"google_product_12 (KnockHeroes)\\\",\\\"name\\\":\\\"google_product_12\\\",\\\"iconUrl\\\":\\\"https:\\\\/\\\\/lh3.googleusercontent.com\\\\/CGmmnQ12Ufe31j8F88td7Gnnc9B89km0t8P0YB5xHVEqPMiKjdMELyybP6lAWBM_kLkO\\\",\\\"description\\\":\\\"google_product_12\\\",\\\"price\\\":\\\"$65.00\\\",\\\"price_amount_micros\\\":65000000,\\\"price_currency_code\\\":\\\"TWD\\\",\\\"skuDetailsToken\\\":\\\"AEuhp4Lg7v6C-WRzJLsAH-AvaBg1biIm3DGBF2Ywng0ukbcTpwvS_5-PUBYdzHejEsPqVPCmYzFafBFuz0FZLZhaALe_1n5oduZVd3X6f10FMumpQls0\\\"}\"]}","Store":"GooglePlay","TransactionID":"pabgcinfblfbfohgddeipaek.AO-J1OxtaPcgT-hhwGn3K6EiLslYdDXcGsp41xl8QIZjjcYZoZrIFHP6dq-Rny3CReRRxAFJr85YhgGEiiBjU6W-4GuYUIci312iDsbTGpWi74aaDLG6210"}|availableToPurchase:True|transactionID:pabgcinfblfbfohgddeipaek.AO-J1OxtaPcgT-hhwGn3K6EiLslYdDXcGsp41xl8QIZjjcYZoZrIFHP6dq-Rny3CReRRxAFJr85YhgGEiiBjU6W-4GuYUIci312iDsbTGpWi74aaDLG6210|hasReceipt:True
// UnityEngine.Debug:Log (object)
// Framework.Log.LoggerEx:Debug (string) (at Assets/Scripts/Framework/Log/Logger.cs:9)
// Game.TestStates:LogData (System.Net.HttpListenerContext) (at Assets/Scripts/Game/TestStates.cs:41)
// System.Reflection.MethodBase:Invoke (object,object[])
// ShadowGroveGames.SimpleHttpAndRestServer.Scripts.SimpleEventServerScript:ProcessReceivedRequest (System.Net.HttpListenerContext) (at Assets/ShadowGroveGames/Simple HTTP and REST Server/Scripts/SimpleEventServerScript.cs:109)
// ShadowGroveGames.SimpleHttpAndRestServer.Scripts.AbstractServerScript:FixedUpdate () (at Assets/ShadowGroveGames/Simple HTTP and REST Server/Scripts/AbstractServerScript.cs:78)

        
    }
}

