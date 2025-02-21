using Akari.GfCore;

namespace GameMain.Runtime
{
    public readonly struct ChangeAnimationRequest  : IGfRequest
    {
        public          GfRunTimeTypeId RttId => GfRunTimeTypeOf<ChangeAnimationRequest>.Id;
        public readonly string AnimationName;

        public ChangeAnimationRequest(string animationName)
        {
            AnimationName     = animationName;
        }
    }
    
    public readonly struct BattleRegisterColliderAttackRequest : IGfRequest
    {
        public          GfRunTimeTypeId RttId => GfRunTimeTypeOf<BattleRegisterColliderAttackRequest>.Id;
        public readonly bool IsOn;
        public readonly AttackId AttackId;

        public BattleRegisterColliderAttackRequest(
            bool isOn,
            in AttackId attackId)
        {
            IsOn     = isOn;
            AttackId = attackId;
        }
    }
    
    public readonly struct BattleCurHpChangeRequest : IGfRequest
    {
        public          GfRunTimeTypeId RttId => GfRunTimeTypeOf<BattleCurHpChangeRequest>.Id;
        public readonly bool IsAdd;

        public BattleCurHpChangeRequest(bool isAdd)
        {
            IsAdd = isAdd;
        }
    }

    public readonly struct BattlePlayDamageEffectAndUIRequest : IGfRequest
    {
        public GfRunTimeTypeId RttId => GfRunTimeTypeOf<BattlePlayDamageEffectAndUIRequest>.Id;

        // NOTE: BattleDamageResult不能跨帧保留(确切地说，EntityComponentSystem的更新处理不能跨某个帧保留)
        public readonly IBattleDamageResult[] DamageResults;
        public readonly IBattleSimpleDamageResult[] SimpleDamageResults;

        public BattlePlayDamageEffectAndUIRequest(IBattleDamageResult[] damageResults,IBattleSimpleDamageResult[] simpleDamageResults)
        {
            DamageResults = damageResults;
            SimpleDamageResults = simpleDamageResults;
        }
    }
    
    public readonly struct BattleDidCauseDamageRequest : IGfRequest
    {
        public          GfRunTimeTypeId     RttId => GfRunTimeTypeOf<BattleDidCauseDamageRequest>.Id;
        public readonly BattleDamageResult DamageResult;

        public BattleDidCauseDamageRequest(
            BattleDamageResult damageResult)
        {
            DamageResult = damageResult;
        }
    }

    public readonly struct BattleReceivedDamageRequest : IGfRequest
    {
        public          GfRunTimeTypeId     RttId => GfRunTimeTypeOf<BattleReceivedDamageRequest>.Id;

        public readonly BattleDamageResult DamageResult;
        public BattleReceivedDamageRequest(BattleDamageResult damageResult)
        {
            DamageResult = damageResult;
        }
    }

    public readonly struct SubsidiaryPlayAnimationRequest  : IGfRequest
    {
        public          GfRunTimeTypeId RttId => GfRunTimeTypeOf<SubsidiaryPlayAnimationRequest>.Id;
        public readonly string AnimationName;

        public SubsidiaryPlayAnimationRequest(string animationName)
        {
            AnimationName     = animationName;
        }
    }
    
    // public readonly struct BattleCurHpChangeRequest : IGfRequest
    // {
    //     public          GfRunTimeTypeId RttId => GfRunTimeTypeOf<BattleCurHpChangeRequest>.Id;
    //     public readonly bool IsAdd;
    //
    //     public BattleCurHpChangeRequest(bool isAdd)
    //     {
    //         IsAdd = isAdd;
    //     }
    // }
    
    public readonly struct UpdatePropertyRequest : IGfRequest
    {
        public GfRunTimeTypeId RttId => GfRunTimeTypeOf<UpdatePropertyRequest>.Id;
    }
    
    public readonly struct DeleteShellRequest : IGfRequest
    {
        public GfRunTimeTypeId RttId => GfRunTimeTypeOf<DeleteShellRequest>.Id;
    }
    
    public readonly struct CurHpMakeZeroRequest : IGfRequest
    {
        public GfRunTimeTypeId RttId => GfRunTimeTypeOf<CurHpMakeZeroRequest>.Id;
    }
}