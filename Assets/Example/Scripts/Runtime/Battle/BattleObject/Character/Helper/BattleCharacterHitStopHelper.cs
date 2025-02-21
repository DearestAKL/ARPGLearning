using Akari.GfGame;

namespace GameMain.Runtime
{
    public static class BattleCharacterHitStopHelper
    {
        public static void RequestHitStop(
            BattleDamageResult damageResult,
            IBattleCharacterAccessorComponent thisAccessor,
            IBattleCharacterAccessorComponent causerAccessor)
        {
            bool isTruth = true;
            int attackerActionOperationId = 0;

            if (damageResult.AttackerHitStopSpan > 0.01f)
            {
                causerAccessor.Entity.Request(new GfChangeSpeedMagnificationRequest(attackerActionOperationId,
                    0F,
                    damageResult.AttackerHitStopSpan,
                    isTruth));
            }
            
            if (damageResult.DefenderHitStopSpan > 0.01f)
            {
                thisAccessor.Entity.Request(new GfChangeSpeedMagnificationRequest(attackerActionOperationId,
                    0F,
                    damageResult.DefenderHitStopSpan,
                    isTruth));
            }
        }

        public static void CalcHitStopSpan(BattleDamageResult damageResult)
        {
            var infoData = damageResult.AttackParameter.SingleAttackModel.Info;
            
            damageResult.AttackerHitStopSpan = 0F;
            damageResult.DefenderHitStopSpan = 0F;

            //todo:if hitstop 没有开启则返回
            
            var attackerHitStopSpan = JudgeHitStopSpan(3);
            if (attackerHitStopSpan > 0.01f)
            {
                damageResult.AttackerHitStopSpan = attackerHitStopSpan;
            }
            
            var defenderHitStopSpan = JudgeHitStopSpan(3);
            if (defenderHitStopSpan > 0.01f)
            {
                damageResult.DefenderHitStopSpan = defenderHitStopSpan;
            }
        }

        private static float JudgeHitStopSpan(int hitStopLv)
        {
            var hitStopSpan = 0f;
            switch (hitStopLv)
            {
                case 1:
                    hitStopSpan = 0.02f;
                    break;
                case 2:
                    hitStopSpan = 0.04f;
                    break;
                case 3:
                    hitStopSpan = 0.06f;
                    break;
                case 4:
                    hitStopSpan = 0.08f;
                    break;
            }

            return hitStopSpan;
        }
    }
}