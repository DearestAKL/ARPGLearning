using System.Collections.Generic;

namespace GameMain.Runtime
{
    public sealed class PropertyPercentageData : IPropertyData
    {
        private float _basePercent;
        private float _totalPercent;
        private List<IPropertyModifier> _modifiers = new List<IPropertyModifier>();
        public float TotalPercent => _totalPercent;
        public float TotalValue => _totalPercent / 100f;

        public PropertyPercentageData(float basePercent)
        {
            _basePercent = basePercent;
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
            _totalPercent = _basePercent;
            
            foreach (var modifier in _modifiers)
            {
                _totalPercent = modifier.Apply(this);
            }
        }

        public int CompareToValue(int value)
        {
            return TotalPercent.CompareTo(value);
        }
        
        public void UpdateBaseData(float basePercent)
        {
            _basePercent = basePercent;
            RecalculateTotalValue();
        }
    }
}