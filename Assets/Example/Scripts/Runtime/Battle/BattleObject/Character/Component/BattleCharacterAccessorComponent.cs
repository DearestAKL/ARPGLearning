using Akari.GfCore;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public interface IBattleCharacterAccessorComponent
    {
        GfEntity Entity { get; }
        GfActorComponent Actor { get; }
        BattleCharacterAttackComponent Attack { get; }
        BattleObjectActionComponent Action { get; }
        BattleCharacterConditionComponent Condition { get; }
        BattleCharacterTransformComponent Transform { get; }
        BattleCharacterPassiveSkillComponent PassiveSkill { get; }
        BattleCharacterBufferComponent Buffer { get; }
        GfColliderComponent2D<uint, BattleColliderAttackParameter, BattleColliderDefendParameter> ColliderComponent { get; }
        
        IBattleObjectDamageNotificator DamageNotificator { get; }
        IBattleObjectDamageReceiverHandler DamageReceiver{ get; }
        
        bool IsAlive{ get; }
    }

    public class BattleCharacterAccessorComponent : AGfGameComponent<BattleCharacterAccessorComponent>,IBattleCharacterAccessorComponent
    {
        public GfActorComponent Actor { get; }

        public BattleCharacterAttackComponent Attack { get; }
        public BattleObjectActionComponent Action { get; }
        public BattleCharacterConditionComponent Condition { get; }
        public BattleCharacterTransformComponent Transform { get; }
        public BattleCharacterPassiveSkillComponent PassiveSkill { get; }
        public BattleCharacterBufferComponent Buffer { get; }
        
        public BattleDamageWarningComponent DamageWarning { get; }

        public GfColliderComponent2D<uint, BattleColliderAttackParameter, BattleColliderDefendParameter> ColliderComponent { get; }
        
        public IBattleObjectDamageNotificator DamageNotificator { get; private set; }
        public IBattleObjectDamageReceiverHandler DamageReceiver { get; private set; }
        public IBattleObjectDamageCauserHandler DamageCauser => Attack.DamageCauserHandler;

        public bool IsAlive => Condition.HpProperty.IsAlive && IsActive;//血量大于0 且Component和Entity 都是激活状态

        public BattleCharacterAccessorComponent(GfEntity entity)
        {
            Entity = entity;
            Actor = entity.GetComponent<GfActorComponent>();
            Attack = entity.GetComponent<BattleCharacterAttackComponent>();
            ColliderComponent = entity.GetComponent<GfColliderComponent2D<uint, BattleColliderAttackParameter, BattleColliderDefendParameter>>();
            Action = entity.GetComponent<BattleObjectActionComponent>();
            Condition = entity.GetComponent<BattleCharacterConditionComponent>();
            Transform = entity.GetComponent<BattleCharacterTransformComponent>();
            PassiveSkill = entity.GetComponent<BattleCharacterPassiveSkillComponent>();
            Buffer = entity.GetComponent<BattleCharacterBufferComponent>();
            DamageWarning = entity.GetComponent<BattleDamageWarningComponent>();
        }

        public void SetDamageNotificator(IBattleObjectDamageNotificator damageNotificator)
        {
            DamageNotificator = damageNotificator;
        }
        
        public void SetDamageReceiver(IBattleObjectDamageReceiverHandler damageReceiver)
        {
            DamageReceiver = damageReceiver;
        }
    }
}
