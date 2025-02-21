using System;
using UnityEngine;

namespace GameMain.Runtime
{
    [RequireComponent(typeof(Animation))]
    public class UIPanelAnimation : MonoBehaviour
    {
        [SerializeField] private AnimationClip openClip;
        [SerializeField] private AnimationClip closeClip;

        private Animation _animation;
        private Action _openAction;
        private Action _closeAction;
        
        private void Awake()
        {
            _animation = GetComponent<Animation>();
            if (openClip != null)
            {
                _animation.AddClip(openClip, openClip.name);
            }
            if (closeClip != null)
            {
                _animation.AddClip(closeClip, closeClip.name);
            }
        }

        private void Update()
        {
            if (_openAction != null && !_animation.IsPlaying(openClip.name))
            {
                _openAction.Invoke();
                _openAction = null;
            }
            
            if (_closeAction != null && !_animation.IsPlaying(closeClip.name))
            {
                _closeAction.Invoke();
                _closeAction = null;
            }
        }
        
        public void PlayOpenAnimation(Action openAction = null)
        {
            _closeAction = null;
            
            if (openClip != null)
            {
                _animation.Stop();
                _animation.Play(openClip.name);
                
                _openAction = openAction;
            }
            else
            {
                openAction?.Invoke();
            }
        }
        
        public void PlayCloseAnimation(Action closeAction = null)
        {
            if (closeClip != null)
            {
                _animation.Stop();
                _animation.Play(closeClip.name);
                
                _closeAction = closeAction;
            }
            else
            {
                closeAction?.Invoke();
            }
        }
    }
}