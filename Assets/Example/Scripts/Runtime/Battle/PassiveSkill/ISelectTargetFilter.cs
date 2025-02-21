namespace GameMain.Runtime
{
    public interface ISelectTargetFilter
    {
        SelectTargetFilterType FilterType { get; }
        bool PassesFilter(IBattleCharacterAccessorComponent accessor);
    }

    public class AttributeFilter : ISelectTargetFilter
    {
        public SelectTargetFilterType FilterType => SelectTargetFilterType.Attribute;
        
        private readonly AttributeType _type;
        private readonly int _value;
        private readonly bool _isLessThan;
        
        public AttributeFilter(AttributeType attributeType,int attributeValue,bool isLessThan)
        {
            _type = attributeType;
            _value = attributeValue;
            _isLessThan = isLessThan;
        }

        public bool PassesFilter(IBattleCharacterAccessorComponent accessor)
        {
            var propertyData = accessor.Condition.GetConditionPropertyData(_type);
            var compareValue = propertyData.CompareToValue(_value);
            return _isLessThan ? compareValue < 0 : compareValue >= 0;
        }
    }
}