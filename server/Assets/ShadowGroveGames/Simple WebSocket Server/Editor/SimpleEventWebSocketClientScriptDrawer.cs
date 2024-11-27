using ShadowGroveGames.SimpleWebSocketServer.Scripts;
using UnityEditor;
using UnityEngine;

namespace ShadowGroveGames.SimpleWebSocketServer.Editor
{
    [CustomEditor(typeof(SimpleEventWebSocketClientScript))]
    public class SimpleEventWebSocketClientScriptDrawer : UnityEditor.Editor
    {
        GUIStyle _labelWordWrapStyle;
        GUIStyle _smallLabelWordWrapStyle;

        public override void OnInspectorGUI()
        {
            PrepareGUIStyles();
            DrawHeaderImage();
            DrawNote();
            base.OnInspectorGUI();
        }

        void PrepareGUIStyles()
        {
            _labelWordWrapStyle = new GUIStyle(EditorStyles.boldLabel);
            _labelWordWrapStyle.wordWrap = true;
            _smallLabelWordWrapStyle = new GUIStyle(EditorStyles.label);
            _smallLabelWordWrapStyle.wordWrap = true;
        }

        void DrawHeaderImage()
        {
            Texture2D headerImage = (Texture2D)Resources.Load("simple-websocket-client-banner", typeof(Texture2D));
            float width = headerImage.width * 0.75f;
            float height = headerImage.height * 0.75f;

            GUI.DrawTexture(new Rect(15, 10, width, height), headerImage, ScaleMode.ScaleToFit, true, width / height);
            EditorGUILayout.Space(height + 10);
        }

        void DrawNote()
        {
#if (UNITY_2021_1_OR_NEWER)
            EditorGUILayout.BeginHorizontal();
            if (EditorGUILayout.LinkButton("For support you can join our ShadowGroveGames Discord"))
                Application.OpenURL("https://discord.shadow-grove.org/");
            EditorGUILayout.EndHorizontal();
#else
            EditorGUILayout.LabelField("For support you can join our ShadowGroveGames Discord:", _smallLabelWordWrapStyle);
            EditorGUILayout.LabelField("https://discord.shadow-grove.org/", _labelWordWrapStyle);
#endif
            EditorGUILayout.Space();
        }
    }
}
