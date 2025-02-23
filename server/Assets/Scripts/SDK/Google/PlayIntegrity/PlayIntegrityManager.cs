
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Framework.Base;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using UnityEngine;


namespace SDK.Google.PlayIntegrity
{
    public class PlayIntegrityManager : SingletonClass<PlayIntegrityManager>
    {
        private static bool _isAccessTokenAvailable = false;
        private static bool _isTokenDecodable = false;
        private static string _accessToken = string.Empty;

        public static bool IsAccessTokenAvailable => _isAccessTokenAvailable;
        public static bool IsTokenDecodable => _isTokenDecodable;

        private const string PACKAGE_NAME = "com.KnockStudio.KnockHeroes";

        public async Task Init()
        {
            try
            {
                Logger.Debug("开始获取 Google OAuth 2.0 Access Token...");
                _accessToken = await GetAccessToken();
                _isAccessTokenAvailable = true;
                Logger.Debug("成功获取 Access Token。");

                // // 示例：假设您有一个 Integrity Token
                // // string integrityToken = "eyJhbGciOiJBMjU2S1ciLCJlbmMiOiJBMjU2R0NNIn0.bmNQGJTraZyo-1FRa5NKpBwa4NsX0V93lxSKGBFSFh2Dz56bFSUVyA.nvD4ZXi2DM7hGAzT.2y3PUYXaeI-WVgcsRk6NP89qgMVbs2mrBO5mJoFqlGO9nuQnZZRF0FQC02S5MGSsTz5xVb9EJqcpsxz8xDHUpbO7uDfjjQyo3DBXZVa9_Q9Qf4L-h6xzhhMtKNN0B9EB34HlgF7a091x57ZOHUhfV5a9p8RVl51x61KUDCRbNdgPEc6tRECbpQW2Zi0-JrnqiB10CbH2PUE5ai58Vmjzvur5jOnzeJrWIUllBsvgjDMMBaS2uvucV5JHXeemjN0wxEgNCxi7xwakFfu_KX5bugOWSE-fGjwAz0DyGfYizbJ6Og1eNOMbwdtTgX850LZZJZl96ahlmLSTgufP5WlZdwzbDg5e65m9sj1GTR3Sq_Rx9hNiYFSTjSZ9OBfdQ79yk9hFEmnlGE-nh9okMdRUn_-qx8tA5nR9I6mmpVumINgNr6KL_UHfyIMZ8r1oDuITQm1tYsUKeoVMW3dhhLcCi0iF6-nOdQ1APe-mMZfo3VE7czCz55M2xtwZsMCVqUzrMO5uS0gsIFFNfADawLyLttVuxVdFzYwN1eDKIbKsWot05VqsZ5W2NdOE0CX-JOZxpaLYqNjO3hDjJDbfmAJ44SZmAXTRFzCC8yTBIHAFSIdPgAG6cWJq9HasENqmbpT4T-S1PI8p7KIlj-EEie2kDYIk3eWe5RsQYprCt8B6vAM9pCGUDEIkVGKclOv58dSQJukLmb9vknQnwNgFt7KvWSs938GhYCZ3L3eg2eBTyiOzqWsR-5d6PYC2jfrEG2BQcK8lEyJOYr9i4pJeK5C4U7LH1uQKql3REvS0-TtcfFWaSyQ6XZIcL9J7iZpFsvHgfbSQ3mlCiD_JwavhgecC4EEqjy6k4pi8i-WKTmIFrbv9MolP21Vh6qojY4zpDIK_-J7TVBtCFuuFFYP8GxqRJxAIShhvLZPMZZyizr2GM-553N21-sDpwj8JI7_KIG3rJphctGim0Xfw1R_cjDJMTKf-ZFD6sKLNDNo8NxpHeKGxxzsW_QT7t-boSrU3VnEZtsH4GPB6zuHA2uqabeLFQfM4swRcUzKwJi0IsUD9LYIE39BNRo30J_5r9OU7_bD_3HbwTG-FMcJ19sbBzP_kJjpBhnY9loWq1P7J4sopwoAwNZ8sUOW30zTc5vRwNAAvzwzNuYtv0GyN4ogvD_tsEdsTEHqhI9XzoTQsSX82qluWOXhb3NtR_QaveZD-KHLOh5TuSU193pXhbe7B1wFyKJeTlfR_VdqOyIRO3b5a2iYKBdijrZzP5EaQpVkRhEoqfQ-VSR-5HVkT9Xw5FPBHWrnUlRIdc9QXrbw_lZbzIPzwcv2btvN7WFU61t5C-7zbCX5jPhoO16aYPXO0gaJRu05lqVpcilwNrxCZN2DCPB9AR-Eeg8yIGT1wKWYgfSDj7fAsOqgzXC5iKw9U9KcVac1jirjuC2TpwP2E21xznWnlht1USb307HxTLb_ICynnUuYjUjppEECfU77Dha5fSdeoh2IAqQB4AinOt5cPUaxLzp5cAO8VhrLWD0Z2aUf3u39Gm8AHVph7_Ih_Qy402g.4i4u5dI24iw1etCLDk5Ugg";
                // // string integrityToken =
                // //     "Cr4CARCnMGtGYKBRJrEGMtyWOjxUSQPHp0VPHBK5l9ErAbl-aE_cXVytVh-gLKgGRnWIkkAW_xkNjcRae10XOb5TB6SL6BM4TLTTtMG24Clrdtp9HFZmt0EGnAbNC1Y6l6otajbR6Kq0NPgY8JhwmopNNoZ-VpifwNdS7d8j53K-eYHvM8Q6pzVzNXNkUVU9j8guWn1G2QVKE6llWxVJqf34T1iE_K-cldVQbSes2KJLKHiUyFZkCX3hM1G3WKnqwayO11c855cBbcPDcMnLxv-pSoquc7gKRBnx7-3H6W95rzJ4pQQenK1DM6C2J8CWepCiC0hAal9vch5KW8vn6ciR0BNnBFVgTfQL1zcYIyJnD4bfhWKvKtCKLTD2xB25GXLonuP14bE5xl_19Frz-QzP2i5RcGH_UWX6ZDfsvik-GncBIEirfTicsyceCGC2mV6idA5gBHD4c38aOGeFHqhXvzFaWvqm2z8SIuBsbzPHczi0cVbmixX2BxHffvWoVCNJKVMHfoGDDfoES5xWjqBgLnxn7R83e-kvrXEzZShqD0KQRY7w9wTB4RgKW6EphMrNwUHyv8wUPQ";
                // string integrityToken = "CsECARCnMGvBQ8QdSm8unsLGyYRdVpOXgTpx5aEyZeFhkrYOMEMDp2dzr1cdcpaz50etEtwlW-6VM1wi9vuOaS_lrxS2HP46r_L1qMEcOPcloY8XOFMNg79y3QnV7BrMtJNojtCmRSciUzxvCCH5SlPTPlHz0fVjfPUBg4qt2kJD--NKi2iA-zHoBIKE5Aq-bpnNrSSy-8hwpQjmhtLymZ6f8TPsBLEix6OFs_8IxDD73UbNu84j-0fHONK-Qzyezgs_SOZPapGlGF7PS6ELAvN9fDJTVwGtRH-ULx-M5as8p_P_PiqJTW7pbSYNDSAYAl70iOChVLoKxy-KST2HHd22QVtb5J4Wtp5fPWED33v-PNhibTFMaR3LotndkwLZBM3-ZGCw5lSVFP1az7F_q794RzWTcmJ5NHzewCO_KSwYlOJdGnUBgyn6w5VlJeJXrW9mcq7SRIKj7bXT5Xt9Pl6O_nMB7ZJ96_4qOtmmE1bbouEo1v32pMKvoYSrQc8oqUC-tIIQQJQ6y1vDmB0NLPyHEYRuNRaicia5AOxRk-3r4fDcIvHLn7RbNUPp6E6R6B7AKKjMpZ3p0gM";
                // Logger.Debug("开始验证 Integrity Token...");
                // _isTokenDecodable = await DecodeIntegrityToken(integrityToken);
                // Logger.Debug($"Integrity Token 验证结果: {(_isTokenDecodable ? "有效" : "无效")}");
            }
            catch (Exception ex)
            {
                Logger.Error($"发生错误: {ex.Message}");
            }
        }

        
        private async Task<string> GetAccessToken()
        {
            try
            {
                var textAsset = Resources.Load<TextAsset>("kncokheroes-91e3de2ac034");
                using (var memStream = new MemoryStream(textAsset.bytes))
                {
                    Logger.Debug("从服务账户文件获取凭证...");
                    var credential = await GoogleCredential.FromStream(memStream)
                        .CreateScoped("https://www.googleapis.com/auth/playintegrity")
                        .UnderlyingCredential.GetAccessTokenForRequestAsync();
                    Logger.Debug("成功获取凭证。");
                    return credential;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"获取 Access Token 时发生错误: {ex.Message}");
                throw;
            }
        }
        
        public async Task<bool> DecodeIntegrityToken(string integrityToken)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"https://playintegrity.googleapis.com/v1/{PACKAGE_NAME}:decodeIntegrityToken";
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);

                var requestBody = new
                {
                    integrity_token = integrityToken
                };
                string jsonBody = JsonConvert.SerializeObject(requestBody);
                HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                
                try
                {
                    Logger.Debug($"发送请求到 {url}");
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    string responseText = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        Logger.Error($"请求失败: {response.StatusCode}, 详情: {responseText}");
                        File.WriteAllText($"{Application.dataPath}/../../log.txt", responseText);
                        return false;
                    }

                    Logger.Debug($"请求成功：{responseText}");
                    
                    // 使用强类型类进行反序列化
                    var responseObject = JsonConvert.DeserializeObject<IntegrityTokenPayload>(responseText);
                    var isAppPassed = PlayIntegrityUtil.IsPackagePassed(responseObject);
                    var isDevicePassed = PlayIntegrityUtil.IsDevicePassed(responseObject);
                    var isAccountPassed = PlayIntegrityUtil.IsGoogleAccountPassed(responseObject);
                    var isEnvPassed = PlayIntegrityUtil.IsEnvPassed(responseObject);
                    var isTest = PlayIntegrityUtil.IsIntegrityTest(responseObject);
                    Logger.Debug($"integrity token result: isAppPassed:{isAppPassed} isDevicePassed:{isDevicePassed} isAccountPassed:{isAccountPassed} isEnvPassed:{isEnvPassed} isTest:{isTest}");
                    return isAppPassed && isAccountPassed;
                }
                catch (Exception ex)
                {
                    Logger.Error($"发送请求时发生错误: {ex.Message}");
                    throw;
                }
            }
        }


        public async Task ProcessIntegrityToken(string integrityToken)
        {
            if (!_isAccessTokenAvailable)
            {
                Logger.Debug("Access Token 未获取，开始初始化...");
                await Init();
            }

            if (_isAccessTokenAvailable)
            {
                Logger.Debug("Access Token 已获取，开始验证 Integrity Token...");
                bool isValid = await DecodeIntegrityToken(integrityToken);
                if (isValid)
                {
                    Logger.Debug("Integrity Token 验证有效。");
                    // 在此处处理有效的 Token
                }
                else
                {
                    Logger.Debug("Integrity Token 验证无效。");
                    // 在此处处理无效的 Token
                }
            }
            else
            {
                Logger.Error("Access Token 获取失败，无法验证 Integrity Token。");
                // 在此处处理获取 Access Token 失败的情况
            }
        }
    }
}


