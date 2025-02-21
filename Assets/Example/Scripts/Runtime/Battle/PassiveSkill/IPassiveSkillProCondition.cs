namespace GameMain.Runtime
{
    public interface IPassiveSkillProCondition
    {
        void Tick(float deltaTime);
        bool CheckCondition();
        void TakeEffect();
    }

    public class APassiveSkillProCondition : IPassiveSkillProCondition
    {
        public virtual bool CheckCondition() { return false; }
        public virtual void Tick(float deltaTime) { }
        public virtual void TakeEffect() { }
    }

    public class TimeIntervalPassiveSkillProCondition : APassiveSkillProCondition
    {
        private readonly float _intervalTime;
        private float _elapsedTime;
        private bool _isCooling = false;
        
        public TimeIntervalPassiveSkillProCondition(float interval)
        {
            _intervalTime = interval;
        }
        
        public override bool CheckCondition()
        {
            return !_isCooling;
        }
        
        public override void Tick(float deltaTime)
        {
            if (_isCooling)
            {
                _elapsedTime += deltaTime;
                if (_elapsedTime > _intervalTime)
                {
                    _isCooling = true;
                }
            }
        }

        public override void TakeEffect()
        {
            _isCooling = true;
            _elapsedTime = 0f;
        }
    }

    public class AttributePassiveSkillProCondition : APassiveSkillProCondition
    {
        private readonly int _value;
        private readonly bool _isLessThan;
        
        private readonly IPropertyData _propertyData; 
        
        public AttributePassiveSkillProCondition(IBattleCharacterAccessorComponent accessor,AttributeType attributeType,int attributeValue,bool isLessThan)
        {
            var type = attributeType;
            _value = attributeValue;
            _isLessThan = isLessThan;
            
            _propertyData = accessor.Condition.GetConditionPropertyData(type);
        }
        
        public override bool CheckCondition()
        {
            var compareValue = _propertyData.CompareToValue(_value);
            return _isLessThan ? compareValue < 0 : compareValue >= 0;
        }
    }

    public class HasBufferProCondition : APassiveSkillProCondition
    {
        private readonly int _bufferId;
        private readonly IBattleCharacterAccessorComponent _accessor;
        public HasBufferProCondition(IBattleCharacterAccessorComponent accessor,int bufferId)
        {
            _bufferId = bufferId;

            _accessor = accessor;
        }
        
        public override bool CheckCondition()
        {
            return _accessor.Buffer.HasBuffer(_bufferId);
        }
    }
}