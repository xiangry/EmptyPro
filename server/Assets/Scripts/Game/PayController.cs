using System.Net;
using Data;
using Data.DB;
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
        
        
    }
}