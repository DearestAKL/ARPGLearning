using System.Collections.Generic;
using System.Linq;

namespace GameMain.Runtime
{
    public enum FilterCategoryType
    {
        WeaponType,
        WeaponQuality,
        
        ArmorType,
        ArmorQuality,
    }

    public enum FilterUIType
    {
        None,
        Weapon,
        Armor,
    }

    public sealed class UIFilterCategory
    {
        public FilterCategoryType FilterCategory;
        
        public string CategoryNameLabelId;
        
        public List<UIFilterTerm> TermList;
        
        public uint[] CurrentSelectedTermValues => CreateSelectedTermValues();
        
        public UIFilterCategory(FilterCategoryType filterCategory, string categoryNameLabelId, List<UIFilterTerm> termList)
        {
            FilterCategory      = filterCategory;
            CategoryNameLabelId = categoryNameLabelId;
            TermList            = termList;
        }

        private uint[] CreateSelectedTermValues()
        {
            var result = new List<uint>(TermList.Count);

            foreach (var term in TermList)
            {
                if (term.IsSelected)
                {
                    result.Add(term.TypeValue);
                }
            }

            return result.ToArray();
        }

        public void Reset()
        {
            foreach (var term in TermList)
            {
                term.Reset();
            }
        }

        public void Decide()
        {
            foreach (var term in TermList)
            {
                term.Decide();
            }
        }

        public void Clear()
        {
            foreach (var term in TermList)
            {
                term.Clear();
            }
        }
    }

    public sealed class UIFilterTerm
    {
        public FilterCategoryType FilterCategory { get; }
        
        public uint TypeValue { get; }
        public bool IsSelected { get; private set; }
        
        public string IconPath { get; }
        public string LabelId { get; }

        private bool _initIsSelected;

        public UIFilterTerm(FilterCategoryType filterCategory, uint enumValue, string iconPath,string labelId)
        {
            FilterCategory  = filterCategory;
            TypeValue       = enumValue;
            IsSelected      = false;
            _initIsSelected = false;
            IconPath        = iconPath;
            LabelId        = labelId;
        }
        
        public void SwitchSelectedCondition(bool flag)
        {
            IsSelected = flag;
        }

        public void ToggleSelectedCondition()
        {
            IsSelected = !IsSelected;
        }

        public void Reset()
        {
            IsSelected = _initIsSelected;
        }

        public void Decide()
        {
            _initIsSelected = IsSelected;
        }

        public void Clear()
        {
            IsSelected = false;
        }
    }

    public sealed class UIFilterParam
    {
        public List<UIFilterCategory> CategoryList; 
        public FilterUIType UIType { get; private set; }

        public bool IsAnyFiltered => CategoryList.Any(c => c.TermList.Any(t => t.IsSelected));
        
        public UIFilterParam(List<UIFilterCategory> categories, FilterUIType uiType)
        {
            CategoryList = categories;
            UIType   = uiType;
        }
        
        public void Reset()
        {
            foreach (var category in CategoryList)
            {
                category.Reset();
            }
        }

        public void Decide()
        {
            foreach (var category in CategoryList)
            {
                category.Decide();
            }
        }

        public void Clear()
        {
            foreach (var category in CategoryList)
            {
                category.Clear();
            }
        }
    }
}