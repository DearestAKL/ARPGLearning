using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameMain.Runtime
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UITMProExpand : MonoBehaviour
    {
        [SerializeField] private string labelId = default;
        
        private TextMeshProUGUI _txt;
        
        private void Awake()
        {
            _txt = GetComponent<TextMeshProUGUI>();
            UpdateText();
        }

        private void UpdateText()
        {
            if (!string.IsNullOrEmpty(labelId))
            {
                _txt.text = labelId.GetLocalization();
            }
        }

#if UNITY_EDITOR
        public void Preparation()
        {
            if (!string.IsNullOrEmpty(labelId))
            {
                _txt = GetComponent<TextMeshProUGUI>();
                var localization = LubanManager.GetEditorTables().TbLocalization.GetOrDefault(labelId);
                _txt.text = localization == null ? "未本地化" : localization.Cn;;
            }
        }
#endif
    }
}