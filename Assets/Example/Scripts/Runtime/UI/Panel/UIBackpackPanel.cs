using System.Collections.Generic;
using System.Linq;
using Akari.GfCore;
using cfg;
using UnityEngine;

namespace GameMain.Runtime
{
    /// <summary>
    /// 背包界面
    /// </summary>
    public class UIBackpackPanel : UIBackpackPanelSign
    {
        public class Params
        {
            public TabType TabType;
            
            public Params(TabType tabType = TabType.Weapon)
            {
                TabType = tabType;
            }
        }
        
        public enum TabType
        {
            Weapon,//武器
            Armor,//防具
            Consumable,//消耗品
            Material,//材料
        }
        
        private List<UIWeaponData> _originalWeaponList; 
        private List<UIArmorData> _originalArmorList; 
        private List<UIItemData> _originalMaterialList; 
        private List<UIItemData> _originalConsumableList; 
        
        private Dictionary<int, List<AUIItemData>> _itemDataMap = new Dictionary<int, List<AUIItemData>>();
        
        private Dictionary<int, UISortDef> _sortDefDict;
        private Dictionary<int, UISortParam> _sortParamDict;
        private Dictionary<int, UIFilterParam> _filterParamDict;
        
        private readonly TabType[] _tabTypes = new TabType[]
        {
            TabType.Weapon,
            TabType.Armor,
            TabType.Consumable,
            TabType.Material,
        };
        
        private TabType _curType = TabType.Weapon;
        
        private int _curSelectIndex;
        
        private Params _data;
        
        private AkariLoopScroll<AUIItemData, UIItemCell> _akariLoopScroll;
        
        public override void OnInit(string name, GameObject go, Transform parent, object userData)
        {
             base.OnInit(name, go, parent, userData);
             
             btnClose.onClick.AddListener(Close);
             btnFunction.onClick.AddListener(OnFunction);
             
             _akariLoopScroll = new AkariLoopScroll<AUIItemData, UIItemCell>(itemScrollView, OnItemUpdateView);
             _akariLoopScroll.SetOnClickInfo(OnItemClick);

             toggleGroupEx.Clear();
             for (int i = 0; i < _tabTypes.Length; i++)
             {
                 toggleGroupEx.CreateToggleEx((int)_tabTypes[i]);
             }
             toggleGroupEx.OnToggleChanged.AddListener(OnToggleChanged);
             
             //sort and filter
             _sortDefDict = new Dictionary<int, UISortDef>()
             {
                 {(int)TabType.Weapon,UIWeaponSortFilterHelper.CreateSortDef()},
                 {(int)TabType.Armor,UIArmorSortFilterHelper.CreateSortDef()},
             };
             //排序条件 会将一项条件优先级提到最高
             var weaponSortParam = UISortFilterHelper.LoadSortParam(Constant.Setting.WeaponSortParamKey, _sortDefDict[(int)TabType.Weapon],UIWeaponSortFilterHelper.CreateSortDefaultParam());
             var amorSortParam = UISortFilterHelper.LoadSortParam(Constant.Setting.ArmorSortParamKey, _sortDefDict[(int)TabType.Armor],UIArmorSortFilterHelper.CreateSortDefaultParam());
             _sortParamDict = new Dictionary<int, UISortParam>()
             {
                 {(int)TabType.Weapon,weaponSortParam},
                 {(int)TabType.Armor,amorSortParam},
             };
             //筛选条件 可以存在多个条件
             var weaponFilterParam = UIWeaponSortFilterHelper.CreateFilterParam();
             var armorFilterParam = UIArmorSortFilterHelper.CreateFilterParam();
             _filterParamDict = new Dictionary<int, UIFilterParam>()         
             {
                 {(int)TabType.Weapon,weaponFilterParam},
                 {(int)TabType.Armor,armorFilterParam},
             };

             sortOptions.OnSort.AddListener(OnRefreshSort);
             btnFilter.onClick.AddListener(OnFilter);
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            
            if (userData is Params data)
            {
                _data = data;
                
                if (!toggleGroupEx.SetToggleOn((int)_data.TabType))
                {
                    //Toggle没有Changed 这里需要主动调用UpdateView
                    _curType = _data.TabType;
                    
                    UpdateScrollView();
                    UpdateView();
                }
                
                //重新打开时清空筛选条件
                foreach (var filterParam in _filterParamDict.Values)
                {
                    filterParam.Clear();
                }
            }
            else if(_data != null)
            {
                //TODO:记录当前的select情况
                
                //返回界面
                UpdateScrollView(true);
                UpdateView();
            }
        }

        private void UpdateView()
        {
            txtTitle.text = $"{UICommonLabel.Backpack.GetLocalization()}/{GetTitleLabel().GetLocalization()}";
            txtLimit.text = $"背包容量 {_akariLoopScroll.TotalCount}/{2000}";

            sortOptions.gameObject.SetActive(_curType == TabType.Weapon || _curType == TabType.Armor);
            if (_curType == TabType.Weapon || _curType == TabType.Armor)
            {
                sortOptions.UpdateView(_sortDefDict[(int)_curType], _sortParamDict[(int)_curType]);
            }
            
            btnFilter.gameObject.SetActive(_curType == TabType.Weapon || _curType == TabType.Armor);
        }
        
        private string GetTitleLabel()
        {
            switch (_curType)
            {
                case TabType.Material:
                    return UICommonLabel.Material;
                case TabType.Consumable:
                    return UICommonLabel.Consumable;
                case TabType.Weapon:
                    return UICommonLabel.Weapon;
                case TabType.Armor:
                    return UICommonLabel.Armor;
                default:
                    return string.Empty;
            }
        }
        
        private void UpdateScrollView(bool reOpen = false)
        {
            bool isDirty = false;
            
            //检测是否有数据需要更新,初始化时或者增减Item时
            if (_curType == TabType.Weapon)
            {
                if (_originalWeaponList == null || _originalWeaponList.Count != UserDataManager.Instance.Weapon.Num)
                {
                    var weaponList = UserDataManager.Instance.Weapon.GetList();
                    _originalWeaponList = weaponList.Select(i => new UIWeaponData(i)).ToList();
                    FilterAndSort();
                    isDirty = true;
                }
            }
            else if (_curType == TabType.Armor)
            {
                if (_originalArmorList == null || _originalArmorList.Count != UserDataManager.Instance.Armor.Num)
                {
                    var armorList = UserDataManager.Instance.Armor.GetList();
                    _originalArmorList = armorList.Select(i => new UIArmorData(i)).ToList();
                    FilterAndSort();
                    isDirty = true;
                }
            }
            else
            {
                if (_originalMaterialList == null 
                    || _originalConsumableList == null
                    || _originalMaterialList.Count + _originalConsumableList.Count 
                    != UserDataManager.Instance.Item.Num)
                {
                    //Item-(Material,Consumable)
                    var itemList = UserDataManager.Instance.Item.GetList();
                    var itemDataList = itemList.Select(i => new UIItemData(i));
                    //Item 默认按照Type分类，Id排序
                    _originalMaterialList = new List<UIItemData>();
                    _originalConsumableList = new List<UIItemData>();
                    foreach (var itemData in itemDataList)
                    {
                        if (itemData.ItemType == ItemType.Material)
                        {
                            _originalMaterialList.Add(itemData);
                        }
                        else if (itemData.ItemType == ItemType.Consumable)
                        {
                            _originalConsumableList.Add(itemData);
                        }
                    }
                    
                    //消耗品 和 材料默认按照id排序 无法筛选或者选择排序方式
                    _originalMaterialList.Sort((a, b) => a.Config.Id.CompareTo(b.Config.Id));
                    _originalConsumableList.Sort((a, b) => a.Config.Id.CompareTo(b.Config.Id));
                    
                    _itemDataMap[(int)TabType.Material] = new List<AUIItemData>(_originalMaterialList);
                    _itemDataMap[(int)TabType.Consumable] = new List<AUIItemData>(_originalConsumableList);
                }
            }

            if (reOpen && !isDirty)
            {
                //恢复界面时 如果元素数量未改变 则只需要刷新数据即可
                itemScrollView.RefillCells();
                return;
            }

            UpdateScrollViewData();
        }

        private void UpdateScrollViewData(int curSelectIndex = 0)
        {
            if (_itemDataMap.TryGetValue((int)_curType, out var curToggleItemData))
            {
                _akariLoopScroll.SetDataRefresh(curToggleItemData);
            }

            if (_akariLoopScroll.TotalCount > 0)
            {
                _curSelectIndex = Mathf.Min(curSelectIndex, _akariLoopScroll.TotalCount);
                _akariLoopScroll.OnClickItem(_curSelectIndex);
            }
        }

        private void OnItemUpdateView(int index, AUIItemData data, UIItemCell item, bool select)
        {
            item.UpdateView(data, select);
        }
        
        private void OnItemClick(int index, AUIItemData data, UIItemCell item, bool select)
        {
            item.UpdateSelect(select);

            if (select)
            {
                _curSelectIndex = index;
                UpdateItemInfo();
            }
        }
        
        private void UpdateItemInfo()
        {
            var curSelectItemData = GetCurSelectItemData();
            itemTips.gameObject.SetActive(curSelectItemData != null);
            btnFunction.gameObject.SetActive(curSelectItemData != null);
            goHasEquip.gameObject.SetActive(curSelectItemData != null);

            if (curSelectItemData != null)
            {
                if (curSelectItemData is UIItemData item)
                {
                    itemTips.UpdateView(item);
                    
                    goHasEquip.gameObject.SetActive(false);
                    
                    btnFunction.gameObject.SetActive(item.ItemType == ItemType.Consumable);
                    if (item.ItemType == ItemType.Consumable)
                    {
                        txtFunction.text = "使用";
                    }
                }
                else if (curSelectItemData is UIWeaponData weapon)
                {
                    itemTips.UpdateView(weapon);
                
                    goHasEquip.gameObject.SetActive(weapon.CharacterId != 0);
                
                    btnFunction.gameObject.SetActive(true);
                    txtFunction.text = "详情";
                }
                else if (curSelectItemData is UIArmorData armor)
                {
                    itemTips.UpdateView(armor);
                    
                    goHasEquip.gameObject.SetActive(armor.CharacterId != 0);
                
                    btnFunction.gameObject.SetActive(true);
                    txtFunction.text = "详情";
                }
            }
        }
        
        private void OnFunction()
        {
            var curSelectItemData = GetCurSelectItemData();
            
            if (curSelectItemData is UIWeaponData weapon)
            {
                //详情
            }
            else if (curSelectItemData is UIArmorData armor)
            {
                //详情
            }
            else if (curSelectItemData is UIItemData item && item.ItemType == ItemType.Consumable)
            {
                //使用
                UserDataManager.Instance.Item.UseItem(item.Config.Id, 1);

                if (!UserDataManager.Instance.Item.HasItem(item.Config.Id))
                {
                    //使用完了，这里需要同步更新
                    var curItemData = _itemDataMap[(int)_curType];
                    curItemData.Remove(curSelectItemData);
                    
                    if (_curSelectIndex >= curItemData.Count)
                    {
                        _curSelectIndex = curItemData.Count - 1;
                    }
                    UpdateScrollViewData(_curSelectIndex);
                }
                else
                {
                    //todo：只需要更新数量
                    _akariLoopScroll.RefillCells();
                }
            }
        }
        
        private void OnToggleChanged(int toggleIndex)
        {
            _curType = (TabType)toggleIndex;
            
            UpdateScrollView();
            UpdateView();
        }

        private void OnRefreshSort(UISortParam sortParam)
        {
            _sortParamDict[(int)_curType] = sortParam;
            FilterAndSort();

            UISortFilterHelper.SaveSortParam(Constant.Setting.WeaponSortParamKey, sortParam);

            UpdateScrollViewData();
        }
        
        private void OnRefreshFilter(UIFilterParam filterParam)
        {
            _filterParamDict[(int)_curType] = filterParam;
            FilterAndSort();
            UpdateScrollViewData();
        }

        private async void OnFilter()
        {
            //open Filter panel
            await UIManager.Instance.OpenUIPanel(UIType.UISideFilterDialog,new UISideFilterDialog.Params(_filterParamDict[(int)_curType],OnRefreshFilter));
        }

        private AUIItemData GetCurSelectItemData()
        {
            if (_akariLoopScroll.TotalCount <= 0)
            {
                return null;
            }
            _itemDataMap.TryGetValue((int)_curType, out var curToggleItemData);
            return curToggleItemData.IsEmpty() ? null : curToggleItemData[_curSelectIndex] as AUIItemData;
        }
        
        private void FilterAndSort()
        {
            var tabTypeId = (int)_curType;
            if (_curType == TabType.Weapon)
            {
                var list = _originalWeaponList;
                list = WeaponFilter(list, _filterParamDict[tabTypeId]);
                list = WeaponSort(list, _sortParamDict[tabTypeId]);
                _itemDataMap[tabTypeId] = new List<AUIItemData>(list);
            }
            else if(_curType == TabType.Armor)
            {
                var list = _originalArmorList;
                list = ArmorFilter(list, _filterParamDict[tabTypeId]);
                list = ArmorSort(list, _sortParamDict[tabTypeId]);
                _itemDataMap[tabTypeId] = new List<AUIItemData>(list);
            }
        }
        
        private List<UIWeaponData> WeaponSort(List<UIWeaponData> list, UISortParam sortParam)
        {
            return UIWeaponSortFilterHelper.SortWeapon(list, sortParam);
        }
        
        private List<UIWeaponData> WeaponFilter(List<UIWeaponData> list, UIFilterParam filterParam)
        {
            return UIWeaponSortFilterHelper.FilterWeapon(list, filterParam);
        }
        
        private List<UIArmorData> ArmorSort(List<UIArmorData> list, UISortParam sortParam)
        {
            return UIArmorSortFilterHelper.SortArmor(list, sortParam);
        }
        
        private List<UIArmorData> ArmorFilter(List<UIArmorData> list, UIFilterParam filterParam)
        {
            return UIArmorSortFilterHelper.FilterArmor(list, filterParam);
        }
    }
}