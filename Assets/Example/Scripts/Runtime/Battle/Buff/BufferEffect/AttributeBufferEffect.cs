namespace GameMain.Runtime
{
    /// <summary>
    /// 改变属性-感觉基础数值与来源(持续效果)
    /// </summary>
    public class AttributeBufferEffect : ABufferEffect
    {
        //1.固定属性
        //2.百分比属性
        //3.来源对象百分比的属性
        
        //TODO: 以及来源对象（buffer来源或者拥有者）
        
        private readonly AttributeType _attributeType;//改变属性
        private readonly int _attributeValue;//改变属性值
        private readonly bool _isPercentage;//是否是百分比
        
        private readonly AttributeType _sourceType;//来源属性
        private readonly bool _isUsedSourceType;//是否使用来源属性
        private readonly bool _isUsedBufferSource;//是否使用Buffer来源者的属性

        private IPropertyData _curPropertyData;
        private IPropertyModifier _curPropertyModifier;


        public static AttributeBufferEffect Create(Buffer buffer, BufferDefinitionMessage message, BufferEffectDefinitionMessage effectMessage)
        {
            var attribute = effectMessage.Attribute;
            var attributeValue = PbDefinitionHelper.GetNumericalMessage(message, attribute.AttributeIndex);
            var bufferEffect = new AttributeBufferEffect(buffer, (BufferEffectTriggerType)effectMessage.TriggerType,
                (AttributeType)attribute.AttributeType, attributeValue, attribute.IsPercentage,
                (AttributeType)attribute.SourceType, attribute.IsUsedSourceType, attribute.IsUsedBufferSource);

            return bufferEffect;
        }

        public AttributeBufferEffect(Buffer buffer,BufferEffectTriggerType triggerType,
            AttributeType attributeType,int attributeValue,bool isPercentage,
            AttributeType sourceType,bool isUsedSourceType,bool isUsedBufferSource)
            : base(buffer,triggerType)
        {
            _attributeType = attributeType;
            _attributeValue = attributeValue;
            _isPercentage = isPercentage;
            
            _sourceType = sourceType;
            _isUsedSourceType = isUsedSourceType;
            _isUsedBufferSource = isUsedBufferSource;
        }
        
        protected override void Apply()
        {
            base.Apply();
            if (_curPropertyData == null) 
            {
                _curPropertyData = Buffer.Accessor.Condition.GetConditionPropertyData(_attributeType);
            }
            
            
            if (_curPropertyModifier == null)
            {
                if (_isUsedSourceType)
                {
                    var target = _isUsedBufferSource ? Buffer.Source : Buffer.Accessor;
                    var sourceValue = target.Condition.GetSourceValue(_sourceType, _attributeValue) * Buffer.Overlay;
                    _curPropertyModifier = PropertyModifierHelper.CreatPropertyModifier(false, (int)sourceValue);
                }
                else
                {
                    _curPropertyModifier = PropertyModifierHelper.CreatPropertyModifier(_isPercentage, _attributeValue * Buffer.Overlay);
                }
            }

            _curPropertyData.AddModifier(_curPropertyModifier);
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            
            //来源属性可能会发生变化 所以需要持续更新
            if (IsValid && _isUsedSourceType)
            {
                var target = _isUsedBufferSource ? Buffer.Source : Buffer.Accessor;
                var sourceValue = target.Condition.GetSourceValue(_sourceType, _attributeValue) * Buffer.Overlay;
                if (_curPropertyModifier != null && _curPropertyModifier.UpdateValue((int)sourceValue))
                {
                    _curPropertyData?.RecalculateTotalValue();
                }
            }
        }

        public override void ChangeOverlay()
        {
            if (_curPropertyModifier != null)
            {
                float newValue;
                if (_isUsedSourceType)
                {
                    var target = _isUsedBufferSource ? Buffer.Source : Buffer.Accessor;
                    newValue = target.Condition.GetSourceValue(_sourceType, _attributeValue) * Buffer.Overlay;
                }
                else
                {
                    newValue = _attributeValue * Buffer.Overlay;
                }
                
                if (_curPropertyModifier.UpdateValue(newValue))
                {
                    _curPropertyData?.RecalculateTotalValue();
                }
            }
        }

        public override void Remove()
        {
            base.Remove();
            _curPropertyData?.RemoveModifier(_curPropertyModifier);
        }
    }
}