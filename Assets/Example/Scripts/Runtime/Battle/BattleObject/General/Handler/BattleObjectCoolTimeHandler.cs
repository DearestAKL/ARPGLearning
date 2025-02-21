using Akari.GfCore;

namespace GameMain.Runtime
{
    public class BattleObjectCoolTimeHandler
    {
        private const    int                 MinTotalPercent        = 0;
        private const    int                 MaxTotalPercent        = 100;

        public int MaxSlotNum { get; private set; }//最大存储数量
        public float SingleCoolTimeBase { get; private set; }//单个基础冷却时间
        public float SingleCoolTime => SingleCoolTimeBase * (TotalPercent * 0.01f);
        public float TotalCoolTimeBase { get; private set; }//总基础冷却时间
        public float TotalCoolTime           => TotalCoolTimeBase * (TotalPercent * 0.01f);
        public float CurrentCoolTime         { get; private set; }
        
        public float CurrentSlotCoolTime     => CurrentCoolTime - CurrentSlotNum * SingleCoolTime;
        public float CurrentSlotCoolTimeRate => (SingleCoolTime <= 0) ? 0 : CurrentSlotCoolTime / SingleCoolTime;
        public int   CurrentSlotNum          => (SingleCoolTime <= 0) ? 0 : GfMathf.FloorToInt(CurrentCoolTime / SingleCoolTime);

        public int TotalPercent => GfMathf.Clamp(100 + IncreasePercent - ReductionPercent, MinTotalPercent, MaxTotalPercent);
        public int IncreasePercent         { get; private set; }
        public int ReductionPercent        { get; private set; }

        public BattleObjectCoolTimeHandler(float coolTime, int maxSlotNum, int startSlotNum)
        {
            SingleCoolTimeBase = coolTime;
            MaxSlotNum = maxSlotNum;
            CurrentCoolTime = startSlotNum * coolTime;
            
            TotalCoolTimeBase  = SingleCoolTimeBase * MaxSlotNum;

            if (CurrentCoolTime > TotalCoolTimeBase)
            {
                CurrentCoolTime = TotalCoolTimeBase;
            }
        }

        public void SetCoolTime(float newCt)
        {
            CurrentCoolTime = newCt;
            if (newCt < 0)
            {
                CurrentCoolTime = 0;
                return;
            }
            
            if (newCt > TotalCoolTime)
            {
                CurrentCoolTime = TotalCoolTime;
            }
        }
        
        public void AddCoolTime(float ctToAdd)
        {
            SetCoolTime(CurrentCoolTime + ctToAdd);
        }
        
        public void SubtractCoolTime(float ctToSubtract)
        {
            SetCoolTime(CurrentCoolTime - ctToSubtract);
        }
        
        public void SetIncreasePercent(int percent)
        {
            IncreasePercent = percent;
        }

        public void SetReductionPercent(int percent)
        {
            ReductionPercent = percent;
        }

        public void OnUpdate(float deltaTime)
        {
            AddCoolTime(deltaTime);
        }
    }
}