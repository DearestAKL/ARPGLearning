using Akari.GfCore;

namespace GameMain.Runtime
{
    public static class FishDef
    {
        public const float FishEscapeTime = 5F;//逃脱需要时间
        public const float FishChangeTime = 3F;//改变拉力框位置Cd
        public const float AddForceCoefficient = 0.75F;//addForce 系数
        public const float SubForceCoefficient = 0.5F;//SubForce 系数
        public const float EnergyRecovery = 10F;//全局精力恢复参数
        
        private const float MinFrameForce = 0.1F;//最小拉力框
        private const float MaxFrameForce = 0.9F;//最大拉力框
        private const float BaseFrameForce = 0.3F;//基础拉力框

        /// <summary>
        /// 计算拉力框宽度
        /// </summary>
        /// <param name="fishData"></param>
        /// <param name="fishingRodData"></param>
        /// <param name="barWidth"></param>
        /// <returns></returns>
        public static float CalculateFrameWidth(FishData fishData,FishingRodData fishingRodData,float barWidth)
        {
            float coefficient = BaseFrameForce + (fishingRodData.Stability - fishData.Power) * 0.1f;
            coefficient = GfMathf.Clamp(coefficient, MinFrameForce, MaxFrameForce);
            return coefficient * barWidth;
        }
    }
    
    public enum FishStatus
    {
        None,
        Start,
        InBar,    // 在拉力条中
        OutBar,   // 在拉力条外
        Escape,   // 逃脱
        Catch     // 捕获
    }
    
    public class FishData
    {
        public float Energy; // 精力, 精力为0时会被捕获
        public float Power;  // 力量, 越大抓鱼力量框越窄
    }
    
    public class FishingRodData
    {
        public float Stability; // 稳定性, 越大抓鱼力量框越宽
        public float DragPower; // 钓力, 越大消耗精力越快
    }
}