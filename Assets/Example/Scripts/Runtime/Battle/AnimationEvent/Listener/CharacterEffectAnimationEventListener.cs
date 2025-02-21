using System.Collections.Generic;
using Akari.GfCore;
using Akari.GfGame;
using Cysharp.Threading.Tasks;

namespace GameMain.Runtime
{
    
    public interface ICharacterEffectAnimationEventListener : IGfAnimationEventListener
    {
        void RegisterEffectPlay(AnimationEventParameterEffectPlay param, GfAnimationEventCallInfo info);
        
        void RegisterLoopEffectPlay(AnimationEventParameterEffectPlay param, GfAnimationEventCallInfo info);
        void RegisterEffectAttach(AnimationEventParameterEffectPlay param, GfAnimationEventCallInfo info);
        void RegisterEffectDetach(AnimationEventParameterString param, GfAnimationEventCallInfo info);
    }

    public sealed class CharacterEffectAnimationEventListener : ACharacterAnimationEventListener,ICharacterEffectAnimationEventListener 
    {
        private          GfComponentCache<GfBoneComponent>                            _boneComponentCache;
        private readonly Dictionary<GfAnimationEventSubTrackHandle, GfHandle> _effectHandleMap;
        
        public GfRunTimeTypeId RttId { get; }
        private GfBoneComponent BoneComponent => GetSelfEntity().GetComponent(ref _boneComponentCache);

        public CharacterEffectAnimationEventListener(
            GfEntity selfEntity)
            : base(selfEntity)
        {
            RttId = GfRunTimeTypeOf<CharacterEffectAnimationEventListener>.Id;
            _effectHandleMap = new Dictionary<GfAnimationEventSubTrackHandle, GfHandle>();
        }

        public async void RegisterEffectPlay(AnimationEventParameterEffectPlay param, GfAnimationEventCallInfo info)
        {
            var effHandle = await RegisterEffectPlay(param);
        }

        public async void RegisterLoopEffectPlay(AnimationEventParameterEffectPlay param, GfAnimationEventCallInfo info)
        {
            switch (info.Reason)
            {
                case GfAnimationEventCallReason.Begin:
                {
                    var effHandle = await RegisterEffectPlay(param);
                    _effectHandleMap.Add(info.Handle, effHandle);

                    if (param.CancelRemove)
                    {
                        var action = Accessor?.Action?.GetNowAction<ABattleCharacterAction>();
                        action?.AddEffectHandle(effHandle);
                    }
                    break;
                }
                case GfAnimationEventCallReason.End:
                {
                    var effHandle = _effectHandleMap[info.Handle];
                    BattleAdmin.VfxManager.Stop(effHandle);
                    _effectHandleMap.Remove(info.Handle);
                    break;
                }
            }
        }
        
        public async void RegisterEffectAttach(AnimationEventParameterEffectPlay param, GfAnimationEventCallInfo info)
        {
            //effect的BattleUnityEffectOffsetType 不能是RootAttach
            
            var effHandle = await RegisterEffectPlay(param);
            
            BoneComponent.Attach(new GfBoneAdapterForVfx(BattleAdmin.VfxManager, effHandle, false),
                param.TargetBoneName,
                param.OffsetPosition,
                param.OffsetRotation);
        }
        
        public void RegisterEffectDetach(AnimationEventParameterString param, GfAnimationEventCallInfo info)
        {
            //TargetBoneName
            BoneComponent.Detach(param.Content);
        }

        private UniTask<GfHandle> RegisterEffectPlay(AnimationEventParameterEffectPlay param)
        {
            return BattleAdmin.Factory.Effect.CreateEffectByEntity(
                GetSelfEntity(),
                param.EffectId,
                EffectGroup.OneShot,
                param.OffsetPosition,
                GfQuaternion.Euler(param.OffsetRotation),
                param.EffectSize,
                GfVfxDeleteMode.Stop,
                GfVfxPriority.High);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
