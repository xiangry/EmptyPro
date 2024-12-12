using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Framework.Log;
using Newtonsoft.Json;

namespace Game.Billing
{
    class GooglePlayBilling
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        private const string GoogleApiBaseUrl =
            "https://androidpublisher.googleapis.com/androidpublisher/v3/applications/";

        private const string PackageName = "com.KnockStudio.KnockHeroes"; // 替换为你的包名
        private const string AccessToken = "ya29.a0AeDClZDjUME84T7zVYKNLF1xoXtFyGOzJwvoY8JraPaiDlx2ZnvZKNFVx4sjdhdIO5-FuqpZlRDus1Mxd2uN4l8ByqHstzsQRIjcW9W5s0OUfMHLv35wnCrnTXgP4AAumOHVGeL646mGkEeFrI-O1LeRsH7JE28piSAjwF5taCgYKATESARESFQHGX2Mia57wkrKNKvbVOjXHBNymew0175"; // 替换为你的OAuth访问令牌

        private const string TAG = "GoogleBilling";
        
        // 验证购买
        public static async Task<bool> ValidatePurchase(string purchaseToken, string productId)
        {
            string url = $"{GoogleApiBaseUrl}{PackageName}/purchases/products/{productId}/tokens/{purchaseToken}";

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var data = responseBody;
                var x = 1;
                x++;
                return false;
                // var result = JsonConvert.DeserializeObject<dynamic>(responseBody);
                // bool isValid = result?.purchaseState == 0; // purchaseState == 0 表示订单合法
                // return isValid;
            }
            else
            {
                LoggerEx.Debug(TAG, $"验证失败: {responseBody}");
                return false;
            }
        }

        // 确认订单
        public static async Task<bool> AcknowledgePurchase(string purchaseToken)
        {
            string url = $"{GoogleApiBaseUrl}{PackageName}/purchases/products/{purchaseToken}:acknowledge";

            var content = new StringContent(JsonConvert.SerializeObject(new { }), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);

            HttpResponseMessage response = await _httpClient.PostAsync(url, content);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                LoggerEx.Debug(TAG, "确认成功");
                return true;
            }
            else
            {
                LoggerEx.Debug(TAG, $"确认失败: {responseBody}");
                return false;
            }
        }

        // 消耗订单
        public static async Task<bool> ConsumePurchase(string purchaseToken, string productId)
        {
            string url =
                $"{GoogleApiBaseUrl}{PackageName}/purchases/products/{productId}/tokens/{purchaseToken}:consume";

            var content = new StringContent(JsonConvert.SerializeObject(new { }), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);

            HttpResponseMessage response = await _httpClient.PostAsync(url, content);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                LoggerEx.Debug(TAG, "消耗成功");
                return true;
            }
            else
            {
                LoggerEx.Debug(TAG, $"消耗失败: {responseBody}");
                return false;
            }
        }

        // 示例流程
        public static async Task ProcessPurchase(string purchaseToken, string productId)
        {
            bool isValid = await ValidatePurchase(purchaseToken, productId);
            if (!isValid)
            {
                LoggerEx.Debug(TAG, "订单验证失败");
                return;
            }

            bool isAcknowledged = await AcknowledgePurchase(purchaseToken);
            if (!isAcknowledged)
            {
                LoggerEx.Debug(TAG, "确认订单失败");
                return;
            }

            bool isConsumed = await ConsumePurchase(purchaseToken, productId);
            if (!isConsumed)
            {
                LoggerEx.Debug(TAG, "消耗订单失败");
                return;
            }

            LoggerEx.Debug(TAG, "订单处理完成");
        }
    }
}


