namespace GameMain.Runtime
{
    public static class PropertyModifierHelper
    {
        public static IPropertyModifier CreatPropertyModifier(bool isPercentage, int value)
        {
            if (isPercentage)
            {
                return value >= 0 ? new PropertyPercentageAddModifier(value) : new PropertyPercentageSubModifier(-value);
            }
            else
            {
                return value >= 0 ? new PropertyFixedAddModifier(value) : new PropertyFixedSubModifier(-value);
            }
        }
    }
}