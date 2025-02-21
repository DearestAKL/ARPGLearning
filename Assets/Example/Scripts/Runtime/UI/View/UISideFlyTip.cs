using DG.Tweening;
using TMPro;
using UnityEngine;

namespace GameMain.Runtime
{
    public class UISideFlyTip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtContent;
        [SerializeField] private DOTweenAnimation doTweenAnimation;
        [SerializeField] private CanvasGroup canvasGroup;

        public void Awake()
        {
            doTweenAnimation.onComplete.AddListener(DelayedHide);
        }

        public void SetContent(string content)
        {
            if (gameObject.activeSelf)
            {
                return;
            }

            transform.SetAsLastSibling();

            gameObject.SetActive(true);

            doTweenAnimation.DORestart();

            canvasGroup.alpha = 1;

            gameObject.SetActive(true);
            txtContent.text = content;
        }

        private void DelayedHide()
        {
            DOVirtual.DelayedCall(1F,
                () => canvasGroup.DOFade(0, 0.5F).OnComplete(
                    () => gameObject.SetActive(false)));
        }

    }
}
