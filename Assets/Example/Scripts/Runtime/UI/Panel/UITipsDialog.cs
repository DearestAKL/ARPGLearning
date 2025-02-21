using System;
using Akari.GfCore;
using DG.Tweening;
using UnityEngine;

namespace GameMain.Runtime
{
    public class UITipsDialog : UITipsDialogSign
    {
        public class Params
        {
            public string Content;
            
            public Params(string content)
            {
                Content = content;
            }
        }
        
        private Params _data;
        private Tween _tween;
        
        public override void OnInit(string name, GameObject go, Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);
            btnBackground.onClick.AddListener(Close);
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            if (userData is Params data)
            {
                _data = data;

                txtContent.text = _data.Content;

                _tween = DOVirtual.DelayedCall(3F, Close);
            }
        }

        public override void OnClose()
        {
            base.OnClose();
            if (_tween.IsActive())
            {
                _tween.Kill();
            }
        }
    }
}