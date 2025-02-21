using System;
using System.Collections.Generic;
using Akari.GfCore;

namespace GameMain.Runtime
{
    public sealed class PropertyHpData : IPropertyData
    {
        private float _baseMaxValue;
        private float _additionMaxValue;
        private float _baseBonus;
        private List<IPropertyModifier> _modifiers = new List<IPropertyModifier>();
        public float BaseMaxValue => _baseMaxValue;//基础属性
        public float AdditionMaxValue => _additionMaxValue;//加成属性
        public float TotalMaxValue => _baseMaxValue + _additionMaxValue;//总属性
        
        public float CurValue { get; private set; }
        

        //=====Show=====
        public int CurShowValue => GfMathf.CeilToInt(CurValue);//血量展示向上取整
        public int TotalMaxShowValue => GfMathf.CeilToInt(TotalMaxValue);//血量展示向上取整
        public int AdditionMaxShowValue => GfMathf.CeilToInt(AdditionMaxValue);//血量展示向上取整
        public float CurValueRatio => CurValue / TotalMaxShowValue;
        
        public bool  IsAlive        => CurShowValue > 0;

        public GfEvent<bool> OnValueChange = new GfEvent<bool>();

        /// <summary>
        /// 用于显示层更新，当受到Modifier导致改变是标记为True
        /// </summary>
        public bool IsViewDirty;

        public PropertyHpData(float baseMaxValue,float baseBonus = 0f)
        {
            _baseMaxValue = baseMaxValue;
            _baseBonus = baseBonus;
            RecalculateTotalValue();
            CurValue = TotalMaxValue;
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
            var oldAdditionMaxValue = _additionMaxValue;
            
            _additionMaxValue = _baseMaxValue * _baseBonus / 100f;
            
            foreach (var modifier in _modifiers)
            {
                _additionMaxValue = modifier.Apply(this);
            }

            var changeValue = _additionMaxValue - oldAdditionMaxValue;
            if (changeValue > 0.1f)
            {
                //最大生命值增加
                SetValue(CurValue + changeValue,true);
            }
            else if (changeValue < 0.1F && CurValue > TotalMaxValue)
            {
                //最大生命值减少 且当前生命大于最大生命值
                SetValue(CurValue, true);
            }
        }
        
        private void SetValue(float newValue,bool isAutoDirty)
        {
            var newCurValue = GfMathf.Clamp(newValue, 0, TotalMaxValue);
            if (GfMathf.Abs(CurValue - newCurValue) > 0.1f)
            {
                if (isAutoDirty)
                {
                    IsViewDirty = true;
                }
                //变化
                CurValue = newCurValue;
                OnValueChange.Invoke(newCurValue > CurValue);
            }
        }
        
        public void AddCurValue(float valueToAdd,bool isAutoDirty = false)
        {
            if (valueToAdd <= 0)
            {
                return;
            }
            SetValue(CurValue + valueToAdd,isAutoDirty);
        }

        public void SubtractCurValue(float valueToSubtract,bool isAutoDirty = false)
        {
            if (valueToSubtract <= 0)
            {
                return;
            }

            SetValue(CurValue - valueToSubtract, isAutoDirty);
        }
        
        public void SetCurValueFully(bool isAutoDirty = false)
        {
            SetValue(TotalMaxValue,isAutoDirty);
        }
        
        public int CompareToValue(int value)
        {
            return CurValueRatio.CompareTo(value / 100f);
        }

        public void UpdateBaseData(float baseMaxValue,float baseBonus = 0f)
        {
            _baseMaxValue = baseMaxValue;
            _baseBonus = baseBonus;
            
            RecalculateTotalValue();
        }
    }
}