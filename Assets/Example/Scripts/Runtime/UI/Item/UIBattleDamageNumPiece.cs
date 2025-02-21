using Akari.GfUnity;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace GameMain.Runtime
{
    public class UIBattleDamageNumPiece : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtNormalNum;
        [SerializeField] private TextMeshProUGUI txtCriticalNum;

        public void Show(Vector2 position, int value,bool isCritical,bool isAdd)
        {
            var activeText = isCritical ? txtCriticalNum : txtNormalNum;
            var inactiveText = isCritical ? txtNormalNum : txtCriticalNum;

            activeText.text = isAdd ? $"+{value}" : $"-{value}";
            activeText.gameObject.SetActive(true);
            inactiveText.gameObject.SetActive(false);

            // 随机选择一个方向
            float randomXDirection = Random.Range(-50f, 50f);
            float randomYDirection = Random.Range(100F, 150f);
            var targetPosition = new Vector2(position.x + randomXDirection, position.y + randomYDirection);

            transform.localPosition = position;
            transform.DOScale(2f, 0.1f).OnComplete(() => transform.DOScale(1f, 0.1f));
            transform.DOLocalMove(targetPosition, 0.4F).SetEase(Ease.OutQuad)
                .OnComplete(() => GfPrefabPool.Return(this));
        }
    }
}