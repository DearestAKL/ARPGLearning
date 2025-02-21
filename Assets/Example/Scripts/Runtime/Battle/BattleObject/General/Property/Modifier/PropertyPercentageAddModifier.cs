using System;

namespace GameMain.Runtime
{
    public class PropertyPercentageAddModifier : IPropertyModifier
    {
        private float _value;
        
        public PropertyPercentageAddModifier(float value)
        {
            _value = value;
        }
        
        public float Apply(PropertyFlatAndPercentageData propertyFlatAndPercentageData)
        {
            return propertyFlatAndPercentageData.AdditionValue + propertyFlatAndPercentageData.BaseValue * (_value / 100f);
        }

        public float Apply(PropertyPercentageData propertyPercentageData)
        {
            return propertyPercentageData.TotalPercent + _value;
        }

        public float Apply(PropertyHpData propertyHpData)
        {
            return propertyHpData.AdditionMaxValue + propertyHpData.BaseMaxValue * (_value / 100f);
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
    
    public class PropertyPercentageSubModifier : IPropertyModifier
    {
        private float _value;
        
        public PropertyPercentageSubModifier(float value)
        {
            _value = value;
        }
        
        public float Apply(PropertyFlatAndPercentageData propertyFlatAndPercentageData)
        {
            return propertyFlatAndPercentageData.AdditionValue - propertyFlatAndPercentageData.BaseValue *(_value / 100f);
        }

        public float Apply(PropertyPercentageData propertyPercentageData)
        {
            return propertyPercentageData.TotalPercent - _value;
        }

        public float Apply(PropertyHpData propertyHpData)
        {
            return propertyHpData.AdditionMaxValue - propertyHpData.BaseMaxValue * (_value / 100f);
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