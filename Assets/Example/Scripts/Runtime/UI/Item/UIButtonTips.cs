using TMPro;
using UnityEngine;

namespace GameMain.Runtime
{
    public class UIButtonTips : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtContent;

        public void UpdateView(string content)
        {
            txtContent.text = content;
        }
    }
}