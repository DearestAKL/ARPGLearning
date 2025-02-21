namespace GameMain.Runtime
{
    public class BattleDamageCalculatorHelper
    {
        /// <summary>
        /// 计算防御减伤系数
        /// </summary>
        /// <param name="causer"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public static float CalcDefenseDamageReduction(IBattleObjectDamageCauserHandler causer,IBattleObjectDamageReceiverHandler receiver)
        {
            // 防御减伤 = 防御/(防御 + causer等级*5 + 500)

            var causerLevel = causer.GetLevel();
            var receiverDefense = receiver.GetDefense();
            
            //敌人防御固定为 等级*5+500，所有玩家和敌人同等级时减伤50%，
            // 如果receiver是怪物，因为怪物防御力为Level*5 + 500
            // 根据公式receiverLevel*5 + 500/(receiverLevel*5 + 500 +causerLevel*5 + 500) = receiverLevel+100/(receiverLevel+100 + causerLevel+100)
            // 可以简化为 receiverLevel+100/(receiverLevel+100 + causerLevel+100)
            
            // 1 1=> 101/202 50%
            // 1 90=> 101/291 35.48%
            // 90 1=> 190/291 65.29%
            // 90 90=> 190/380 50%
            
            //玩家防御 走配置 如果玩家1级防御力是 100
            //怪物等级越高造成伤害越高
            // 1 => 100/(100+505) = 16.53%
            // 90 => 100/(100+950) = 9.52%;
            
            return receiverDefense / (receiverDefense + causerLevel * 5 + 500);
        }
    }
}