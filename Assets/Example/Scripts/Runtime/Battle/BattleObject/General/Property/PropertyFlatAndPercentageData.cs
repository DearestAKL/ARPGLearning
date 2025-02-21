using System;
using System.Collections.Generic;
using Akari.GfCore;

namespace GameMain.Runtime
{
    public sealed class PropertyFlatAndPercentageData : IPropertyData
    {
        private float _baseValue;
        private float _additionValue;
        private float _baseBonus;
        private List<IPropertyModifier> _modifiers = new List<IPropertyModifier>();
        
        public float BaseValue => _baseValue;//基础属性
        public float AdditionValue => _additionValue;//加成属性
        public float TotalValue => _baseValue + _additionValue;//总属性
        
        public int TotalShowValue => GfMathf.CeilToInt(TotalValue);//血量展示向上取整
        public int AdditionShowValue => GfMathf.CeilToInt(AdditionValue);//血量展示向上取整

        public PropertyFlatAndPercentageData(float baseValue,float baseBonus = 0f)
        {
            _baseValue = baseValue;
            _baseBonus = baseBonus;
            RecalculateTotalValue();
        }

        public void AddModifier(IPropertyModifier modifier)
        {
            _modifiers.Add(modifier);
            RecalculateTotalValue();
        }

        public void RemoveModifier(IPropertyModifier modifier)
        {
            _modifiers.Remove(modifier);
            RecalculateTotalValue();
        }
        
        public void RecalculateTotalValue()
        {
            _additionValue = _baseValue * _baseBonus / 100f;
            foreach (var modifier in _modifiers)
            {
                _additionValue = modifier.Apply(this);
            }
        }
        
        public int CompareToValue(int value)
        {
            return TotalValue.CompareTo(value);
        }
        
        
        public void UpdateBaseData(float baseValue,float baseBonus = 0f)
        {
            _baseValue = baseValue;
            _baseBonus = baseBonus;
            RecalculateTotalValue();
        }
    }
}