using System;
using Akari.GfCore;
using Akari.GfGame;
using Akari.GfUnity;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameMain.Runtime
{
    public sealed class BattleUnityEffectFactory : IBattleEffectFactory
    {
        public void Dispose()
        {
            GfPrefabPool.DeleteAllPool();
        }

        public async UniTask<GfHandle> CreateEffect(
            GfEntity owner,
            string effectId,
            EffectGroup effectGroup,
            GfFloat3 position,
            GfQuaternion rotation,
            GfFloat3 scale,
            GfVfxDeleteMode deleteMode,
            GfVfxPriority priority)
        {
            if (string.IsNullOrEmpty(effectId))
            {
                return GfHandle.Invalid;
            }

            //var go = await AssetManager.Instance.LoadAsset<GameObject>(AssetPathHelper.GetBattleEffectPath(effectId));
            var go = await AssetManager.Instance.LoadAsset<GameObject>(effectId);
            if (!go)
            {
                return GfHandle.Invalid;
            }
            var prefab = go.GetComponent<BattleUnityEffectView>();

            return CreateEffectInternal(
                owner,
                prefab,
                effectGroup,
                position,
                rotation,
                scale,
                deleteMode,
                priority);
        }

        public async UniTask<GfHandle> CreateEffectByEntity(
            GfEntity owner,
            string effectId,
            EffectGroup effectGroup,
            GfFloat3 offsetPosition,
            GfQuaternion offsetRotation,
            GfFloat3 offsetScale,
            GfVfxDeleteMode deleteMode,
            GfVfxPriority priority)
        {
            if (string.IsNullOrEmpty(effectId))
            {
                return GfHandle.Invalid;
            }
            
            //var go = await AssetManager.Instance.LoadAsset<GameObject>(AssetPathHelper.GetBattleEffectPath(effectId));
            var go = await AssetManager.Instance.LoadAsset<GameObject>(effectId);
            if (!go)
            {
                return GfHandle.Invalid;
            }
            var prefab = go.GetComponent<BattleUnityEffectView>();
            
            GfFloat3 position;
            GfQuaternion rotation;
            GfFloat3 scale;
            
            switch (prefab.OffsetType)
            {
                case BattleUnityEffectOffsetType.BoundsCenter:
                {
                    var ownerTransform = owner.Transform;
                    position = GfTransformHelper.TransformPoint(ownerTransform.Position + new GfFloat3(0, 0.75f, 0), ownerTransform.Rotation, ownerTransform.Scale, offsetPosition);
                    rotation = ownerTransform.Rotation *  offsetRotation;
                    scale = ownerTransform.Scale * offsetScale;
                    
                    return CreateEffectInternal(
                        owner,
                        prefab,
                        effectGroup,
                        position,
                        rotation,
                        scale,
                        deleteMode,
                        priority);
                }
                case BattleUnityEffectOffsetType.Root:
                {
                    var ownerTransform = owner.Transform;
                    position     = GfTransformHelper.TransformPoint(ownerTransform.Position, ownerTransform.Rotation,ownerTransform.Scale,offsetPosition);
                    rotation     = ownerTransform.Rotation *  offsetRotation;
                    scale        = ownerTransform.Scale * offsetScale;
                    
                    return CreateEffectInternal(
                        owner,
                        prefab,
                        effectGroup,
                        position,
                        rotation,
                        scale,
                        deleteMode,
                        priority);
                }
                case BattleUnityEffectOffsetType.RootAttach:
                {
                    var ownerTransform = owner.Transform;
                    position     = GfTransformHelper.TransformPoint(ownerTransform.Position, ownerTransform.Rotation,ownerTransform.Scale,offsetPosition);
                    rotation     = ownerTransform.Rotation *  offsetRotation;
                    scale        = ownerTransform.Scale * offsetScale;
                    
                    var handle = CreateEffectInternal(
                        owner,
                        prefab,
                        effectGroup,
                        position,
                        rotation,
                        scale,
                        deleteMode,
                        priority);
                    
                    var unityView = owner.GetComponent<BattleCharacterViewComponent>();
                    var component = unityView.BoneComponent;
                    component?.Attach(new GfBoneAdapterForVfx(BattleAdmin.VfxManager, handle, false),
                        "root",
                        offsetPosition,
                        offsetRotation.ToEulerAngles());

                    return handle;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private GfHandle CreateEffectInternal(
            GfEntity owner,
            BattleUnityEffectView prefab,
            EffectGroup effectGroup,
            GfFloat3 position,
            GfQuaternion rotation,
            GfFloat3 scale,
            GfVfxDeleteMode deleteMode,
            GfVfxPriority priority,
            int span = -1,
            bool isAutoDelete = true)
        {
            var isPooled   = prefab.IsPooled;
            
            BattleUnityEffectView instance;
            if (isPooled)
            {
                instance = GfPrefabPool.IssueOrCreatPool(prefab);
                if (!instance)
                {
                    return GfHandle.Invalid;
                }
            }
            else
            {
                instance = Object.Instantiate(prefab);
            }
            
            
            var transform = instance.transform;
            transform.localPosition = position.ToVector3();
            transform.localRotation = rotation.ToQuaternion();
            transform.localScale    = scale.ToVector3();
            var effect = new GfVfxObject(
                GfUnityVfxAdapterFactory.Create(
                    instance,
                    span,
                    isAutoDelete,
                    isPooled),
                (byte)effectGroup.EnumToInt(),
                deleteMode,
                priority);

            var handle = BattleAdmin.VfxManager.Add(effect);
            if (handle == GfHandle.Invalid)
            {
                if (isPooled)
                {
                    GfPrefabPool.Return(instance);
                }

                return GfHandle.Invalid;
            }

            
            //if (prefab.AffectOwnerSpeed)
            if (owner != null && owner.IsActive)
            {
                var speedAffectComponent = owner.GetComponent<GfVfxSpeedAffectComponent>();
                speedAffectComponent?.Add(handle);
            }
            
            return handle;
        }
    }
}
