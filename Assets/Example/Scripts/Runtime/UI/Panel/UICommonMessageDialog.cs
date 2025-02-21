using System;
using UnityEngine;

namespace GameMain.Runtime
{
    public class UICommonMessageDialog : UICommonMessageDialogSign
    {
        public class Params
        {
            public string Content;
            public Action ConfirmAction;
            public Action CancelAction;

            public string ConfirmContent;
            public string CancelContent;

            public Params(string content, 
                Action confirmAction, 
                Action cancelAction,
                string confirmContent, 
                string cancelContent)
            {
                Content = content;
                ConfirmAction = confirmAction;
                CancelAction = cancelAction;
                ConfirmContent = confirmContent;
                CancelContent = cancelContent;
            }
        }

        private Params _data;

        public override void OnInit(string name, GameObject go, Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);

            btnConfirm.onClick.AddListener(OnConfirm);
            btnCancel.onClick.AddListener(OnCancel);
            btnBackground.onClick.AddListener(OnBackground);
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            if (userData is Params data)
            {
                _data = data;

                UpdateStatus();
            }
        }

        private void UpdateStatus()
        {
            goTitle.SetActive(false);
            goButtonDown.SetActive(true);

            txtContent.text = _data.Content;
            
            if (string.IsNullOrEmpty(_data.CancelContent))
            {
                txtCancel.text = "取消";
            }
            else
            {
                txtCancel.text = _data.CancelContent;
            }

            if (string.IsNullOrEmpty(_data.ConfirmContent))
            {
                txtConfirm.text = "确认";
            }
            else
            {
                txtConfirm.text = _data.ConfirmContent;
            }

            txtBackground.gameObject.SetActive(_data.CancelAction != null);
        }

        private void OnConfirm()
        {
            this.Close();
            _data.ConfirmAction?.Invoke();
        }

        private void OnCancel()
        {
            this.Close();
            _data.CancelAction?.Invoke();
        }
        
        private void OnBackground()
        {
            if (_data.CancelAction != null)
            {
                //CancelAction有则启用 背景关闭
                this.Close();
            }
        }
    }
}