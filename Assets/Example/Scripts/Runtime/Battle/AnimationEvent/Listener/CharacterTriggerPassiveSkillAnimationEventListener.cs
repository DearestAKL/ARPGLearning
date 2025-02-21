using Akari.GfCore;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public interface ICharacterTriggerPassiveSkillAnimationEventListener: IGfAnimationEventListener
    {
        void RegisterTriggerPassiveSkillEvent(AnimationEventParameterIntValue param, GfAnimationEventCallInfo info);
    }
    
    public class CharacterTriggerPassiveSkillAnimationEventListener :  ACharacterAnimationEventListener,ICharacterTriggerPassiveSkillAnimationEventListener 
    {
        public GfRunTimeTypeId RttId { get; }

        public CharacterTriggerPassiveSkillAnimationEventListener(
            GfEntity selfEntity)
            : base(selfEntity)
        {
            RttId   = GfRunTimeTypeOf<CharacterTriggerPassiveSkillAnimationEventListener>.Id;
        }

        public void RegisterTriggerPassiveSkillEvent(AnimationEventParameterIntValue param, GfAnimationEventCallInfo info)
        {
            Accessor.PassiveSkill.TriggerEvents(param.Value, PassiveSkillEventType.OnAnimationEvent);
        }
    }
}