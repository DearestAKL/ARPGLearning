using Akari.GfCore;

namespace GameMain.Runtime
{
    public sealed class SingleAttackModel
    {
        public AttackId AttackId { get; }
        public AttackDefinitionCollisionData[] Collisions => Info.Collisions;
        public AttackDefinitionInfoData Info { get;}

        private readonly BattleHitCategory _hitCategory;

        public SingleAttackModel(AttackId attackId, AttackDefinitionInfoData info)
        {
            AttackId = attackId;
            Info = info;

            _hitCategory = Info.HitCategory;
        }

        public bool CanHitByHitCategory(TeamId causerTeamId,
            TeamId receiverTeamId, GfHandle causerHandle,GfHandle receiverHandle)
        {
            if (causerTeamId == TeamId.Invalid || receiverTeamId == TeamId.Invalid) 
            {
                return false;
            }
            
            //处理对己方生效的HitCategory,区分Myself以及SameWithoutMySelf
            
            //是否对自己生效
            if (causerHandle == receiverHandle) 
            {
                if ((_hitCategory & BattleHitCategory.Myself) == 0)
                {
                    return false;
                }
            }


            if(causerTeamId == receiverTeamId) 
            {
                //是否对队友生效
                if ((_hitCategory & BattleHitCategory.SameWithoutMySelf) == 0)
                {
                    return false;
                }
            }
            else
            {
                //是否对敌人生效
                if ((_hitCategory & BattleHitCategory.Other) == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
