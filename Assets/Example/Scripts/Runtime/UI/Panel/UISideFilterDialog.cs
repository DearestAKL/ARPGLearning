using System;
using UnityEngine;

namespace GameMain.Runtime
{
    public class UISideFilterDialog : UISideFilterDialogSign
    {
        public class Params
        {
            public readonly UIFilterParam FilterParam;
            public readonly Action<UIFilterParam> RefreshFilter;
            
            public Params(UIFilterParam filterParam,Action<UIFilterParam> refreshFilter)
            {
                FilterParam = filterParam;
                RefreshFilter = refreshFilter;
            }
        }
        
        private Params _data;

        public override void OnInit(string name, GameObject go, Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);
            
            btnClose.onClick.AddListener(Close);
            btnConfirm.onClick.AddListener(OnConfirm);
            btnReset.onClick.AddListener(OnReset);
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            if (userData is Params data)
            {
                _data = data;
                UpdateView();
            }
        }

        private void UpdateView()
        {
            goWeaponFilter.gameObject.SetActive(_data.FilterParam.UIType == FilterUIType.Weapon);
            goArmorFilter.gameObject.SetActive(_data.FilterParam.UIType == FilterUIType.Armor);
            
            for (int i = 0; i < _data.FilterParam.CategoryList.Count; i++)
            {
                var filterCategory = _data.FilterParam.CategoryList[i];
                if (filterCategory.FilterCategory == FilterCategoryType.WeaponType)
                {
                    weaponType.SetFilterCategory(filterCategory);
                }
                else if (filterCategory.FilterCategory == FilterCategoryType.WeaponQuality)
                {
                    weaponQuality.SetFilterCategory(filterCategory);
                }
                else if (filterCategory.FilterCategory == FilterCategoryType.ArmorType)
                {
                    armorType.SetFilterCategory(filterCategory);
                }
                else if (filterCategory.FilterCategory == FilterCategoryType.ArmorQuality)
                {
                    armorQuality.SetFilterCategory(filterCategory);
                }
            }
        }

        private void OnConfirm()
        {
            //确定
            _data.FilterParam.Decide();
            _data.RefreshFilter.Invoke(_data.FilterParam);
            Close();
        }
        
        private void OnReset()
        {
            //重置
            _data.FilterParam.Reset();
            UpdateView();
        }

        private void OnClear()
        {
            //清空
            _data.FilterParam.Clear();
            UpdateView();
        }
    }
}