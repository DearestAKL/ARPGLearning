using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Runtime
{
    public class UIItemCell : LoopScrollItem
    {
        [SerializeField] private Image imgIcon = default;
        [SerializeField] private Image imgRarityBg = default;
        [SerializeField] private TextMeshProUGUI txtBottom = default;//num/level
        [SerializeField] private UIStarCollection starCollection = default;
        
        [SerializeField] private GameObject goLock = default;
        
        [SerializeField] private TextMeshProUGUI txtAscension = default;//显示 突破等级,突破等级Max时 显示不同颜色
        [SerializeField] private Image imgAscension = default;//突破等级Max时 显示不同颜色

        [SerializeField] private GameObject goCharacterHead;
        [SerializeField] private Image imgCharacterHead;
        
        [SerializeField] private GameObject goSelect;

        public void UpdateView(AUIItemData item,bool select)
        {
            if (item is UIItemData itemData)
            {
                UpdateView(itemData);
            }
            else if(item is UIWeaponData weaponData)
            {
                UpdateView(weaponData);
            }
            else if(item is UIArmorData armorData)
            {
                UpdateView(armorData);
            }
            
            goSelect.gameObject.SetActive(select);
        }

        public void UpdateSelect(bool select)
        {
            goSelect.gameObject.SetActive(select);
        }

        public void UpdateView(UIItemData item)
        {
            imgIcon.SetIcon(UIIconPathHelper.GetItemPath(item.Config.Icon));
            imgRarityBg.SetIcon(UIIconPathHelper.GetQualityBg(item.Config.Quality));
                
            starCollection.UpdateView(item.Config.Quality);

            txtBottom.text = item.Amount.ToString();
            
            imgAscension.gameObject.SetActive(false);
            
            goLock.gameObject.SetActive(false);
            goCharacterHead.gameObject.SetActive(false);
        }
        
        public void UpdateView(UIWeaponData weapon)
        {
            imgIcon.SetIcon(UIIconPathHelper.GetWeaponPath(weapon.Config.Icon));
            imgRarityBg.SetIcon(UIIconPathHelper.GetQualityBg(weapon.Config.Quality));
            starCollection.UpdateView(weapon.Config.Quality);
            
            txtBottom.text = $"Lv.{weapon.Level}";
            
            imgAscension.gameObject.SetActive(true);
            txtAscension.text = weapon.AscensionLevel.ToString();
            //imgAscension
            
            goCharacterHead.SetActive(weapon.CharacterId != 0);
            //goLock
            //imgCharacterHead
        }
        
        public void UpdateView(UIArmorData armor)
        {
            //imgIcon
            imgIcon.SetIcon(UIIconPathHelper.GetArmorPath(armor.Config.Icon));
            imgRarityBg.SetIcon(UIIconPathHelper.GetQualityBg(armor.Config.Quality));
            starCollection.UpdateView(armor.Config.Quality);
            
            txtBottom.text = $"+{armor.Level}";
            
            imgAscension.gameObject.SetActive(false);
            
            goCharacterHead.SetActive(armor.CharacterId != 0);
            //goLock
            //imgCharacterHead
        }
    }
}