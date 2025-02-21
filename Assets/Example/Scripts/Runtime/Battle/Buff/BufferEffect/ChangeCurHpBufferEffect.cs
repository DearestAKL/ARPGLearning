namespace GameMain.Runtime
{
    /// <summary>
    /// 恢复或者扣血(瞬间效果)
    /// </summary>
    public class ChangeCurHpBufferEffect : ABufferEffect
    {
        private readonly AttributeType _sourceAttributeType;
        private readonly int _changeFixedValue;
        private readonly int _changePercentageValue;
        private readonly bool _isAdd;
        
        private readonly bool _isUsedBufferSource;//是否使用Buffer来源者的属性

        public static ChangeCurHpBufferEffect Create(Buffer buffer, BufferDefinitionMessage message, BufferEffectDefinitionMessage effectMessage)
        {
            var changeCurHp = effectMessage.ChangeCurHp;
            var changeFixedValue = PbDefinitionHelper.GetNumericalMessage(message, changeCurHp.FixedValueIndex);
            var changePercentageValue = PbDefinitionHelper.GetNumericalMessage(message, changeCurHp.PercentageValueIndex);
            var bufferEffect = new ChangeCurHpBufferEffect(buffer,
                (BufferEffectTriggerType)effectMessage.TriggerType, (AttributeType)changeCurHp.SourceType,
                changeFixedValue, changePercentageValue, changeCurHp.IsAdd, changeCurHp.IsUsedBufferSource);

            return bufferEffect;
        }
        
        public ChangeCurHpBufferEffect(Buffer buffer,BufferEffectTriggerType triggerType,
            AttributeType attributeType,int changeFixedFixedValue,int changePercentageValue,bool isAdd,
            bool isUsedBufferSource)
            : base(buffer,triggerType)
        {
            _sourceAttributeType = attributeType;
            _changeFixedValue = changeFixedFixedValue;
            _changePercentageValue = changePercentageValue;
            _isAdd = isAdd;
            _isUsedBufferSource = isUsedBufferSource;
        }

        protected override void Apply()
        {
            base.Apply();
            
            float changeValue = _changeFixedValue;
            if (_changePercentageValue > 0)
            {   
                var target = _isUsedBufferSource ? Buffer.Source : Buffer.Accessor;
                changeValue = target.Condition.GetSourceValue(_sourceAttributeType, _changePercentageValue);
            }
            
            if (_isAdd)
            {
                BattleAdmin.DamageHandler.HandleSimpleDamage(Buffer.Source.Entity.ThisHandle,
                    Buffer.Accessor.DamageNotificator, (int)changeValue,
                    AttackCategoryType.Heal);
            }
            else
            {
                BattleAdmin.DamageHandler.HandleSimpleDamage(Buffer.Source.Entity.ThisHandle,
                    Buffer.Accessor.DamageNotificator, (int)changeValue,
                    AttackCategoryType.Damage, DamageViewType.Hide);
            }
        }
    }
}