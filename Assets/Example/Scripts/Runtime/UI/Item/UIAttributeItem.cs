using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace GameMain.Runtime
{
    public class UIAttributeItem : MonoBehaviour
    {
        [SerializeField] private Image imgType = null;
        [SerializeField] private TextMeshProUGUI txtType = null;
        [SerializeField] private TextMeshProUGUI txtValue = null;
        [SerializeField] private TextMeshProUGUI txtAddValue = null;//可空
        [SerializeField] private Button btnDetail = null;//可空
        
        private void Awake()
        {
            if (btnDetail != null)
            {
                btnDetail.onClick.AddListener(OnDetail);
            }
        }
        
        public void UpdateView(int totalValue, int addValue = 0)
        {
            var baseValue = totalValue - addValue;
            txtValue.text = baseValue.ToString();
            
            if (txtAddValue != null)
            {
                txtAddValue.gameObject.SetActive(addValue > 0);
                if (addValue > 0)
                {
                    txtAddValue.text = $"+{addValue}";
                }
            }
        }
        
        public void UpdateView(float value)
        {
            txtValue.text = $"{value:F1}%";
            if (txtAddValue != null)
            {
                txtAddValue.gameObject.SetActive(false);
            }
        }
        
        public void UpdateView(AttributeType type,int value)
        {
            UpdateView(value);

            //set imgType
            //set txtType
        }
        
        public void UpdateView(AttributeType type,float value)
        {
            UpdateView(value);
            
            //set imgType
            //set txtType
        }

        private void OnDetail()
        {
            
        }
    }
}