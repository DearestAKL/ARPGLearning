using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

namespace GameMain.Runtime
{
    public class UISettingSlider : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtValue;
        [SerializeField] private Slider slider;

        public readonly UnityEvent<int> OnValueChange = new UnityEvent<int>();

        private void Awake()
        {
            slider.onValueChanged.AddListener(OnValueChanged);
        }

        /// <summary>
        /// 添加 全局音量 控制
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(int value)
        {
            slider.value = value;
            txtValue.text = value.ToString();
        }

        private void OnValueChanged(float value)
        {
            int wholeNumbers = (int)value;
            OnValueChange.Invoke(wholeNumbers);
            txtValue.text = wholeNumbers.ToString();
        }
    }
}