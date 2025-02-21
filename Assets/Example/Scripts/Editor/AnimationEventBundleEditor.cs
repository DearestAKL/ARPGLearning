using Akari.GfUnityEditor.AnimationEventBundler;
using UnityEditor;
using UnityEngine;

namespace GameMain.Editor
{
    [CustomEditor(typeof(AnimationEventBundle))]
    public class AnimationEventBundleEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Label("把fbx下的animation单独导出", EditorStyles.boldLabel);
            
            if (GUILayout.Button("导出AnimationClip"))
            {
                Export();
            }

            if (GUILayout.Button("替换为的导出AnimationClip"))
            {
                Replace();
            }
            
            if (GUILayout.Button("手动序列化"))
            {
                Serialize();
            }
        }
        
        private void Export()
        {
            AnimationEventBundle animationEventBundle = target as AnimationEventBundle;
            
            var originalClip = animationEventBundle.TargetAnimationEventClip;
            if (originalClip == null)
            {
                return;
            }

            // 获取AnimationClip的路径
            string path = AssetDatabase.GetAssetPath(animationEventBundle);
            // 获取文件夹路径
            string folderPath = System.IO.Path.GetDirectoryName(path);
            // 获取文件夹名
            string folderName = System.IO.Path.GetFileName(folderPath);
                
            string exportPath = EditorUtility.SaveFilePanel("Export Animation Clip", $"Assets/Example/GameRes/Character/{folderName}/AnimationClip", originalClip.name, "anim");
               
            if (!string.IsNullOrEmpty(exportPath))
            {
                string assetPath = "Assets" + exportPath.Substring(Application.dataPath.Length);
                    
                AnimationClip copiedClip = new AnimationClip();
                EditorUtility.CopySerialized(originalClip, copiedClip);
                AssetDatabase.CreateAsset(copiedClip, assetPath);
                AssetDatabase.SaveAssets();
                Debug.Log("Animation Clip copied and exported successfully at: " + assetPath);
            }
        }

        private void Replace()
        {
            AnimationEventBundle animationEventBundle = target as AnimationEventBundle;
            
            var originalClip = animationEventBundle.TargetAnimationEventClip;
            if (originalClip == null)
            {
                return;
            }
                
            // 获取AnimationClip的路径
            string path = AssetDatabase.GetAssetPath(animationEventBundle);
            // 获取文件夹路径
            string folderPath = System.IO.Path.GetDirectoryName(path);
            // 获取文件夹名
            string folderName = System.IO.Path.GetFileName(folderPath);

            var animationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(
                $"Assets/Example/GameRes/Character/{folderName}/AnimationClip/{originalClip.name}.anim");
            if (animationClip != null)
            {
                animationEventBundle.TargetAnimationEventClip = animationClip;
                EditorUtility.SetDirty(target);
            }
            else
            {
                Debug.LogWarning("请先导出");
            }
        }

        private void Serialize()
        {
            GfAnimationEventSerializer.Serialize(target as AnimationEventBundle);
        }
    }
}