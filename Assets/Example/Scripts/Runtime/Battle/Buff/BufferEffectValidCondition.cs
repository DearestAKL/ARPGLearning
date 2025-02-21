namespace GameMain.Runtime
{
    public interface IBufferEffectValidCondition
    {
        public IBufferEffect BufferEffect { get; }
        
        bool CheckCondition();
        void Tick(float deltaTime);
        void TakeEffect();
    }

    public abstract class ABufferEffectValidCondition : IBufferEffectValidCondition
    {
        public IBufferEffect BufferEffect { get; }
        
        public virtual bool CheckCondition() { return false; }
        
        public virtual void Tick(float deltaTime) { }
        
        public virtual void TakeEffect() { }
        
        protected ABufferEffectValidCondition(IBufferEffect bufferEffect)
        {
            BufferEffect = bufferEffect;
        }
    }
    
    public class TimeIntervalBufferEffectValidCondition : ABufferEffectValidCondition
    {
        private readonly float _intervalTime;
        private float _elapsedTime;
        private bool _isCooling = false;
        
        public TimeIntervalBufferEffectValidCondition(IBufferEffect bufferEffect,float interval)            
            : base(bufferEffect)
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
    
    public class AttributeBufferEffectValidCondition : ABufferEffectValidCondition
    {
        private readonly int _value;
        private readonly bool _isLessThan;
        
        private readonly IPropertyData _propertyData; 
        
        public AttributeBufferEffectValidCondition(IBufferEffect bufferEffect,AttributeType attributeType,int attributeValue,bool isLessThan)
            : base(bufferEffect)
        {
            var type = attributeType;
            _value = attributeValue;
            _isLessThan = isLessThan;
            
            _propertyData = bufferEffect.Buffer.Accessor.Condition.GetConditionPropertyData(type);
        }
        
        public override bool CheckCondition()
        {
            var compareValue = _propertyData.CompareToValue(_value);
            return _isLessThan ? compareValue < 0 : compareValue >= 0;
        }
    }
}