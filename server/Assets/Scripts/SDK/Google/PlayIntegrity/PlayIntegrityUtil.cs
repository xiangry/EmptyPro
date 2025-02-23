using System;
using Newtonsoft.Json;
using UnityEngine;

namespace SDK.Google.PlayIntegrity
{
    // token信息 1
    //     {
    //     "tokenPayloadExternal": {
    //         "requestDetails": {
    //             "requestPackageName": "com.KnockStudio.KnockHeroes",
    //             "timestampMillis": "1740314672893",
    //             "requestHash": "wosImrjbb1GPuiqcnBipEAa_wO-OZNhsQdPSHt36t5MKhrmiR4IYSh16"
    //         },
    //         "appIntegrity": {
    //             "appRecognitionVerdict": "PLAY_RECOGNIZED",
    //             "packageName": "com.KnockStudio.KnockHeroes",
    //             "certificateSha256Digest": [
    //             "K_GwJAixtf9bjXkScNuQpDVe9QyJ--sIkT5q3Xs3KiE"
    //                 ],
    //             "versionCode": "11"
    //         },
    //         "deviceIntegrity": {
    //             "recentDeviceActivity": {
    //                 "deviceActivityLevel": "UNEVALUATED"
    //             },
    //             "deviceAttributes": {}
    //         },
    //         "accountDetails": {
    //             "appLicensingVerdict": "LICENSED"
    //         },
    //         "testingDetails": {
    //             "isTestingResponse": true
    //         },
    //         "environmentDetails": {
    //             "playProtectVerdict": "UNEVALUATED",
    //             "appAccessRiskVerdict": {}
    //         }
    //     }
    // }
    
    // token 2
//     {
//     "requestDetails": {
//         "requestPackageName": "com.package.name",
//         "timestampMillis": "1617893780",
//         "nonce": "aGVsbG8gd29scmQgdGhlcmU"
//     },
//     "appIntegrity": {
//         "appRecognitionVerdict": "PLAY_RECOGNIZED",
//         "packageName": "com.package.name",
//         "certificateSha256Digest": [
//         "6a6a1474b5cbbb2b1aa57e0bc3"
//             ],
//         "versionCode": "42"
//     },
//     "deviceIntegrity": {
//         "deviceRecognitionVerdict": [
//         "MEETS_BASIC_INTEGRITY",
//         "MEETS_DEVICE_INTEGRITY",
//         "MEETS_STRONG_INTEGRITY",
//         "MEETS_VIRTUAL_INTEGRITY"
//             ],
//         "recentDeviceActivity": {
//             "deviceActivityLevel": "LEVEL_3"
//         },
//         "deviceAttributes": {
//             "sdkVersion": 32
//         }
//     },
//     "accountDetails": {
//         "appLicensingVerdict": "LICENSED"
//     },
//     "environmentDetails": {
//         "playProtectVerdict": "NO_ISSUES",
//         "appAccessRiskVerdict": {
//             "appsDetected": [
//             "KNOWN_INSTALLED",
//             "UNKNOWN_INSTALLED",
//             "UNKNOWN_CAPTURING"
//                 ]
//         }
//     }
// }

    public static class PlayIntegrityUtil
    {
        /// <summary>
        /// 应用完整性 判断是否是google官方下载的包
        /// appRecognitionVerdict：应用识别结果，可能的值包括：
        ///     PLAY_RECOGNIZED：应用被 Google Play 认可，且未被篡改。
        ///     UNRECOGNIZED_VERSION：应用未被识别，可能是未经授权的修改版本。
        ///     UNEVALUATED：未对应用进行评估。
        /// packageName：应用的包名。
        /// certificateSha256Digest：应用签名证书的 SHA-256 摘要列表。
        /// versionCode：应用的版本代码。
        /// </summary>
        /// <param name="integrityTokenPayload"></param>
        /// <returns></returns>
        public static bool IsPackagePassed(IntegrityTokenPayload integrityTokenPayload)
        {
            try
            {
                // 获取 appRecognitionVerdict 的值
                var appRecognitionVerdict =
                    integrityTokenPayload?.tokenPayloadExternal?.appIntegrity?.appRecognitionVerdict;

                // 检查 appRecognitionVerdict 是否为 "PLAY_RECOGNIZED"
                if (appRecognitionVerdict == "PLAY_RECOGNIZED")
                {
                    // 应用是从 Google Play 商店下载的官方版本，允许用户玩游戏
                    return true;
                }
                else
                {
                    // 应用不是官方版本，阻止用户玩游戏
                    return false;
                }
            }
            catch (JsonException ex)
            {
                // 处理 JSON 解析错误
                Debug.Log($"JSON 解析错误: {ex.Message}");
                return false;
            }

            catch (Exception ex)
            {
                // 处理其他异常
                Console.WriteLine($"发生错误: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 设备完整性
        /// deviceActivityLevel：设备活动级别，可能的值包括：
        ///     UNEVALUATED：未对设备活动进行评估。
        ///     UNKNOWN：设备活动状态未知。
        ///     PLAY_PROTECT_ACTIVE：设备的 Google Play Protect 处于活动状态。
        /// deviceAttributes：设备属性的相关信息。
        /// </summary>
        /// <param name="integrityTokenPayload"></param>
        /// <returns></returns>
        public static bool IsDevicePassed(IntegrityTokenPayload integrityTokenPayload)
        {
            try
            {
                // 获取 deviceActivityLevel 的值
                var deviceActivityLevel =
                    integrityTokenPayload?.tokenPayloadExternal?.deviceIntegrity?.recentDeviceActivity
                        ?.deviceActivityLevel;

                // 检查 deviceActivityLevel 是否为 "PLAY_PROTECT_ACTIVE"
                if (deviceActivityLevel == "PLAY_PROTECT_ACTIVE")
                {
                    // 备的 Google Play Protect 处于活动状态
                    return true;
                }
                else
                {
                    // 设备活动状态未知 或者 未对设备活动进行评估
                    return false;
                }
            }
            catch (JsonException ex)
            {
                // 处理 JSON 解析错误
                Debug.Log($"JSON 解析错误: {ex.Message}");
                return false;
            }

            catch (Exception ex)
            {
                // 处理其他异常
                Console.WriteLine($"发生错误: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 账户详情
        /// appLicensingVerdict：应用许可结果，可能的值包括：
        ///     LICENSED：用户已获得应用的合法许可。
        ///     UNLICENSED：用户未获得应用的合法许可。
        ///     UNEVALUATED：未对应用许可进行评估。
        /// </summary>
        /// <param name="integrityTokenPayload"></param>
        /// <returns></returns>
        public static bool IsGoogleAccountPassed(IntegrityTokenPayload integrityTokenPayload)
        {
            try
            {
                // 账户状态
                var appLicensingVerdict =
                    integrityTokenPayload?.tokenPayloadExternal?.accountDetails?.appLicensingVerdict;

                // 检查 deviceActivityLevel 是否为 "PLAY_PROTECT_ACTIVE"
                if (appLicensingVerdict == "LICENSED")
                {
                    // 用户已获得应用的合法许可。
                    return true;
                }
                else
                {
                    // 用户未获得应用的合法许可。/ 未对应用许可进行评估。
                    return false;
                }
            }
            catch (JsonException ex)
            {
                // 处理 JSON 解析错误
                Debug.Log($"JSON 解析错误: {ex.Message}");
                return false;
            }

            catch (Exception ex)
            {
                // 处理其他异常
                Console.WriteLine($"发生错误: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 测试详情
        /// appLicensingVerdict：应用许可结果，可能的值包括：
        ///     LICENSED：用户已获得应用的合法许可。
        ///     UNLICENSED：用户未获得应用的合法许可。
        ///     UNEVALUATED：未对应用许可进行评估。
        /// </summary>
        /// <param name="integrityTokenPayload"></param>
        /// <returns></returns>
        public static bool IsIntegrityTest(IntegrityTokenPayload integrityTokenPayload)
        {
            try
            {
                var isTestingResponse =
                    integrityTokenPayload?.tokenPayloadExternal?.testingDetails?.isTestingResponse;

                if (isTestingResponse == true)
                {
                    // test state。
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (JsonException ex)
            {
                // 处理 JSON 解析错误
                Debug.Log($"JSON 解析错误: {ex.Message}");
                return false;
            }

            catch (Exception ex)
            {
                // 处理其他异常
                Console.WriteLine($"发生错误: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 环境详情
        /// playProtectVerdict：Google Play Protect 的评估结果，可能的值包括：
        ///     UNEVALUATED：未对 Play Protect 状态进行评估。
        ///     PLAY_PROTECT_NOT_INSTALLED：设备上未安装 Play Protect。
        ///     PLAY_PROTECT_DISABLED：设备上的 Play Protect 被禁用。
        ///     PLAY_PROTECT_ENABLED：设备上的 Play Protect 处于启用状态。
        /// appAccessRiskVerdict：应用访问风险的评估结果。
        /// </summary>
        /// <param name="integrityTokenPayload"></param>
        /// <returns></returns>
        public static bool IsEnvPassed(IntegrityTokenPayload integrityTokenPayload)
        {
            try
            {
                var playProtectVerdict =
                    integrityTokenPayload?.tokenPayloadExternal?.environmentDetails?.playProtectVerdict;

                if (playProtectVerdict == "NO_ISSUES")
                {
                    // test state。
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (JsonException ex)
            {
                // 处理 JSON 解析错误
                Debug.Log($"JSON 解析错误: {ex.Message}");
                return false;
            }

            catch (Exception ex)
            {
                // 处理其他异常
                Console.WriteLine($"发生错误: {ex.Message}");
                return false;
            }
        }


    }
}