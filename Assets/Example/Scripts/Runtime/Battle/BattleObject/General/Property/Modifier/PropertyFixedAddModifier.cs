using System;

namespace GameMain.Runtime
{
    public class PropertyFixedAddModifier : IPropertyModifier
    {
        private float _value;

        public PropertyFixedAddModifier(float value)
        {
            _value = value;
        }

        public float Apply(PropertyFlatAndPercentageData propertyFlatAndPercentageData)
        {
            return propertyFlatAndPercentageData.AdditionValue + _value;
        }

        public float Apply(PropertyPercentageData propertyPercentageData)
        {
            //百分比属性无法推荐固定值
            return propertyPercentageData.TotalPercent;
        }

        public float Apply(PropertyHpData propertyHpData)
        {
            return propertyHpData.AdditionMaxValue + _value;
        }

        public bool UpdateValue(float value)
        {
            if (Math.Abs(_value - value) < BattleDef.TOLERANCE)
            {
                return false;
            }
            _value = value;
            return true;
        }
    }
    
    public class PropertyFixedSubModifier : IPropertyModifier
    {
        private float _value;

        public PropertyFixedSubModifier(float value)
        {
            _value = value;
        }

        public float Apply(PropertyFlatAndPercentageData propertyFlatAndPercentageData)
        {
            return propertyFlatAndPercentageData.AdditionValue - _value;
        }

        public float Apply(PropertyPercentageData propertyPercentageData)
        {
            //百分比属性无法推荐固定值
            return propertyPercentageData.TotalPercent;
        }

        public float Apply(PropertyHpData propertyHpData)
        {
            return propertyHpData.AdditionMaxValue - _value;
        }

        public bool UpdateValue(float value)
        {
            if (Math.Abs(_value - value) < BattleDef.TOLERANCE)
            {
                return false;
            }
            _value = value;
            return true;
        }
    }
}