using System.Collections.Generic;

namespace SDK.Google.PlayIntegrity
{
    public class IntegrityTokenPayload
    {
        public TokenPayloadExternal tokenPayloadExternal { get; set; }
    }

    public class TokenPayloadExternal
    {
        public RequestDetails requestDetails { get; set; }
        public AppIntegrity appIntegrity { get; set; }
        public DeviceIntegrity deviceIntegrity { get; set; }
        public AccountDetails accountDetails { get; set; }
        public TestingDetails testingDetails { get; set; }
        public EnvironmentDetails environmentDetails { get; set; }
    }

    public class RequestDetails
    {
        public string requestPackageName { get; set; }
        public string timestampMillis { get; set; }
        public string nonce { get; set; }
    }

    public class AppIntegrity
    {
        public string appRecognitionVerdict { get; set; }
        public string packageName { get; set; }
        public List<string> certificateSha256Digest { get; set; }
        public string versionCode { get; set; }
    }

    public class DeviceIntegrity
    {
        public RecentDeviceActivity recentDeviceActivity { get; set; }
        public DeviceAttributes deviceAttributes { get; set; }
    }

    public class RecentDeviceActivity
    {
        public string deviceActivityLevel { get; set; }
    }

    public class DeviceAttributes
    {
        // 根据实际情况添加属性
    }

    public class AccountDetails
    {
        public string appLicensingVerdict { get; set; }
    }

    public class TestingDetails
    {
        public bool isTestingResponse { get; set; }
    }

    public class EnvironmentDetails
    {
        public string playProtectVerdict { get; set; }
        public AppAccessRiskVerdict appAccessRiskVerdict { get; set; }
    }

    public class AppAccessRiskVerdict
    {
        // 根据实际情况添加属性
    }

    public static class IntegrityConstants
    {
        // App Recognition Verdicts
        public const string APP_RECOGNITION_VERDICT_PLAY_RECOGNIZED = "PLAY_RECOGNIZED";
        public const string APP_RECOGNITION_VERDICT_UNRECOGNIZED_VERSION = "UNRECOGNIZED_VERSION";
        public const string APP_RECOGNITION_VERDICT_UNRECOGNIZED = "UNRECOGNIZED";

        // Device Activity Levels
        public const string DEVICE_ACTIVITY_LEVEL_UNEVALUATED = "UNEVALUATED";
        public const string DEVICE_ACTIVITY_LEVEL_UNKNOWN = "UNKNOWN";
        public const string DEVICE_ACTIVITY_LEVEL_INACTIVE = "INACTIVE";
        public const string DEVICE_ACTIVITY_LEVEL_ACTIVE = "ACTIVE";

        // App Licensing Verdicts
        public const string APP_LICENSING_VERDICT_LICENSED = "LICENSED";
        public const string APP_LICENSING_VERDICT_UNLICENSED = "UNLICENSED";

        // Play Protect Verdicts
        public const string PLAY_PROTECT_VERDICT_UNEVALUATED = "UNEVALUATED";
        public const string PLAY_PROTECT_VERDICT_UNKNOWN = "UNKNOWN";
        public const string PLAY_PROTECT_VERDICT_PROTECTED = "PROTECTED";
        public const string PLAY_PROTECT_VERDICT_UNPROTECTED = "UNPROTECTED";
    }
}
