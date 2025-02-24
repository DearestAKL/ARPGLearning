using System.Collections.Generic;
using System.Linq;
using Akari.GfCore;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameMain.Runtime
{
    /// <summary>
    /// 通用GM功能集合
    /// </summary>
    public class UIGMCommonPanel : UIGMCommonPanelSign
    {
        private readonly Dictionary<int, string> _typeDisplayStrings = new Dictionary<int, string>()
        {
            {(int)AddItemType.Item, "道具"},
            {(int)AddItemType.Weapon, "武器"},
            {(int)AddItemType.Armor, "遗物"},
        };
        
        private enum AddItemType
        {
            Item,
            Weapon,
            Armor
        }

        private AddItemType _curAddItemType = AddItemType.Item;
        
        public override void OnInit(string name, GameObject go, Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);

            btnClose.onClick.AddListener(Close);
            
            var displayStrings = _typeDisplayStrings.Values.ToList();
            dropdownItem.AddOptions(displayStrings);
            btnAddItem.onClick.AddListener(OnAddItem);
            
            dropdownItem.onValueChanged.AddListener(OnAddItemTypeChange);
            dropdownItem.value = (int)_curAddItemType;

            CreatButton(OnOpenFishPanel, "钓鱼");
            CreatButton(GetAllItem, "获取所有道具数量X10");
        }

        private async void OnAddItem()
        {
            int.TryParse(inputAddItemId.text,out var id);
            int.TryParse(inputAddItemNum.text,out var num);
            if (id == 0 || num == 0)
            {
                //Debug 请检查配置是否符合规范
                return;
            }

            var itemDataList = new List<AUIItemData>();
            if (_curAddItemType == AddItemType.Item)
            {
                if (LubanManager.Instance.Tables.TbItem.GetOrDefault(id) == null)
                {
                    await UIHelper.OpenTips($"Item {id} Item配置表中不存在");
                    return;
                }
                
                UserDataManager.Instance.Item.AddItem(id, num);
                itemDataList.Add(new UIItemData(id, num));
            }
            else if(_curAddItemType == AddItemType.Weapon)
            {
                if (LubanManager.Instance.Tables.TbWeapon.GetOrDefault(id) == null)
                {
                    await UIHelper.OpenTips($"Weapon {id} Weapon配置表中不存在");
                    return;
                }

                var addTimes = GfMathf.Clamp(num, 1, 20);
                for (int i = 0; i < addTimes; i++)
                {
                    var userWeapon = UserDataManager.Instance.Weapon.AddWeapon(id);
                    itemDataList.Add(new UIWeaponData(userWeapon));
                }
            }
            else if (_curAddItemType == AddItemType.Armor)
            {
                if (LubanManager.Instance.Tables.TbArmor.GetOrDefault(id) == null)
                {
                    await UIHelper.OpenTips($"Armor {id} Armor配置表中不存在");
                    return;
                }
                var addTimes = GfMathf.Clamp(num, 1, 20);
                for (int i = 0; i < addTimes; i++)
                {
                    var userArmor = UserDataManager.Instance.Armor.AddArmor(id);
                    itemDataList.Add(new UIArmorData(userArmor));
                }
            }
            await UIManager.Instance.OpenUIPanel(UIType.UIRewardDialog, new UIRewardDialog.Params(itemDataList));
        }

        private void OnAddItemTypeChange(int value)
        {
            _curAddItemType = (AddItemType)value;
        }

        private void CreatButton(UnityAction action,string btnText)
        {
            var btn = Object.Instantiate(btnMod, contentBtns).GetComponent<Button>();
            btn.gameObject.SetActive(true);
            btn.onClick.AddListener(action);

            var txt = btn.GetComponentInChildren<TextMeshProUGUI>();
            txt.text = btnText;
        }

        private async void OnOpenFishPanel()
        {
            await UIManager.Instance.OpenUIPanel(UIType.UIFishPanel, new UIFishPanel.Params(
                new FishData()
                {
                    Energy = 100,
                    Power = 10,
                },new FishingRodData()
                {
                    Stability = 10,
                    DragPower = 10,
                }));
        }

        private async void GetAllItem()
        {
            var itemDataList = new List<AUIItemData>();
            int num = 10;
            var itemConfigs = LubanManager.Instance.Tables.TbItem.DataList;
            foreach (var itemConfig in itemConfigs)
            {
                UserDataManager.Instance.Item.AddItem(itemConfig.Id, num);
                itemDataList.Add(new UIItemData(itemConfig.Id, num));
            }
            var armorConfigs = LubanManager.Instance.Tables.TbArmor.DataList;
            foreach (var armorConfig in armorConfigs)
            {
                for (int i = 0; i < num; i++)
                {
                    var userArmor = UserDataManager.Instance.Armor.AddArmor(armorConfig.Id);
                    itemDataList.Add(new UIArmorData(userArmor));
                }
            }
            var weaponConfigs = LubanManager.Instance.Tables.TbWeapon.DataList;
            foreach (var weaponConfig in weaponConfigs)
            {
                for (int i = 0; i < num; i++)
                {
                    var userWeapon = UserDataManager.Instance.Weapon.AddWeapon(weaponConfig.Id);
                    itemDataList.Add(new UIWeaponData(userWeapon));
                }
            }
            await UIManager.Instance.OpenUIPanel(UIType.UIRewardDialog, new UIRewardDialog.Params(itemDataList));
        }
    }
}