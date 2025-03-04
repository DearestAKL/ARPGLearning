using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain.Runtime
{
    public class UIBattleStatusPiece : MonoBehaviour
    {
        [SerializeField] private UIBattleHpBar hpHpBar;
        [SerializeField] private UIBattlePoiseBar poiseBar;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float visibilitySpanTime = 3f;
        
        private float _elapsedTime;
        private bool _isUserPlayer;
        private bool _isVisibility;
        private Vector3 _position;
        public bool IsVisibility => _isVisibility;

        private readonly Color _playerColor = new Color(55f/255f,158f/255f,13f/255f,1f);
        private readonly Color _enemyColor = new Color(212f/255f,33f/255f,40f/255f,1f);

        public void Init(float hpRatio, float poiseRatio, bool isTeamA, bool isUserPlayer)
        {
            _isUserPlayer = isUserPlayer;
            hpHpBar.InitRatio(hpRatio, isTeamA ? _playerColor: _enemyColor);

            poiseBar.gameObject.SetActive(poiseRatio > -1);
            if (poiseRatio > -1)
            {
                poiseBar.InitRatio(poiseRatio);
            }
            
            SetVisibility(_isUserPlayer);
        }

        public void UpdateHp(float newHpRatio)
        {
            if (!_isUserPlayer) 
            {
                SetVisibility(true);
                _elapsedTime = 0;
            }

            hpHpBar.UpdateRatio(newHpRatio);
        }

        public void SetPosition(Vector2 uiPosition)
        {
            _position = uiPosition;
            float sqrDistance = (transform.localPosition - _position).sqrMagnitude;
            if (sqrDistance > 50F * 50F)
            {
                transform.localPosition = _position;
            }
        }

        public void UpdatePoiseBar(float newPoiseRatio,bool isFailure)
        {
            if (newPoiseRatio > -1)
            {
                poiseBar.UpdateRatio(newPoiseRatio,isFailure);
            }
        }

        public void SetVisibility(bool isVisibility)
        {
            _isVisibility = isVisibility;
            canvasGroup.alpha = _isVisibility ? 1 : 0;
        }

        private void Update()
        {
            if (_isUserPlayer || !_isVisibility)
            {
                return;
            }

            _elapsedTime += Time.deltaTime;
            if (_elapsedTime > visibilitySpanTime)
            {
                SetVisibility(false);
            }
        }

        private void LateUpdate()
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _position, 0.25F);
        }
    }
}
