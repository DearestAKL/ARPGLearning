using GameMain.Runtime;
using UnityEditor;
using UnityEngine;

namespace GameMain.Editor
{
    [CustomEditor(typeof(RoguelikeRoomView))]
    public class BattleRoomEditor : UnityEditor.Editor
    {
        private RoguelikeRoomView _view;

        public void OnEnable()
        {
            _view = target as RoguelikeRoomView;
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (GUILayout.Button("Update"))
            {
                _view.Preparation();
                EditorUtility.SetDirty(_view.gameObject);
            }
        }
    }
}