namespace GameMain.Runtime
{
    public interface IPropertyModifier
    {
        float Apply(PropertyFlatAndPercentageData propertyFlatAndPercentageData);
        float Apply(PropertyPercentageData propertyPercentageData);
        float Apply(PropertyHpData propertyPercentageData);

        bool UpdateValue(float value);
    }
}