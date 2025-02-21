using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GameMain.Runtime
{
    public class UISettingDropdown : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtName;
        [SerializeField] private TMP_Dropdown dropdown = null;

        public readonly UnityEvent<int> OnValueChange = new UnityEvent<int>();

        private void Awake()
        {
            dropdown.onValueChanged.AddListener(OnOptionChanged);
        }

        public void AddOptions(List<string> options, int curIndex)
        {
            dropdown.AddOptions(options);
            dropdown.value = curIndex;
        }
        
        private void OnOptionChanged(int index)
        {
            OnValueChange.Invoke(index);
        }
    }
}