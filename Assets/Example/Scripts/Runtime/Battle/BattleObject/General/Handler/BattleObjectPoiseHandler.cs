using Akari.GfCore;

namespace GameMain.Runtime
{
    /// <summary>
    /// 韧性，抗打断
    /// </summary>
    public class BattleObjectPoiseHandler
    {
        enum Status
        {
            None,//无
            InForce,//生效中
            Failure,//失效中
        }
    
        private readonly float _maxValue;

        private const float AttenuationSpeed = 0f; //衰减速度,/s

        private const float FailureTime = 5f; //失效时间，韧性值满后进入

        private float _failureElapsedTime = 0f;

        public float MaxValue => _maxValue;
        public float CurValue { get; private set; }
        public float CurrentRatio => _status == Status.None ? -1 : CurValue / MaxValue;
        public int TotalPercent => GfMathf.Min(100, IncreasePercent - ReductionPercent);//削韧抗性百分比,影响韧性值增加量，不可以超过100%
        public int IncreasePercent  { get; private set; }//削韧抗性增益百分比
        public int ReductionPercent { get; private set; }//削韧抗性减益百分比

        public bool IsFailure => _status != Status.InForce;//失效

        private Status _status = Status.None;
        
        public BattleObjectPoiseHandler(float maxValue)
        {
            if (maxValue > 0)
            {
                _status = Status.InForce;
            }
            
            _maxValue = maxValue;
            CurValue = 0;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_status == Status.None)
            {
                return;
            }
            
            if (_status == Status.InForce)
            {
                if (CurValue > 0)
                {
                    SetValue(CurValue - (AttenuationSpeed * deltaTime));
                }
            }
            else
            {
                _failureElapsedTime += deltaTime;
                if (_failureElapsedTime > FailureTime)
                {
                    _failureElapsedTime = 0f;
                    //重新生效
                    _status = Status.InForce;
                    CurValue = 0;
                }
                else
                {
                    CurValue = (1 - (_failureElapsedTime/FailureTime)) * MaxValue;
                }
            }
        }

        public void SetValue(float value)
        {
            if (_status == Status.None)
            {
                return;
            }
            
            CurValue = GfMathf.Clamp(value, 0, MaxValue);
        }

        public void AddValue(int valueToAdd,out bool isBreak)
        {
            isBreak = false;
            if (_status == Status.InForce)
            {
                var value = CurValue + valueToAdd * (1 - (TotalPercent * 0.01f));
                if (value > MaxValue)
                {
                    //进入失效状态，break
                    _status = Status.Failure;
                    isBreak = true;
                }
            
                SetValue(value);
            }
        }
        
        public void AddIncreasePercent(int percent)
        {
            IncreasePercent += percent;
        }
        
        public void SubtractIncreasePercent(int percent)
        {
            IncreasePercent -= percent;
        }

        public void AddReductionPercent(int percent)
        {
            ReductionPercent += percent;
        }
        
        public void SubtractReductionPercent(int percent)
        {
            ReductionPercent -= percent;
        }
    }
}