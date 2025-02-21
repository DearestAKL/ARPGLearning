using System.Collections.Generic;
using System.Linq;
using System;
using cfg;
using UnityEngine;

namespace GameMain.Runtime
{
    public static class UIWeaponSortFilterHelper
    {
        private delegate int WeaponSortMethod(UIWeaponData x, UIWeaponData y);

        private static readonly Dictionary<int, WeaponSortMethod> WeaponSortDict = new Dictionary<int, WeaponSortMethod>
        {
            {(int)SortCategoryType.WeaponLevel, CompareWeaponLevel},
            {(int)SortCategoryType.WeaponQuality, CompareWeaponQuality},
            {(int)SortCategoryType.WeaponType, CompareWeaponType},
        };

        private static int Compare(int x,int y)
        {
            return x.CompareTo(y);
        }
        
        //type=>level=>quality=>id
        private static int CompareWeaponType(UIWeaponData x, UIWeaponData y)
        {
            var compare = -Compare((int)x.Config.Type, (int)y.Config.Type);
            return compare != 0 ? compare : CompareWeaponDef(x, y, SortCategoryType.WeaponType);
        }
        
        //level=>quality=>type=>id
        private static int CompareWeaponLevel(UIWeaponData x, UIWeaponData y)
        {
            var compare = -Compare(x.Level, y.Level);
            return compare != 0 ? compare : CompareWeaponDef(x, y, SortCategoryType.WeaponLevel);
        }
        
        //quality=>leve=>type=>id
        private static int CompareWeaponQuality(UIWeaponData x, UIWeaponData y)
        {
            var compare = -Compare(x.Config.Quality, y.Config.Quality);
            return compare != 0 ? compare : CompareWeaponDef(x, y, SortCategoryType.WeaponQuality);
        }
        
        //默认顺序 如果对于sortType将被忽略
        //level=>quality=>type=>id
        private static int CompareWeaponDef(UIWeaponData x, UIWeaponData y, SortCategoryType sortCategoryType)
        {
            var compare = 0;
            compare = sortCategoryType == SortCategoryType.WeaponLevel ? 0 : -Compare(x.Level, y.Level);
            if (compare == 0)
            {
                compare = sortCategoryType == SortCategoryType.WeaponQuality ? 0 :-Compare(x.Config.Quality, y.Config.Quality);
                if (compare == 0)
                {
                    compare = sortCategoryType == SortCategoryType.WeaponType ? 0 :-Compare((int)x.Config.Type, (int)y.Config.Type);
                    if (compare == 0)
                    {
                        compare = -Compare(x.Config.Id, y.Config.Id);
                    }
                }
            }

            return compare;
        }
        
        public static List<T> SortWeapon<T>(
            IEnumerable<T> targetList,
            IUISortCoreParam sortParam)
            where T : UIWeaponData
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

                var sortMethod = WeaponSortDict[sortParam.SortTypeValue];
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
                new((int)SortCategoryType.WeaponLevel, "等级顺序"),
                new((int)SortCategoryType.WeaponQuality, "品质顺序"),
                new((int)SortCategoryType.WeaponType, "类型顺序"),
            };

            return new UISortDef(list, SortUIType.Weapon);
        }
        
        public static UISortParam CreateSortDefaultParam()
        {
            return new UISortParam(
                false,
                (int)SortCategoryType.WeaponLevel,
                SortUIType.Weapon
            );
        }


        #region Filter
        public static List<T> FilterWeapon<T>(
            IEnumerable<T> targetList,
            UIFilterParam filterParam)
            where T : UIWeaponData
        {
#if UNITY_EDITOR
            //using (new RyProfileScope("UIHomeSortFilterHelper.FilterEquipment"))
#endif
            {
                var filtered = FilterWeaponInternal(targetList, filterParam);
                if (filtered is List<T> list)
                {
                    return list;
                }
                return filtered.ToList();
            }
        }

        private static IEnumerable<T> FilterWeaponInternal<T>(
            IEnumerable<T> list,
            UIFilterParam filterParam) where T : UIWeaponData
        {
            if (filterParam == null)
            {
                return list;
            }
            
            var mapper = new UIWeaponFilterFlagMapper();
            mapper.Initialize();
            
            var categories = filterParam.CategoryList;
            
            var rareCategory = categories.Find(r => r.FilterCategory == FilterCategoryType.WeaponType);
            var rareFlags    = mapper.ToWeaponTypeFlags(rareCategory.CurrentSelectedTermValues);
            var filtered     = FilterWeaponType(list, rareFlags, mapper);

            var rankCategory = categories.Find(r => r.FilterCategory == FilterCategoryType.WeaponQuality);
            var rankFlags    = mapper.ToWeaponQualityFlags(rankCategory.CurrentSelectedTermValues);
            filtered = FilterWeaponQuality(filtered, rankFlags, mapper);

            return filtered;
        }
        
        private static IEnumerable<T> FilterWeaponType<T>(IEnumerable<T> list,
            ulong filterFlags, UIWeaponFilterFlagMapper mapper) where T : UIWeaponData
        {
            if (filterFlags == AUIFilterFlagMapper.AllFlags)
            {
                return list;
            }

            return list.Where(e => (filterFlags & mapper.ToWeaponTypeFlag(e.Config.Type)) != 0);
        }
        
        private static IEnumerable<T> FilterWeaponQuality<T>(IEnumerable<T> list,
            ulong filterFlags, UIWeaponFilterFlagMapper mapper) where T : UIWeaponData
        {
            if (filterFlags == AUIFilterFlagMapper.AllFlags)
            {
                return list;
            }

            return list.Where(e => (filterFlags & mapper.ToWeaponQualityFlag(e.Config.Quality)) != 0);
        }
        
        public static UIFilterParam CreateFilterParam()
        {
            var weaponTypeCategory    = CreateFilterCategory(FilterCategoryType.WeaponType);
            var weaponQualityCategory = CreateFilterCategory(FilterCategoryType.WeaponQuality);

            return new UIFilterParam(new List<UIFilterCategory>
                {
                    weaponTypeCategory,
                    weaponQualityCategory
                },
                FilterUIType.Weapon);
        }

        private static UIFilterCategory CreateFilterCategory(FilterCategoryType type)
        {
            if (type == FilterCategoryType.WeaponType)
            {
                return new UIFilterCategory(FilterCategoryType.WeaponType, "类型", CreateFilterTermList(type));
            }
            else if(type == FilterCategoryType.WeaponQuality)
            {
                return new UIFilterCategory(FilterCategoryType.WeaponQuality, "品质", CreateFilterTermList(type));
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private static List<UIFilterTerm> CreateFilterTermList(FilterCategoryType type)
        {
            List<UIFilterTerm> result;
            
            if (type == FilterCategoryType.WeaponType)
            {
                result = new List<UIFilterTerm>()
                {
                    new UIFilterTerm(FilterCategoryType.WeaponType,(uint)WeaponType.Sword,"Sword","单手剑"),
                    new UIFilterTerm(FilterCategoryType.WeaponType,(uint)WeaponType.Bow,"Bow","弓"),
                    new UIFilterTerm(FilterCategoryType.WeaponType,(uint)WeaponType.Spear,"Spear","长枪"),
                    new UIFilterTerm(FilterCategoryType.WeaponType,(uint)WeaponType.MagicWand,"MagicWand","魔杖")
                };
            }
            else if(type == FilterCategoryType.WeaponQuality)
            {
                result = new List<UIFilterTerm>()
                {
                    new UIFilterTerm(FilterCategoryType.WeaponQuality,1,"1Star","1星"),
                    new UIFilterTerm(FilterCategoryType.WeaponQuality,2,"2Star","2星"),
                    new UIFilterTerm(FilterCategoryType.WeaponQuality,3,"3Star","3星"),
                    new UIFilterTerm(FilterCategoryType.WeaponQuality,4,"4Star","4星"),
                    new UIFilterTerm(FilterCategoryType.WeaponQuality,5,"5Star","5星")
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