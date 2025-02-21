using Akari.GfCore;

namespace GameMain.Runtime
{
    public interface IBufferEffect
    {
        public Buffer Buffer { get; }
        void ChangeOverlay();
        void Tick(float deltaTime);
        
        void Trigger(BufferEffectTriggerType triggerType);

        void Init(IBufferEffectValidCondition[] validConditions);

        void Remove();
    }

    //触发时机，Buffer的Begin,Update,End，其它类型 OnHit，OnBeHit
    //生效条件，时间间隔，属性要求
    public abstract class ABufferEffect : IBufferEffect
    {
        public Buffer Buffer { get; }

        private BufferEffectTriggerType _triggerType;
        
        private IBufferEffectValidCondition[] _validConditions;//多个神效条件

        public bool IsValid { get; private set; } //持续类的Effect会用到，TriggerType为Update

        protected ABufferEffect(Buffer buffer,BufferEffectTriggerType triggerType)
        {
            Buffer = buffer;
            _triggerType = triggerType;

            if (_triggerType == BufferEffectTriggerType.OnHit)
            {
                buffer.Accessor.Entity.On<BattleDidCauseDamageRequest>(OnHit);
            }
        }

        public void Init(IBufferEffectValidCondition[] validConditions)
        {
            _validConditions = validConditions;
        }

        public virtual void ChangeOverlay()
        {
            //持续类的effect 可能会根据层数改变属性
        }

        public virtual void Tick(float deltaTime)
        {
            //validConditions 因为有时间类型条件 所以也需要Tick
            foreach (var condition in _validConditions)
            {
                condition.Tick(deltaTime);
            }

            if (_triggerType == BufferEffectTriggerType.OnUpdate)
            {
                if (IsValid)
                {
                    if (!CheckValidCondition())
                    {
                        //失效
                        Remove();
                    }
                }
                else
                {
                    if (CheckValidCondition())
                    {
                        //生效
                        Apply();
                    }
                }
            }
        }

        public void Trigger(BufferEffectTriggerType triggerType)
        {
            if (triggerType == _triggerType)
            {
                //检测是否生效
                if (CheckValidCondition())
                {
                    Apply();
                }
            }
        }
        
        public virtual void Remove()
        {
            IsValid = false;
        }

        protected virtual void Apply()
        {
            IsValid = true;
        }

        private bool CheckValidCondition()
        {
            foreach (var condition in _validConditions)
            {
                if (!condition.CheckCondition())
                {
                    return false;
                }
            }
            
            return true;
        }

        #region Request

        private void OnHit(in BattleDidCauseDamageRequest request)
        {
            if (request.DamageResult.AttackCategoryType == AttackCategoryType.Damage)
            {
                Trigger(BufferEffectTriggerType.OnHit);
            }
        }
        #endregion
    }

    /// <summary>
    /// 持续伤害(持续效果)
    /// </summary>
    public class DotBufferEffect : ABufferEffect
    {
        private readonly DotType _dotType;//dot类型
        private readonly int _attributeValue;//属性值
        public static DotBufferEffect Create(Buffer buffer, BufferDefinitionMessage message, BufferEffectDefinitionMessage effectMessage)
        {
            var attribute = effectMessage.Attribute;
            var attributeValue = PbDefinitionHelper.GetNumericalMessage(message, attribute.AttributeIndex);
            var bufferEffect = new DotBufferEffect(buffer, (BufferEffectTriggerType)effectMessage.TriggerType, attributeValue, DotType.Poison);

            return bufferEffect;
        }

        public DotBufferEffect(Buffer buffer,BufferEffectTriggerType triggerType,int attributeValue, 
            DotType dotType)
            : base(buffer,triggerType)
        {
            _attributeValue = attributeValue;
            _dotType = dotType;
        }
        
        protected override void Apply()
        {
            base.Apply();

            float damageValue = 0F;
            // if (_dotType == DotType.Poison)
            // {
            //     
            // }
            // else if (_dotType == DotType.Burn)
            // {
            //     
            // }
            // else if (_dotType == DotType.Bleed)
            // {
            //     
            // }
            
            //默认攻击力百分比
            damageValue = Buffer.Overlay * Buffer.Source.Condition.AttackProperty.TotalValue * _attributeValue / 100;
            
            BattleAdmin.DamageHandler.HandleDotDamage(Buffer.Source.Attack.DamageCauserHandler,
                Buffer.Accessor.DamageReceiver, Buffer.Accessor.DamageNotificator, damageValue);
        }
    }

    /// <summary>
    /// 根据已损失生命来提示属性(持续效果)
    /// </summary>
    public class AttributeByHealthLostBufferEffect : ABufferEffect
    {
        private readonly AttributeType _attributeType;//改变属性
        //每失去_healthLostValue%血量，属性提示_attributeValue%
        private readonly int _healthLostValue;//生命值损失百分比
        private readonly int _attributeValue;//改变属性值百分比
        
        private IPropertyData _curPropertyData;
        private IPropertyModifier _curPropertyModifier;
        
        public AttributeByHealthLostBufferEffect(Buffer buffer, BufferEffectTriggerType triggerType) : base(buffer, triggerType)
        {
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
                int count = GfMathf.FloorToInt(Buffer.Accessor.Condition.HpProperty.CurValueRatio * 100 / _healthLostValue);
                _curPropertyModifier = PropertyModifierHelper.CreatPropertyModifier(true, _attributeValue * count * Buffer.Overlay);
            }

            _curPropertyData.AddModifier(_curPropertyModifier);
        }
    }
}