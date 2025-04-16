using TMPro;
using UnityEngine;

namespace GameMain.Runtime
{
    public class UIButtonTips : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtContent;
        [SerializeField] private TextMeshProUGUI txtBtn;

        public void UpdateView(string content,string btnContent)
        {
            txtContent.text = content;

            if (btnContent == Constant.InputDef.Interaction)
            {
                txtBtn.text = "F";
            }
            else if (btnContent == Constant.InputDef.Jump)
            {
                txtBtn.text = "Space"; 
            }
        }
    }
}