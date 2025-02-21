using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Runtime
{
    public class UIAttributeAscensionItem : MonoBehaviour
    {
        [SerializeField] private Image imgType = null;
        [SerializeField] private TextMeshProUGUI txtType = null;
        [SerializeField] private TextMeshProUGUI txtContentValue = null;
        [SerializeField] private TextMeshProUGUI txtLeftValue = null;
        [SerializeField] private GameObject goArrow = null;
        [SerializeField] private GameObject goUp = null;
        
        public void UpdateView(int curValue, int nextValue = 0)
        {
            bool hasUp = nextValue > curValue;

            goArrow.gameObject.SetActive(hasUp);
            goUp.gameObject.SetActive(hasUp);
            txtContentValue.gameObject.SetActive(hasUp);
            if (hasUp)
            {
                txtContentValue.text = curValue.ToString();
                txtLeftValue.text = nextValue.ToString();
                txtLeftValue.color = Constant.ColorDef.UpYellow;
            }
            else
            {
                txtLeftValue.text = curValue.ToString();
                txtLeftValue.color = Constant.ColorDef.White;
            }
        }

        public void UpdateView(float curValue, float nextValue = 0f)
        {
            bool hasUp = nextValue > curValue;
            goArrow.gameObject.SetActive(hasUp);
            goUp.gameObject.SetActive(hasUp);
            txtContentValue.gameObject.SetActive(hasUp);
            if (hasUp)
            {
                txtContentValue.text = $"{curValue:F1}%";
                txtContentValue.text = $"{nextValue:F1}%";
                txtLeftValue.color = Constant.ColorDef.UpYellow;
            }
            else
            {
                txtLeftValue.text =  $"{curValue:F1}%";
                txtLeftValue.color = Constant.ColorDef.White;
            }
        }
        
        public void UpdateView(AttributeType type, int curValue, int nextValue = 0)
        {
            UpdateView(curValue, nextValue);
        }
        
        public void UpdateView(AttributeType type, float curValue, float nextValue = 0f)
        {
            UpdateView(curValue, nextValue);
        }
    }
}