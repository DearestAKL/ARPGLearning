using System.Collections.Generic;

namespace GameMain.Runtime
{
    public enum SortCategoryType
    {
        WeaponLevel,
        WeaponQuality,
        WeaponType,
        
        ArmorLevel,
        ArmorQuality,
        ArmorType,
    }
    
    public enum SortUIType
    {
        None,
        Weapon,
        Armor,
    }
    
    public sealed class UISortCategory
    {
        public readonly int SortTypeValue;
        
        public readonly string TextLabelId;

        public UISortCategory(int sortTypeValue, string textLabelId)
        {
            TextLabelId   = textLabelId;
            SortTypeValue = sortTypeValue;
        }
    }
    
    public sealed class UISortDef
    {
        public readonly IReadOnlyList<UISortCategory> SortCategoryList;
        
        public readonly SortUIType UIType;

        public UISortDef(List<UISortCategory> categoryList, SortUIType type)
        {
            SortCategoryList = categoryList;
            UIType       = type;
        }
    }
    
    public interface IUISortCoreParam
    {
        bool IsReverseOrder { get; }
        int SortTypeValue { get; }
    }
    
    public sealed class UISortCoreParam : IUISortCoreParam
    {
        public bool IsReverseOrder { get; set; }
        public int  SortTypeValue  { get; set; }

        public UISortCoreParam(bool isReverse, int sortTypeValue)
        {
            IsReverseOrder = isReverse;
            SortTypeValue  = sortTypeValue;
        }

        public UISortCoreParam Copy()
        {
            return new UISortCoreParam(IsReverseOrder, SortTypeValue);
        }

        public void CopyFrom(IUISortCoreParam other)
        {
            IsReverseOrder = other.IsReverseOrder;
            SortTypeValue  = other.SortTypeValue;
        }
    }
    
    public sealed class UISortParam : IUISortCoreParam
    {
        private UISortCoreParam mInitParam;
        private UISortCoreParam mCurrentParam;
        
        public bool IsReverseOrder => mCurrentParam.IsReverseOrder;
        
        public int SortTypeValue => mCurrentParam.SortTypeValue;
        
        public SortUIType UIType { get; }

        public UISortParam(bool isReverse, int currentSortTypeValue, SortUIType type)
        {
            mInitParam    = new UISortCoreParam(isReverse, currentSortTypeValue);
            mCurrentParam = mInitParam.Copy();
            UIType    = type;
        }

        public void ChangeSortType(int value)
        {
            mCurrentParam.SortTypeValue = value;
        }

        public void ToggleSortOrder()
        {
            SetSortOrder(!IsReverseOrder);
        }

        public void SetSortOrder(bool isReverse)
        {
            mCurrentParam.IsReverseOrder = isReverse;
        }
        
        public void Reset()
        {
            mCurrentParam.CopyFrom(mInitParam);
        }
        
        public void Decide()
        {
            mInitParam.CopyFrom(mCurrentParam);
        }
    }
}