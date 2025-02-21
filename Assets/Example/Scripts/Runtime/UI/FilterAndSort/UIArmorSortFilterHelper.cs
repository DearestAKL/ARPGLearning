using System.Collections.Generic;
using System.Linq;
using System;
using cfg;
using UnityEngine;

namespace GameMain.Runtime
{
    public static class UIArmorSortFilterHelper
    {
        private delegate int ArmorSortMethod(UIArmorData x, UIArmorData y);

        private static readonly Dictionary<int, ArmorSortMethod> ArmorSortDict = new Dictionary<int, ArmorSortMethod>
        {
            {(int)SortCategoryType.ArmorLevel, CompareArmorLevel},
            {(int)SortCategoryType.ArmorQuality, CompareArmorQuality},
            {(int)SortCategoryType.ArmorType, CompareArmorType},
        };

        private static int Compare(int x,int y)
        {
            return x.CompareTo(y);
        }
        
        //type=>level=>quality=>id
        private static int CompareArmorType(UIArmorData x, UIArmorData y)
        {
            var compare = -Compare((int)x.Config.Type, (int)y.Config.Type);
            return compare != 0 ? compare : CompareArmorDef(x, y, SortCategoryType.ArmorType);
        }
        
        //level=>quality=>type=>id
        private static int CompareArmorLevel(UIArmorData x, UIArmorData y)
        {
            var compare = -Compare(x.Level, y.Level);
            return compare != 0 ? compare : CompareArmorDef(x, y, SortCategoryType.ArmorLevel);
        }
        
        //quality=>leve=>type=>id
        private static int CompareArmorQuality(UIArmorData x, UIArmorData y)
        {
            var compare = -Compare(x.Config.Quality, y.Config.Quality);
            return compare != 0 ? compare : CompareArmorDef(x, y, SortCategoryType.ArmorQuality);
        }
        
        //默认顺序 如果对于sortType将被忽略
        //level=>quality=>type=>id
        private static int CompareArmorDef(UIArmorData x, UIArmorData y, SortCategoryType sortCategoryType)
        {
            var compare = 0;
            compare = sortCategoryType == SortCategoryType.ArmorLevel ? 0 : -Compare(x.Level, y.Level);
            if (compare == 0)
            {
                compare = sortCategoryType == SortCategoryType.ArmorQuality ? 0 :-Compare(x.Config.Quality, y.Config.Quality);
                if (compare == 0)
                {
                    compare = sortCategoryType == SortCategoryType.ArmorType ? 0 :-Compare((int)x.Config.Type, (int)y.Config.Type);
                    if (compare == 0)
                    {
                        compare = -Compare(x.Config.Id, y.Config.Id);
                    }
                }
            }

            return compare;
        }
        
        public static List<T> SortArmor<T>(
            IEnumerable<T> targetList,
            IUISortCoreParam sortParam)
            where T : UIArmorData
        {
#if UNITY_EDITOR
            //using (new RyProfileScope("UIHomeSortFilterHelper.SortEquipment"))
#endif
            {
                var sortedList = targetList.ToList();
                if (sortParam == null)
                {
                    return sortedList;
                }

                var sortMethod = ArmorSortDict[sortParam.SortTypeValue];
                if (!sortParam.IsReverseOrder)
                {
                    sortedList.Sort((x, y) => sortMethod.Invoke(x, y));
                }
                else
                {
                    sortedList.Sort((x, y) => -sortMethod.Invoke(x, y));
                }

                return sortedList;
            }
        }

        public static UISortDef CreateSortDef()
        {
            var list = new List<UISortCategory>
            {
                new((int)SortCategoryType.ArmorLevel, "等级顺序"),
                new((int)SortCategoryType.ArmorQuality, "品质顺序"),
                new((int)SortCategoryType.ArmorType, "类型顺序"),
            };

            return new UISortDef(list, SortUIType.Armor);
        }
        
        public static UISortParam CreateSortDefaultParam()
        {
            return new UISortParam(
                false,
                (int)SortCategoryType.ArmorLevel,
                SortUIType.Armor
            );
        }


        #region Filter
        public static List<T> FilterArmor<T>(
            IEnumerable<T> targetList,
            UIFilterParam filterParam)
            where T : UIArmorData
        {
#if UNITY_EDITOR
            //using (new RyProfileScope("UIHomeSortFilterHelper.FilterEquipment"))
#endif
            {
                var filtered = FilterArmorInternal(targetList, filterParam);
                if (filtered is List<T> list)
                {
                    return list;
                }
                return filtered.ToList();
            }
        }

        private static IEnumerable<T> FilterArmorInternal<T>(
            IEnumerable<T> list,
            UIFilterParam filterParam) where T : UIArmorData
        {
            if (filterParam == null)
            {
                return list;
            }
            
            var mapper = new UIArmorFilterFlagMapper();
            mapper.Initialize();
            
            var categories = filterParam.CategoryList;
            
            var rareCategory = categories.Find(r => r.FilterCategory == FilterCategoryType.ArmorType);
            var rareFlags    = mapper.ToArmorTypeFlags(rareCategory.CurrentSelectedTermValues);
            var filtered     = FilterArmorType(list, rareFlags, mapper);

            var rankCategory = categories.Find(r => r.FilterCategory == FilterCategoryType.ArmorQuality);
            var rankFlags    = mapper.ToArmorQualityFlags(rankCategory.CurrentSelectedTermValues);
            filtered = FilterArmorQuality(filtered, rankFlags, mapper);

            return filtered;
        }
        
        private static IEnumerable<T> FilterArmorType<T>(IEnumerable<T> list,
            ulong filterFlags, UIArmorFilterFlagMapper mapper) where T : UIArmorData
        {
            if (filterFlags == AUIFilterFlagMapper.AllFlags)
            {
                return list;
            }

            return list.Where(e => (filterFlags & mapper.ToArmorTypeFlag(e.Config.Type)) != 0);
        }
        
        private static IEnumerable<T> FilterArmorQuality<T>(IEnumerable<T> list,
            ulong filterFlags, UIArmorFilterFlagMapper mapper) where T : UIArmorData
        {
            if (filterFlags == AUIFilterFlagMapper.AllFlags)
            {
                return list;
            }

            return list.Where(e => (filterFlags & mapper.ToArmorQualityFlag(e.Config.Quality)) != 0);
        }
        
        public static UIFilterParam CreateFilterParam()
        {
            var armorTypeCategory    = CreateFilterCategory(FilterCategoryType.ArmorType);
            var armorQualityCategory = CreateFilterCategory(FilterCategoryType.ArmorQuality);

            return new UIFilterParam(new List<UIFilterCategory>
                {
                    armorTypeCategory,
                    armorQualityCategory
                },
                FilterUIType.Armor);
        }

        private static UIFilterCategory CreateFilterCategory(FilterCategoryType type)
        {
            if (type == FilterCategoryType.ArmorType)
            {
                return new UIFilterCategory(FilterCategoryType.ArmorType, "类型", CreateFilterTermList(type));
            }
            else if(type == FilterCategoryType.ArmorQuality)
            {
                return new UIFilterCategory(FilterCategoryType.ArmorQuality, "品质", CreateFilterTermList(type));
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private static List<UIFilterTerm> CreateFilterTermList(FilterCategoryType type)
        {
            List<UIFilterTerm> result;
            
            if (type == FilterCategoryType.ArmorType)
            {
                result = new List<UIFilterTerm>()
                {
                    new UIFilterTerm(FilterCategoryType.ArmorType,(uint)ArmorType.Head,"Sword","头盔"),
                    new UIFilterTerm(FilterCategoryType.ArmorType,(uint)ArmorType.Chest,"Bow","上衣"),
                    new UIFilterTerm(FilterCategoryType.ArmorType,(uint)ArmorType.Waist,"Spear","腰带"),
                    new UIFilterTerm(FilterCategoryType.ArmorType,(uint)ArmorType.Legs,"MagicWand","下装"),
                    new UIFilterTerm(FilterCategoryType.ArmorType,(uint)ArmorType.Feet,"MagicWand","鞋子"),
                };
            }
            else if(type == FilterCategoryType.ArmorQuality)
            {
                result = new List<UIFilterTerm>()
                {
                    new UIFilterTerm(FilterCategoryType.ArmorQuality,1,"1Star","1星"),
                    new UIFilterTerm(FilterCategoryType.ArmorQuality,2,"2Star","2星"),
                    new UIFilterTerm(FilterCategoryType.ArmorQuality,3,"3Star","3星"),
                    new UIFilterTerm(FilterCategoryType.ArmorQuality,4,"4Star","4星"),
                    new UIFilterTerm(FilterCategoryType.ArmorQuality,5,"5Star","5星")
                };
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return result;
        }
        #endregion
    }
}