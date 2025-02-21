using TMPro;
using UnityEngine;

namespace GameMain.Runtime
{
    public class UINameTips: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtName;

        public void UpdateView(string name)
        {
            txtName.text = name;
        }
    }
}