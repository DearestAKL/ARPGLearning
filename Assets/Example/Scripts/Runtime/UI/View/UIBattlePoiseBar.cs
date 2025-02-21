using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Runtime
{
    public class UIBattlePoiseBar : MonoBehaviour
    {
        [SerializeField] private Image imgProgress;
        [SerializeField] private CanvasGroup canvasGroup;

        private float _curRatio;
        private Tweener _tweener;

        public bool IsFailure { get; private set; }

        public void InitRatio(float ratio)
        {
            _curRatio = ratio;
            imgProgress.transform.localScale = new Vector3(_curRatio, 1, 1);
        }

        public void UpdateRatio(float newRatio, bool isFailure)
        {
            if (Math.Abs(_curRatio - newRatio) < 0.0001f && IsFailure == isFailure)
            {
                //无变化
                return;
            }

            if (IsFailure != isFailure)
            {
                if (isFailure)
                {
                    //失效
                    StartBlinking();
                }
                else
                {
                    //重新生效
                    _tweener?.Kill();
                    canvasGroup.alpha = 1f;
                }

                IsFailure = isFailure;
            }
            
            _curRatio = newRatio;
            imgProgress.transform.localScale = new Vector3(_curRatio, 1, 1);
        }

        private void StartBlinking()
        {
            // 创建一个透明度Tween
            canvasGroup.alpha = 1f;
            _tweener = canvasGroup.DOFade(0.5f, 0.3f).SetLoops(-1, LoopType.Yoyo);
        }
    }
}