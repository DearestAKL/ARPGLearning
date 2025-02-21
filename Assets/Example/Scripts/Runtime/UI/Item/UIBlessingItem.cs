using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using cfg;
using UnityEngine.Events;

namespace GameMain.Runtime
{
    public class UIBlessingItem : MonoBehaviour
    {
        [SerializeField] private Button btnClick;
        [SerializeField] private TextMeshProUGUI txtName;
        [SerializeField] private TextMeshProUGUI txtContent;
        [SerializeField] private Image imgIcon;
        [SerializeField] private Image imgBg;
        [SerializeField] private Image[] stars;

        public UnityEvent<BlessingConfig> OnClickEvent = new UnityEvent<BlessingConfig>();

        private BlessingConfig _blessing;

        private void Awake()
        {
            btnClick.onClick.AddListener(OnClick);
        }

        public void UpdateView(BlessingConfig blessing)
        {
            _blessing = blessing;
            
            txtName.text = blessing.Name;
            txtContent.text = blessing.Describe;

            SetStars(1);
        }

        private void SetStars(int num)
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].transform.gameObject.SetActive(i < num);
            }
        }

        private void OnClick()
        {
            OnClickEvent.Invoke(_blessing);
        }
    }

    public enum BlessingQuality
    {
        Common,
        Rare,
        Epic,
    }
}