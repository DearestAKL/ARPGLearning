using Akari.GfCore;
using Akari.GfGame;
using Akari.GfUnityEditor.AnimationEventBundler;
using Google.Protobuf;
using GameMain.Runtime;
using UnityEngine;

namespace GameMain.Editor
{
    public class EditorAnimationEventParameterEffectPlay : AnimationEventObjectParameterBase
    {
        //[AnimationEventPathConvert(UnityPath.Effect.EffectPrefabPrefix, ".prefab", true, true)]
        public GameObject Effect;//使用GameObject 在调用JsonUtility.FromJsonOverwrite时会有报错，但是不影响
        public string EffectId;
        public string TargetBoneName;
        public GfFloat3Data OffsetPosition;
        public GfFloat3Data OffsetRotation;
        public GfFloat3Data EffectSize = new GfFloat3Data(1.0f, 1.0f, 1.0f);
        public float CameraOffset;
        public bool CancelRemove;
        public bool ChangeAttribute;

        public override int GetPbTypeId()
        {
            return RyPbTypes.AnimationEventParameterEffectPlay;
        }

        public override IMessage Serialize()
        {
#if UNITY_EDITOR
            if (Effect != null)
            {
                EffectId = GetEffectAssetPath();
            }
#endif
            
            var message = new AnimationEventParameterMessageEffectPlay();
            message.EffectId = EffectId ?? string.Empty;
            message.TargetBoneName = TargetBoneName ?? string.Empty;
            message.OffsetPosition = OffsetPosition.ToGfFloat3().ToGfFloat3Message();
            message.OffsetRotation = OffsetRotation.ToGfFloat3().ToGfFloat3Message();
            message.EffectSize = EffectSize.ToGfFloat3().ToGfFloat3Message();
            message.CameraOffset = CameraOffset;
            message.CancelRemove = CancelRemove;
            message.ChangeAttribute = ChangeAttribute;
            return message;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Effect != null)
            {
                EffectId = GetEffectAssetPath();
            }
            else if(!string.IsNullOrEmpty(EffectId))
            {
                //Effect丢失了,尝试通过路径找回
                Effect = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(EffectId);
            }
        }

        private string GetEffectAssetPath()
        {
            var rootPath = UnityEditor.AssetDatabase.GetAssetPath(Effect);
            return rootPath;
            //return AssetPathHelper.ReplaceBattleEffectPath(rootPath);
        }
#endif

    }
}
