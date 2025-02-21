using System.Collections.Generic;
using Akari.GfCore;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public interface ICharacterDamageAnimationEventListener : IGfAnimationEventListener
    {
        void RegisterAttack(AnimationEventParameterUintId param, GfAnimationEventCallInfo info);
    }

    public sealed class CharacterDamageAnimationEventListener : ACharacterAnimationEventListener,ICharacterDamageAnimationEventListener
    {

        private readonly Dictionary<GfAnimationEventSubTrackHandle, AttackId> _subTrackHandleToAttackIdMap;

        public GfRunTimeTypeId RttId { get; }

        public CharacterDamageAnimationEventListener(
            GfEntity selfEntity)
            : base(selfEntity)
        {
            RttId = GfRunTimeTypeOf<CharacterDamageAnimationEventListener>.Id;

            _subTrackHandleToAttackIdMap = new Dictionary<GfAnimationEventSubTrackHandle, AttackId>();
        }

        public void RegisterAttack(AnimationEventParameterUintId param, GfAnimationEventCallInfo info)
        {
            var selfEntity = GetSelfEntity();
            if (selfEntity == null)
            {
                return;
            }

            var attackId = _subTrackHandleToAttackIdMap.GetValue(info.Handle);
            if (attackId == AttackId.Invalid)
            {
                attackId = AttackId.CreateForCharacterAttack(param.Id);
                
                if (attackId == AttackId.Invalid)
                {
                    return;
                }

                _subTrackHandleToAttackIdMap.Add(info.Handle, attackId);
            }
            
            selfEntity.Request(new BattleRegisterColliderAttackRequest(info.Reason.ToIsOn(), attackId));
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
