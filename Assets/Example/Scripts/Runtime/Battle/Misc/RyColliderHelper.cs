using Akari.GfCore;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public static class RyColliderHelper
    {
        //public static GfCollider2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter> CreateColliderForDefenderSize(
        //    InGameCharacterModel inGameCharacterModel,
        //    float additionalCenterZOffset = 0f)
        //{
        //    float xExtent = inGameCharacterModel.XSizeType.GetDefenderColliderExtent();
        //    float zExtent = inGameCharacterModel.ZSizeType.GetDefenderColliderExtent();
        //    var center = new GfFloat2(0f, inGameCharacterModel.DefenderColliderZOffset + additionalCenterZOffset);
        //    var extents = new GfFloat2(xExtent, zExtent);

        //    return new GfCollider2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter>(center, extents, false);
        //}

        public static GfCollider2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter> CreateColliderForDefenderSize()
        {
            var center = new GfFloat2(0f, 0);
            //var extents = new GfFloat2(0.5f, 0.5f);
            var extents = new GfFloat2(0.5f, 0);

            return new GfCollider2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter>(center, extents, false);
        }
        
        public static GfCollider2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter> CreateColliderForDefenderSize(GfFloat2 center,GfFloat2 extents)
        {
            return new GfCollider2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter>(center, extents, false);
        }
    }
}
