using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Runtime
{
    public class UIBattleHpBar : MonoBehaviour
    {
        [SerializeField] private Image imgTransition;
        [SerializeField] private Image imgProgress;
        
        private TweenerCore<Vector3, Vector3, VectorOptions> _tweener;

        private float _curRatio;

        public void InitRatio(float ratio,Color progressColor)
        {
            _curRatio = ratio;
            imgProgress.color = progressColor;
            imgProgress.transform.localScale = new Vector3(_curRatio, 1, 1);
            imgTransition.transform.localScale = new Vector3(_curRatio, 1, 1);
        }

        public void UpdateRatio(float newRatio)
        {
            if (Math.Abs(_curRatio - newRatio) < 0.001f)
            {
                //无变化
                return;
            }
            var oldRatio = _curRatio;
            _curRatio = newRatio;
            
            var forceImg = oldRatio > _curRatio ? imgProgress : imgTransition;
            var transitImg = oldRatio > _curRatio ? imgTransition : imgProgress;
            
            forceImg.transform.localScale = new Vector3(_curRatio, 1, 1);

            if (_tweener.IsActive() && !_tweener.IsComplete())
            {
                _tweener.Kill();
                _tweener = null;
            }
            
            _tweener = transitImg.transform.DOScaleX(_curRatio, 0.5f).SetDelay(0.5F);
        }
    }
}