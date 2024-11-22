#if UNITY_EDITOR
namespace ShadowGroveGames.SimpleWebSocketServer.Editor.Setup
{
    internal static class Asset
    {
        internal const string KEY = "SimpleWebSocketServer";
        internal const string NAME = "Simple WebSocket Server";
        internal const string LOGO = "simple-websocket-server-banner";
        internal const string REVIEW_URL = "https://assetstore.unity.com/packages/slug/260199?utm_source=editor#reviews";
        internal const string README_GUID = "b58b8e69cfc30a74fab2f0480d1c3f4a";

        internal readonly static string[] DONT_SHOW_IF_ASSABMLY_LOADED = new string[]
        {
            "org.Shadow-Grove.SimpleServerComplete.Editor",
        };

        // Review
        internal const int REVIEW_MIN_OPENINGS = 2;
        internal const int REVIEW_MIN_DAYS = 10;

        // Editor Prefs
        internal const string EDITOR_PREFS_KEY_GETTING_STARTED = KEY + "-GettingStarted";
        internal const string EDITOR_PREFS_KEY_REVIEW_DISABLE_REMINDER = KEY + "-ReviewReminder";
        internal const string EDITOR_PREFS_KEY_REVIEW_EDITOR_OPEN_COUNT = KEY + "-ReviewEditorOpenCount";
        internal const string EDITOR_PREFS_KEY_REVIEW_INIT_DATE = KEY + "-ReviewInitDate";
    }
}
#endif