using System;
using System.Collections.Generic;
using Akari.GfCore;
using UnityEngine;
using UnityEngine.Events;

namespace GameMain.Runtime
{
    public class UISortOptions : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_Dropdown dropdownItem = null;
        [SerializeField] private UnityEngine.UI.Button btnReverse = null;
        
        private UISortParam _sortParam;
        private UISortDef _sortDef;

        public UnityEvent<UISortParam> OnSort = new UnityEvent<UISortParam>();

        private void Awake()
        {
            btnReverse.onClick.AddListener(OnReverse);
            
            dropdownItem.onValueChanged.AddListener(OnSortTypeChange);
        }

        public void UpdateView(UISortDef sortDef, UISortParam sortParam)
        {
            _sortDef = sortDef;
            _sortParam = sortParam;
            
            List<string> displayStrings = new List<string>();
            for (int i = 0; i < sortDef.SortCategoryList.Count; i++)
            {
                displayStrings.Add(sortDef.SortCategoryList[i].TextLabelId);
            }
            dropdownItem.ClearOptions();
            dropdownItem.AddOptions(displayStrings);
            dropdownItem.SetValueWithoutNotify(sortParam.SortTypeValue);
        }

        private void OnReverse()
        {
            _sortParam.ToggleSortOrder();
            OnSort?.Invoke(_sortParam);
        }

        private void OnSortTypeChange(int value)
        {
            var sortTypeValue = _sortDef.SortCategoryList[value].SortTypeValue;
            _sortParam.ChangeSortType(sortTypeValue);
            OnSort?.Invoke(_sortParam);
        }
    }
}