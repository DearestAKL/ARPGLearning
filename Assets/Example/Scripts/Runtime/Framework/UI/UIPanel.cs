using System;
using Akari.GfCore;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace GameMain.Runtime
{
    public abstract class UIPanel
    {
        private GameObject _rootGo;
        private CanvasGroup _canvasGroup = null;
        private UIPanelAnimation _panelAnimation = null;
        private string _name = null;
        
        protected GfCompositeDisposable InitSubscriptions;//在回收时Dispose
        protected GfCompositeDisposable OpenSubscriptions;//在界面关闭时Dispose

        protected GameObject RootGo => _rootGo;
        public string Name => _name;

        public bool IsActive { get;private set; }
        public virtual UIPanelFlags Flags => UIPanelFlags.None;

        public virtual void OnInit(string name,GameObject go,Transform parent,object userData)
        {
            InitSubscriptions = GfCompositeDisposable.Create();
            
            _name = name;
            _rootGo = go;
            _rootGo.transform.SetParent(parent);
            
            _panelAnimation = _rootGo.GetComponent<UIPanelAnimation>();
            
            _canvasGroup = _rootGo.GetOrAddComponent<CanvasGroup>();

            RectTransform transform = _rootGo.GetComponent<RectTransform>();
            transform.localScale = Vector3.one;
            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.anchoredPosition = Vector2.zero;
            transform.sizeDelta = Vector2.zero;
            transform.anchoredPosition3D = Vector3.zero;

            _rootGo.GetOrAddComponent<GraphicRaycaster>();
        }

        public virtual void OnOpen(object userData)
        {
            if (IsActive) { return; }
            
            OpenSubscriptions = GfCompositeDisposable.Create();
            
            _rootGo.transform.SetAsLastSibling();
            _rootGo.gameObject.SetActive(true);
            _canvasGroup.alpha = 1f;
            
            if (_panelAnimation != null)
            {
                //开启动画播完后才能激活界面
                _panelAnimation.PlayOpenAnimation(() =>
                {
                    IsActive = true;
                    _canvasGroup.blocksRaycasts = true;
                });
            }
            else
            {
                IsActive = true;
                _canvasGroup.blocksRaycasts = true;
            }
        }

        public virtual void OnClose()
        {
            if (!IsActive) { return; }
            
            IsActive = false;
            _canvasGroup.blocksRaycasts = false;
            
            if (_panelAnimation != null)
            {
                //关闭动画只是表现 不在影响界面逻辑
                _panelAnimation.PlayCloseAnimation(() =>
                {
                    _canvasGroup.alpha = 0f;
                    _rootGo.gameObject.SetActive(false);
                });
            }
            else
            {
                _canvasGroup.alpha = 0f;
                _rootGo.gameObject.SetActive(false);
            }
            
            OpenSubscriptions.Dispose();
        }

        public virtual void OnPause()
        {

        }

        public virtual void OnRecycle()
        {
            Object.Destroy(_rootGo);
            InitSubscriptions.Dispose();
        }

        public virtual void OnResume()
        {

        }

        public virtual void OnUpdate(float elapseSeconds)
        {

        }

        public void Close()
        {
            UIManager.Instance.CloseUIPanel(Name);
        }
    }

    [Flags]
    public enum UIPanelFlags
    {
        None = 0,
    }
}
