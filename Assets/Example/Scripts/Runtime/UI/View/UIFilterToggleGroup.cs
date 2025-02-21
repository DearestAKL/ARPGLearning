using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameMain.Runtime
{
    public class UIFilterToggleGroup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtLabel;
        [SerializeField] private UIFilterToggle filterTogglePrefab;
        [SerializeField] private Transform filterToggleContent;
        [SerializeField] private List<UIFilterToggle> filterToggles = new List<UIFilterToggle>();

        public void SetFilterCategory(UIFilterCategory filterCategory)
        {
            txtLabel.text = filterCategory.CategoryNameLabelId;
            
            if (filterCategory.TermList.Count > filterToggles.Count)
            {
                CreateFilterToggle(filterCategory.TermList.Count - filterToggles.Count);
            }

            for (int i = 0; i < filterToggles.Count; i++)
            {
                if (i <= filterCategory.TermList.Count)
                {
                    filterToggles[i].gameObject.SetActive(true);
                    filterToggles[i].SetFilterTerm(filterCategory.TermList[i]);
                }
                else
                {
                    filterToggles[i].gameObject.SetActive(false);
                }
            }
        }

        private void CreateFilterToggle(int num)
        {
            for (int i = 0; i < num; i++)
            {
                var filterToggle = Instantiate(filterTogglePrefab, filterToggleContent ? filterToggleContent : transform);
                filterToggles.Add(filterToggle);
            }
        }
    }
}