using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Runtime
{
    public class UIItemTips : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtTitle;
        [SerializeField] private TextMeshProUGUI txtDescription;
        [SerializeField] private Image imgTitle;
        [SerializeField] private Image imgDetail;
        [SerializeField] private Image imgIcon;
        

        public void UpdateView(UIItemData item)
        {
            imgTitle.color = Constant.ColorDef.GetQualityColor(item.Config.Quality);
            
            imgIcon.SetIcon(UIIconPathHelper.GetItemPath(item.Config.Icon));
            imgDetail.SetIcon(UIIconPathHelper.GetQualityColorBg(item.Config.Quality));
            
            txtTitle.text = item.Config.Name;
        }
        
        public void UpdateView(UIWeaponData weapon)
        {
            imgTitle.color = Constant.ColorDef.GetQualityColor(weapon.Config.Quality);
            
            imgIcon.SetIcon(UIIconPathHelper.GetEquipInfoPath(weapon.Config.Icon));
            imgDetail.SetIcon(UIIconPathHelper.GetQualityColorBg(weapon.Config.Quality));
            
            txtTitle.text = weapon.Config.Name;
        }
        
        public void UpdateView(UIArmorData armor)
        {
            imgTitle.color = Constant.ColorDef.GetQualityColor(armor.Config.Quality);
            
            //imgIcon
            imgDetail.SetIcon(UIIconPathHelper.GetQualityColorBg(armor.Config.Quality));
            
            txtTitle.text = armor.Config.Name;
        }
    }
}