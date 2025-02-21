using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Runtime
{
    [RequireComponent(typeof(Toggle))]
    public class UIToggleEx : APointerExpand
    {
        public enum Type
        {
            None,
            Label,
            Color
        }

        [SerializeField] private TextMeshProUGUI txtLabel;
        [SerializeField] private Image imgLabel;
        [SerializeField] private Type type;
        [SerializeField] private TextMeshProUGUI txtOnLabel;
        [SerializeField] private Color colorNormal;
        [SerializeField] private Color colorOn;
        
        private Toggle _toggle;
        private UIToggleGroupEx _toggleGroupEx;

        private int _index = -1;

        public Toggle Toggle
        {
            get
            {
                if (_toggle == null)
                {
                    _toggle = GetComponent<Toggle>();
                }

                return _toggle;
            }
        }
        public int Index => _index;

        public void Init(UIToggleGroupEx toggleGroupEx, int index, string toggleName = null,string iconPath = null)
        {
            Toggle.onValueChanged.AddListener(OnValueChanged);
            
            _toggleGroupEx = toggleGroupEx;
            _index = index;
            
            if (!string.IsNullOrEmpty(toggleName))
            {
                if (txtLabel != null)
                {
                    txtLabel.FormatLocalization(toggleName);
                    if(type == Type.Label)
                    {
                        if (txtOnLabel != null)
                        {
                            txtOnLabel.FormatLocalization(toggleName);
                        }
                    }
                    else if(type == Type.Color)
                    {
                        txtLabel.color = Toggle.isOn ? colorOn : colorNormal;
                    }
                }
            }

            if (!string.IsNullOrEmpty(iconPath))
            {
                if (imgLabel != null)
                {
                    imgLabel.SetIcon(iconPath);
                    if(type == Type.Color)
                    {
                        imgLabel.color = Toggle.isOn ? colorOn : colorNormal;
                    }
                }
            }
        }

        public void Clear()
        {
            _index= -1;
            gameObject.SetActive(false);
        }

        private void OnValueChanged(bool isOn)
        {
            if (isOn)
            {
                _toggleGroupEx.ChangeCurIndex(_index);
            }

            if (type == Type.Label)
            {
                if (txtOnLabel != null)
                {
                    txtOnLabel.gameObject.SetActive(isOn);
                }
            }
            else if(type == Type.Color)
            {
                if (txtLabel != null)
                {
                    txtLabel.color = isOn ? colorOn : colorNormal;
                }

                if (imgLabel != null)
                {
                    imgLabel.color = isOn ? colorOn : colorNormal;
                }
            }
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            if (type == Type.Color)
            {
                var toggle = GetComponent<Toggle>();
                if (toggle != null)
                {
                    if (txtLabel != null)
                    {
                        txtLabel.color = toggle.isOn ? colorOn : colorNormal;  
                    }

                    if (imgLabel != null)
                    {
                        imgLabel.color = toggle.isOn ? colorOn : colorNormal;    
                    }
                }
            }
        }
#endif
    }
}