namespace GameMain.Runtime
{
    public interface IPropertyData
    {
        void AddModifier(IPropertyModifier modifier);
        void RemoveModifier(IPropertyModifier modifier);
        void RecalculateTotalValue();
        
        /// <summary>
        /// 比较 每种PropertyData类型 可能会有不同的策略
        /// </summary>
        int CompareToValue(int value);
    }
}