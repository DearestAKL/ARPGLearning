using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameMain.Runtime
{
    [RequireComponent(typeof(Button))]
    public class UIButtonExpand : APointerExpand
    {
        private Action _enterAction;
        private Action _exitAction;
        
        private Action _downAction;
        private Action _upAction;
        
        public void AddEnterAndExitAction(Action enterAction,Action exitAction)
        {
            _enterAction = enterAction;
            _exitAction = exitAction;
        }
        
        public void AddDownAndUpAction(Action downAction,Action upAction)
        {
            _downAction = downAction;
            _upAction = upAction;
        }

        // 当鼠标进入Button时调用
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            _enterAction?.Invoke();
        }
        
        // 当鼠标离开Button时调用
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            _exitAction?.Invoke();
        }
        

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            _downAction?.Invoke();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            _upAction?.Invoke();
        }
    }
}