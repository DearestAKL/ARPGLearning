using GameMain.Runtime;
using UnityEditor;
using UnityEngine;

namespace GameMain.Editor
{
    [CustomEditor(typeof(UITMProExpand))]
    public class UITMProExpandEditor : UnityEditor.Editor
    {
        private UITMProExpand _view;

        public void OnEnable()
        {
            _view = target as UITMProExpand;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            // 保存
            if (GUILayout.Button("Apply", GUILayout.Height(24)))
            {
                _view.Preparation();
                EditorUtility.SetDirty(_view.gameObject);
            }
        }
    }
}