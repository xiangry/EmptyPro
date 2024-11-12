using UnityEditor;
using UnityEngine;

public class BuildPlayer : MonoBehaviour
{
    [MenuItem("Build/Build Android")]
    public static void MyBuild()
    {
        PlayerSettings.Android.keystoreName = @"***";
        PlayerSettings.Android.keystorePass = "***";
        PlayerSettings.Android.keyaliasName = "***";
        PlayerSettings.Android.keyaliasPass = "***";

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/MyScene.unity" };
        buildPlayerOptions.locationPathName = "Build/Build14.apk";
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.None;
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}
