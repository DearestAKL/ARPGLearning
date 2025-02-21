using Akari.GfCore;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public interface ICharacterActionCancelAnimationEventListener : IGfAnimationEventListener
    {
        void RegisterCanMove(AnimationEventParameterNull param, GfAnimationEventCallInfo info);
        void RegisterCanAttack(AnimationEventParameterNull param, GfAnimationEventCallInfo info);
        void RegisterCanDash(AnimationEventParameterNull param, GfAnimationEventCallInfo info);
        void RegisterIsDamageImmunity(AnimationEventParameterNull param, GfAnimationEventCallInfo info);
        void RegisterIsSuperArmor(AnimationEventParameterNull param, GfAnimationEventCallInfo info);
        void RegisterIsDodge(AnimationEventParameterNull param, GfAnimationEventCallInfo info);
    }

    public sealed class CharacterActionCancelAnimationEventListener : ACharacterAnimationEventListener, ICharacterActionCancelAnimationEventListener
    {
        public GfRunTimeTypeId RttId { get; }
        

        public CharacterActionCancelAnimationEventListener(
            GfEntity selfEntity)
            : base(selfEntity)
        {
            RttId    = GfRunTimeTypeOf<CharacterActionCancelAnimationEventListener>.Id;
        }

        public void RegisterCanMove(AnimationEventParameterNull param, GfAnimationEventCallInfo info)
        {
            Accessor?.Condition.Frame.CanMove.SetBuffer(info.Reason.ToIsOn(), info.Reason.ToIsOn());
        }

        public void RegisterCanAttack(AnimationEventParameterNull param, GfAnimationEventCallInfo info)
        {
            Accessor?.Condition.Frame.CanAttack.SetBuffer(info.Reason.ToIsOn(), info.Reason.ToIsOn());
        }
        
        public void RegisterCanDash(AnimationEventParameterNull param, GfAnimationEventCallInfo info)
        {
            Accessor?.Condition.Frame.CanDash.SetBuffer(info.Reason.ToIsOn(), info.Reason.ToIsOn());
        }
        
        public void RegisterIsDamageImmunity(AnimationEventParameterNull param, GfAnimationEventCallInfo info)
        {
            Accessor?.Condition.Frame.IsDamageImmunity.SetBuffer(info.Reason.ToIsOn(), info.Reason.ToIsOn());
        }
        
        public void RegisterIsSuperArmor(AnimationEventParameterNull param, GfAnimationEventCallInfo info)
        {
            Accessor?.Condition.Frame.IsSuperArmor.SetBuffer(info.Reason.ToIsOn(), info.Reason.ToIsOn());
        }
        
        public void RegisterIsDodge(AnimationEventParameterNull param, GfAnimationEventCallInfo info)
        {
            Accessor?.Condition.Frame.IsDodge.SetBuffer(info.Reason.ToIsOn(), info.Reason.ToIsOn());
        }
    }
}
