using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class AABBuilder
    {
        [MenuItem("Build/AAB")]
        public static void BuildAAB()
        {
            // 获取所有场景
            List<string> scenes = new List<string>();
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    scenes.Add(scene.path);
                }
            }

            // 设置打包选项
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = scenes.ToArray();
            buildPlayerOptions.locationPathName = $"{Application.dataPath}/../../app.aab"; // 替换为实际保存路径
            buildPlayerOptions.target = BuildTarget.Android;
            buildPlayerOptions.options = BuildOptions.None;
            buildPlayerOptions.targetGroup = BuildTargetGroup.Android;

            // 进行打包
            BuildPipeline.BuildPlayer(buildPlayerOptions);
        }
    }
}