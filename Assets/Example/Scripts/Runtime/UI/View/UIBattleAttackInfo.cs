using Akari.GfCore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Runtime
{
    public class UIBattleAttackInfo : MonoBehaviour
    {
        [SerializeField] private Image imgIcon;
        [SerializeField] private Image imgCtProgress;
        
        [SerializeField] private Image imgLock;
        [SerializeField] private Image imgCooling;
        [SerializeField] private Image imgForce;
        [SerializeField] private TextMeshProUGUI txtCoolTime;
        [SerializeField] private TextMeshProUGUI txtSlotNum;

        public void Init()
        {
            
        }

        public void UpdateView(float currentSlotCoolTimeRate,int currentSlotNum,float currentSlotRemainingCoolTime)
        {
            // Progress
            bool isProgressActive = currentSlotCoolTimeRate < 1f;
            imgCtProgress.gameObject.SetActive(isProgressActive);
            imgCtProgress.fillAmount = isProgressActive ? currentSlotCoolTimeRate : 0f;
            
            // Slot Num
            bool isSlotNumActive = currentSlotNum > 1;
            txtSlotNum.gameObject.SetActive(isSlotNumActive);
            txtSlotNum.text = isSlotNumActive ? currentSlotNum.ToString() : "";
            
            // Cooling
            bool isCoolingActive = currentSlotNum <= 0;
            imgCooling.gameObject.SetActive(isCoolingActive);
            txtCoolTime.gameObject.SetActive(isCoolingActive);
            txtCoolTime.text = isCoolingActive ? GfMathf.CeilToInt(currentSlotRemainingCoolTime).ToString() : "";
        }

        public void SetForce(bool isForce)
        {
            imgForce.gameObject.SetActive(isForce);
        }
    }
}