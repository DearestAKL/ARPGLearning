using System;
using Akari.GfUnityEditor;
using UnityEditor;
using UnityEngine;

namespace TreeDesigner.Editor
{
    public static class BehaviourTreeMenu
    {
        [MenuItem("Tools/TreeDesigner/Build All Tree")] 
        public static void BuildAllBehaviourTree()
        {
            Debug.Log("finding...");
            var trees = GfUnityEditorUtility.FindAssets<BaseTree>(null, null);
            foreach (var tree in trees)
            {
                try
                {
                    Debug.Log("tree... : " + tree.name);
                    GfEBtSerializer.Serialize(tree);
                }
                catch (Exception e)
                {
                    Debug.LogError($"BehaviourTree error ({tree.name}) : {e}");
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}