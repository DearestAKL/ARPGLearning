using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Runtime
{
    public class UIGroup
    {
        private Stack<UIPanelInfo> _uiPanelStack;
        private readonly List<UIPanelInfo> _uiPanelInfos;
        private bool _isPause;
        private string _groupName;
        private int _depth;

        private GameObject _rootGo;
        private Canvas _canvas;
        private CanvasScaler _canvasScaler;
        private bool _isOpenStack = false;
        
        private Vector2 _screen  = new Vector2(1920, 1080);

        public UIGroup(string groupName, int depth, Transform parent,bool isOpenStack)
        {
            _depth = depth;
            _groupName = groupName;
            OnInit(parent);
            _isOpenStack = isOpenStack;

            _isPause = false;
            _uiPanelStack = new Stack<UIPanelInfo>();
            _uiPanelInfos = new List<UIPanelInfo>();
        }

        /// <summary>
        /// 初始化 生成并缓存实体
        /// </summary>
        /// <param name="rootGo"></param>
        public void OnInit(Transform parentTrans)
        {
            var depth = Depth;

            _rootGo = new GameObject(_groupName);
            _rootGo.layer = LayerMask.NameToLayer("UI");
            _canvas = _rootGo.GetOrAddComponent<Canvas>();
            _canvas.renderMode = RenderMode.ScreenSpaceCamera;
            _canvas.worldCamera = UIManager.Instance.UICamera;
            _rootGo.GetOrAddComponent<GraphicRaycaster>();
            
            _canvasScaler = _rootGo.GetOrAddComponent<CanvasScaler>();
            _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _canvasScaler.referenceResolution = _screen;
            UpdateMatchWidthOrHeight();

            _canvas.overrideSorting = true;
            _canvas.sortingOrder = depth;

            RectTransform transform = _rootGo.GetComponent<RectTransform>();
            transform.SetParent(parentTrans);
            transform.localScale = Vector3.zero;
            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.anchoredPosition = Vector2.zero;
            transform.sizeDelta = Vector2.zero;
        }

        /// <summary>
        /// 界面组轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public void OnUpdate(float elapseSeconds, float realElapseSecond)
        {
            foreach (var uiPanelInfo in _uiPanelInfos)
            {
                if (uiPanelInfo.Paused)
                {
                    continue;
                }

                uiPanelInfo.UIPanel.OnUpdate(elapseSeconds);
            }
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        public GameObject RootGo
        {
            get 
            { 
                return _rootGo; 
            }
        }

        /// <summary>
        /// 获取界面组类型。
        /// </summary>
        public string GroupName
        {
            get
            {
                return _groupName;
            }
        }

        /// <summary>
        /// 获取或设置界面组深度。
        /// </summary>
        public int Depth
        {
            get
            {
                return _depth;
            }
            set
            {
                if (_depth == value)
                {
                    return;
                }

                _depth = value;
                _canvas.sortingOrder = _depth;
                //Refresh();
            }
        }

        /// <summary>
        /// 获取或设置界面组是否暂停。
        /// </summary>
        public bool IsPause
        {
            get
            {
                return _isPause;
            }
            set
            {
                if (_isPause == value)
                {
                    return;
                }

                _isPause = value;
                //Refresh();
            }
        }

        /// <summary>
        /// 获取界面组中界面数量。
        /// </summary>
        public int UIPanelCount
        {
            get
            {
                return _uiPanelInfos.Count;
            }
        }

        /// <summary>
        /// 获取当前界面。
        /// </summary>
        public UIPanel CurrentUIPanel
        {
            get
            {
                return _uiPanelStack.Count > 0 ? _uiPanelStack.Peek().UIPanel : null;
            }
        }

        /// <summary>
        /// 从界面组中获取界面。
        /// </summary>
        public UIPanel GetUIPanel(string uiPanelAssetName)
        {
            foreach (UIPanelInfo uiPanelInfo in _uiPanelInfos)
            {
                if (uiPanelInfo.UIPanel.Name == uiPanelAssetName)
                {
                    return uiPanelInfo.UIPanel;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取界面信息
        /// </summary>
        /// <param name="uiPanel"></param>
        /// <returns></returns>
        private UIPanelInfo GetUIPanelInfo(UIPanel uiPanel)
        {

            foreach (UIPanelInfo uiPanelInfo in _uiPanelInfos)
            {
                if (uiPanelInfo.UIPanel == uiPanel)
                {
                    return uiPanelInfo;
                }
            }

            return null;
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="panelName"></param>
        public async UniTask OpenUIPanel(string panelName, object userData)
        {
            var uiPanel = GetUIPanel(panelName);
            if (uiPanel == null)
            {
                //没有 则创建
                var panel = await UIManager.Instance.Factory.CreatUIPanel(panelName);
                if(panel == null)
                {
                    return;
                }
                var type = UIManager.Instance.GetUIPanelType(panelName);
                if (type == null)
                {
                    return;
                }

                uiPanel = (UIPanel)Activator.CreateInstance(type);
                uiPanel.OnInit(panelName, panel, _rootGo.transform, userData);
                //缓存下来
                _uiPanelInfos.Add(UIPanelInfo.Create(uiPanel));
            }
            
            if (_isOpenStack && _uiPanelStack.TryPeek(out var curPanelInfo))
            {
                //CloseUIPanel(lastPanel.UIPanel.Name);
                curPanelInfo.UIPanel.OnClose();
                if (!curPanelInfo.Paused)
                {
                    curPanelInfo.UIPanel.OnPause();
                    curPanelInfo.Paused = true;
                }
            }
            
            UIPanelInfo uiPanelInfo = GetUIPanelInfo(uiPanel);
            uiPanel.OnOpen(userData);
            if (uiPanelInfo.Paused)
            {
                uiPanel.OnResume();
                uiPanelInfo.Paused = false;
            }
            
            _uiPanelStack.Push(uiPanelInfo);
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="panelName"></param>
        public void CloseUIPanel(string panelName)
        {
            var uiPanel = GetUIPanel(panelName);
            if (uiPanel == null || !uiPanel.IsActive)
            {
                //没有该界面
                return;
            }

            UIPanelInfo uiPanelInfo = GetUIPanelInfo(uiPanel);
            uiPanel.OnClose();
            if (!uiPanelInfo.Paused)
            {
                uiPanel.OnPause();
                uiPanelInfo.Paused = true;
            }

            _uiPanelStack.TryPeek(out var curPanelInfo);
            if (curPanelInfo == uiPanelInfo)
            {
                //出栈
                _uiPanelStack.Pop();

                if (_isOpenStack && _uiPanelStack.TryPeek(out var nextPanelInfo))
                {
                    nextPanelInfo.UIPanel.OnOpen(null);
                    if (nextPanelInfo.Paused)
                    {
                        nextPanelInfo.UIPanel.OnResume();
                        nextPanelInfo.Paused = false;
                    }
                }
            }
        }

        public void RecycleUIPanel(string panelName)
        {
            var uiPanel = GetUIPanel(panelName);
            if (uiPanel == null)
            {
                //没有该界面
                return;
            }

            UIPanelInfo uiPanelInfo = GetUIPanelInfo(uiPanel);
            _uiPanelInfos.Remove(uiPanelInfo);
        }

        public void CloseAll()
        {
            _uiPanelStack.Clear();
            
            foreach (var uiPanelInfo in _uiPanelInfos)
            {
                uiPanelInfo.UIPanel.OnClose();
                if (!uiPanelInfo.Paused)
                {
                    uiPanelInfo.UIPanel.OnPause();
                    uiPanelInfo.Paused = true;
                }
            }
        }

        public void CloseStackPopPanel()
        {
            if (_isOpenStack && _uiPanelStack.Count > 1 && _uiPanelStack.TryPeek(out var curPanelInfo))
            {
                CloseUIPanel(curPanelInfo.UIPanel.Name);
            }
        }

        public void UpdateMatchWidthOrHeight()
        {
            float aspectRatio = (float)Screen.width / Screen.height;
            //屏幕宽高比例大于基准比例时 以height为基准，否则以width为基准
            _canvasScaler.matchWidthOrHeight = aspectRatio > _screen.x / _screen.y ? 1F : 0F;
        }
    }
}