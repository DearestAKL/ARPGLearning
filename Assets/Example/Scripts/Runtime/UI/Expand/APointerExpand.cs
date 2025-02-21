using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMain.Runtime
{
    public abstract class APointerExpand : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler,IPointerUpHandler
    {
        [SerializeField] private ButtonSoundType soundType;
        
        [SerializeField] private Vector3 highlightedSize = new Vector3(1F,1F,1F);
        [SerializeField] private Vector3 pressedSize = new Vector3(1F,1F,1F);
        
        private void OnDisable()
        {
            if (Math.Abs(transform.localScale.x - 1) > 0.001f)
            {
                transform.localScale = Vector3.one;
            }
        }
        
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            PlayButtonSound();
        }
        
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(highlightedSize, 0.2f);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(Vector3.one, 0.2f);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            transform.DOScale(pressedSize, 0.1f);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            transform.DOScale(Vector3.one, 0.1f);
        }

        private void PlayButtonSound()
        {
            if (soundType == ButtonSoundType.None)
            {
                return;
            }
            
            AudioManager.Instance.PlaySound(soundType.GetSoundAssetName());
        }
    }
}