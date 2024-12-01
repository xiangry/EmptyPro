using System.Collections;
using System.Collections.Generic;
using System.Net;
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
            LoggerEx.Debug($"ReceiveLog:{context.Request.GetStringBody()}");
        }
        
    }
}

