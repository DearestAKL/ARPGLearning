using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Runtime
{
    [RequireComponent(typeof(Toggle))]
    public class UIFilterToggle : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtLabel;
        [SerializeField] private Color colorNormal;
        [SerializeField] private Color colorOn;
        
        private Toggle _toggle;
        private UIFilterTerm _filterTerm;
        
        
        private void Awake()
        {
            if (_toggle == null)
            {
                _toggle = GetComponent<Toggle>();
            }
        }

        public void SetFilterTerm(UIFilterTerm filterTerm)
        {
            _filterTerm = filterTerm;
            _toggle.isOn = _filterTerm.IsSelected;

            txtLabel.text = filterTerm.LabelId;
            txtLabel.color = _filterTerm.IsSelected ? colorOn : colorNormal;
            
            _toggle.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(bool isOn)
        {
            if (_filterTerm.IsSelected == isOn)
            {
                return;
            }
            
            _filterTerm.ToggleSelectedCondition();
            txtLabel.color = isOn ? colorOn : colorNormal;
        }
        
#if UNITY_EDITOR
        public void OnValidate()
        {
            _toggle = GetComponent<Toggle>();
            if (_toggle != null && txtLabel != null)
            {
                txtLabel.color = _toggle.isOn ? colorOn : colorNormal;
            }
        }
#endif
    }
}