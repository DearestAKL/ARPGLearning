using Akari.GfCore;
using Akari.GfGame;
using Akari.GfUnity;
using UnityEngine;

namespace GameMain.Runtime
{
    public interface ICharacterSoundAnimationEventListener : IGfAnimationEventListener
    {
        void RegisterCommonSoundPlay(AnimationEventParameterString param, GfAnimationEventCallInfo info);
    }
        
    public sealed class CharacterSoundAnimationAnimationEventListener :  ACharacterAnimationEventListener,ICharacterSoundAnimationEventListener 
    {
        public GfRunTimeTypeId RttId { get; }

        public CharacterSoundAnimationAnimationEventListener(
            GfEntity selfEntity)
            : base(selfEntity)
        {
            RttId   = GfRunTimeTypeOf<CharacterSoundAnimationAnimationEventListener>.Id;
        }

        public void RegisterCommonSoundPlay(AnimationEventParameterString param, GfAnimationEventCallInfo info)
        {
            AudioManager.Instance.PlaySound(param.Content, true, Accessor.Entity.Transform.Position.ToVector3());
        }
    }
}