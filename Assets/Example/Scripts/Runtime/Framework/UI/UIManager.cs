using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Akari.GfCore;
using Cysharp.Threading.Tasks;

namespace GameMain.Runtime
{
    public class UIManager : GfSingleton<UIManager>
    {
        private Dictionary<string, UIGroup> _uiGroups = new Dictionary<string, UIGroup>();
        private readonly Dictionary<string, Type> _uiTypeMap = new Dictionary<string, Type>();
        private Transform _root;
        private Camera _uiCamera;
        private Camera _mainCamera;
        private Transform _battleGroupTransform;

        private UIFactory _factory;
        public UIFactory Factory => _factory;
        public Camera UICamera => _uiCamera;
        public Camera MainCamera => _mainCamera;
        public Transform BattleGroupTransform => _battleGroupTransform;

        public bool HasPopUp { get; private set; }

        protected override void OnCreated()
        {
            _root = new GameObject("UIRoot").transform;
            Object.DontDestroyOnLoad(_root);
            
            _factory = new UIFactory();
        }
        
        protected override void OnDisposed()
        {
            _uiGroups.Clear();
        }

        public async UniTask Init()
        {
            var cameras = await AssetManager.Instance.Instantiate(AssetPathHelper.GetOtherPath("Cameras"), _root);
            _uiCamera = cameras.transform.Find("UI Camera").GetComponent<Camera>();
            _mainCamera = cameras.transform.Find("Main Camera").GetComponent<Camera>();
            
            InitCanvas();
            InitUITypeMap();
        }

        /// <summary>
        /// 初始化生成Canvas
        /// </summary>
        private void InitCanvas()
        {
            AddUIGroup(UIGroupType.Battle, 0, _root, false);
            AddUIGroup(UIGroupType.Base, 100, _root, true);
            AddUIGroup(UIGroupType.MidCommon, 200, _root, false);
            AddUIGroup(UIGroupType.Top, 300, _root, true);
            AddUIGroup(UIGroupType.FirstPopUp, 400, _root, false);
            AddUIGroup(UIGroupType.SecondPopUp, 500, _root, false);
            AddUIGroup(UIGroupType.SystemPopUp, 600, _root, false);

            _battleGroupTransform = GetUIGroup(UIGroupType.Battle).RootGo.transform;
        }
        
        private void InitUITypeMap()
        {
            _uiTypeMap.Add(UIType.UIMainMenuPanel,typeof(UIMainMenuPanel));
            _uiTypeMap.Add(UIType.UISettingPanel,typeof(UISettingPanel));
            //_uiTypeMap.Add(UIType.UICommonMessageDialog,typeof(UICommonMessageDialog));
            _uiTypeMap.Add(UIType.UILoading,typeof(UILoading));
            _uiTypeMap.Add(UIType.UIBattlePanel,typeof(UIBattlePanel));
            _uiTypeMap.Add(UIType.UIBlessingChoicesDialog,typeof(UIBlessingChoicesDialog));
            _uiTypeMap.Add(UIType.UICommonMessageDialog,typeof(UICommonMessageDialog));
            _uiTypeMap.Add(UIType.UICharacterPanel,typeof(UICharacterPanel));
            _uiTypeMap.Add(UIType.UISideMenuPanel,typeof(UISideMenuPanel));
            _uiTypeMap.Add(UIType.UITipsDialog,typeof(UITipsDialog));
            _uiTypeMap.Add(UIType.UIAttributeDetailDialog,typeof(UIAttributeDetailDialog));
            _uiTypeMap.Add(UIType.UICharacterAscensionPanel,typeof(UICharacterAscensionPanel));
            _uiTypeMap.Add(UIType.UIGMCommonPanel,typeof(UIGMCommonPanel));
            _uiTypeMap.Add(UIType.UIRewardDialog,typeof(UIRewardDialog));
            _uiTypeMap.Add(UIType.UIBackpackPanel,typeof(UIBackpackPanel));
            _uiTypeMap.Add(UIType.UISideFilterDialog,typeof(UISideFilterDialog));
            _uiTypeMap.Add(UIType.UIFishPanel,typeof(UIFishPanel));
            _uiTypeMap.Add(UIType.UIGMBattlePanel,typeof(UIGMBattlePanel));
        }

        public Type GetUIPanelType(string panelName)
        {
            _uiTypeMap.TryGetValue(panelName, out Type type);
            return type != null ? type : null;
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach (KeyValuePair<string, UIGroup> uiGroup in _uiGroups)
            {
                uiGroup.Value.OnUpdate(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 添加界面组
        /// </summary>
        /// <param name="uiGroupName"></param>
        /// <param name="uiGroupDepth"></param>
        /// <param name="parent"></param>
        public void AddUIGroup(string uiGroupName,int uiGroupDepth,Transform parent,bool isOpenStack)
        {
            _uiGroups.Add(uiGroupName, new UIGroup(uiGroupName, uiGroupDepth, parent,isOpenStack));
        }

        /// <summary>
        /// 获取界面组
        /// </summary>
        /// <param name="uiGroupName"></param>
        /// <returns></returns>
        public UIGroup GetUIGroup(string uiGroupName)
        {
            return _uiGroups[uiGroupName];
        }

        /// <summary>
        /// 获取界面
        /// </summary>
        /// <param name="panelName"></param>
        /// <param name="uiGroupName"></param>
        /// <returns></returns>
        public UIPanel GetUIPanel(string panelName)
        {
            var uiGroupName = UIType.GetGroupTypeByPanelName(panelName);
            
            var group = GetUIGroup(uiGroupName);
            var uiPanel = group.GetUIPanel(panelName);
            return uiPanel;
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="panelName"></param>
        /// <param name="uiGroupName"></param>
        /// <param name="userData"></param>
        public async UniTask OpenUIPanel(string panelName, object userData = null)
        {
            var uiGroupName = UIType.GetGroupTypeByPanelName(panelName);
            if (UIGroupType.IsPopUpGroupType(uiGroupName))
            {
                HasPopUp = true;
            }
            
            var group = GetUIGroup(uiGroupName);
            await group.OpenUIPanel(panelName, userData);
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="panelName"></param>
        /// <param name="uiGroupName"></param>
        public void CloseUIPanel(string panelName)
        {
            var uiGroupName = UIType.GetGroupTypeByPanelName(panelName);
            var group = GetUIGroup(uiGroupName);
            group.CloseUIPanel(panelName);
            
            if (UIGroupType.IsPopUpGroupType(uiGroupName))
            {
                HasPopUp = CheckHasPopUp();
            }
        }

        /// <summary>
        /// 回收界面
        /// </summary>
        /// <param name="panelName"></param>
        /// <param name="canvasType"></param>
        public void RecycleUIPanel(string panelName)
        {
            var uiGroupName = UIType.GetGroupTypeByPanelName(panelName);
            var group = GetUIGroup(uiGroupName);
            group.RecycleUIPanel(panelName);
        }

        public void CloseAllExcludeSystem()
        {
            foreach (var group in _uiGroups)
            {
                if (group.Key != UIGroupType.SystemPopUp)
                {
                    group.Value.CloseAll();  
                }
            }
        }

        public void CloseBaseStackPopPanel()
        {
            var baseGroup = GetUIGroup(UIGroupType.Base);
            baseGroup.CloseStackPopPanel();
        }

        public void UpdateMatchWidthOrHeight()
        {
            foreach (var group in _uiGroups.Values)
            {
                group.UpdateMatchWidthOrHeight();
            }
        }

        private bool CheckHasPopUp()
        {
            foreach (var uiGroupName in UIGroupType.PopUpGroupTypes)
            {
                var group = GetUIGroup(uiGroupName);
                if (group.CurrentUIPanel != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
    
    public static class UIGroupType
    {
        public const string Battle = "Battle";
        public const string Base = "Base";
        public const string MidCommon = "MidCommon";
        public const string Top = "Top";
        public const string FirstPopUp = "FirstPopUp";
        public const string SecondPopUp = "SecondPopUp";
        public const string SystemPopUp = "SystemPopUp";

        public static readonly List<string> PopUpGroupTypes = new List<string>()
        {
            FirstPopUp,
            SecondPopUp,
            SystemPopUp,
        };
        
        public static bool IsPopUpGroupType(string groupType)
        {
            return PopUpGroupTypes.Contains(groupType);
        }
    }

    public static class UIType
    {
        public const string UIMainMenuPanel = "UIMainMenuPanel";
        public const string UISettingPanel = "UISettingPanel";
        public const string UICommonMessageDialog = "UICommonMessageDialog";
        public const string UILoading = "UILoading";
        public const string UIBattlePanel = "UIBattlePanel";
        public const string UIBlessingChoicesDialog = "UIBlessingChoicesDialog";
        public const string UICharacterPanel = "UICharacterPanel";
        public const string UISideMenuPanel = "UISideMenuPanel";
        public const string UITipsDialog = "UITipsDialog";
        public const string UIAttributeDetailDialog = "UIAttributeDetailDialog";
        public const string UICharacterAscensionPanel = "UICharacterAscensionPanel";
        public const string UIGMCommonPanel = "UIGMCommonPanel";
        public const string UIRewardDialog = "UIRewardDialog";
        public const string UIBackpackPanel = "UIBackpackPanel";
        public const string UISideFilterDialog = "UISideFilterDialog";
        public const string UIFishPanel = "UIFishPanel";
        public const string UIGMBattlePanel = "UIGMBattlePanel";

        private static readonly Dictionary<string, string> UITypeGroupMap = new Dictionary<string, string>()
        {
            //Window Base 应用界面栈，只显示最上层
            {UIType.UIMainMenuPanel,UIGroupType.Base},
            {UIType.UIBattlePanel,UIGroupType.Base},
            {UIType.UICharacterPanel,UIGroupType.Base},
            {UIType.UISideMenuPanel,UIGroupType.Base},
            {UIType.UICharacterAscensionPanel,UIGroupType.Base},
            {UIType.UIBackpackPanel,UIGroupType.Base},
            {UIType.UIGMCommonPanel,UIGroupType.Base},
            {UIType.UISettingPanel,UIGroupType.Base},

            //Window Mid 可全部显示
            {UIType.UIFishPanel,UIGroupType.MidCommon},
            {UIType.UIGMBattlePanel,UIGroupType.MidCommon},
            
            //Window Top 应用界面栈，只显示最上层

            //FirstPopUp 第一弹窗层
            {UIType.UIBlessingChoicesDialog,UIGroupType.FirstPopUp},
            {UIType.UIAttributeDetailDialog,UIGroupType.FirstPopUp},
            
            {UIType.UIRewardDialog,UIGroupType.FirstPopUp},
            {UIType.UISideFilterDialog,UIGroupType.FirstPopUp},

            //SecondPopUp 第二弹窗层


            //SystemPopUp 系统弹窗层
            {UIType.UILoading,UIGroupType.SystemPopUp},
            {UIType.UICommonMessageDialog,UIGroupType.SystemPopUp},
            {UIType.UITipsDialog,UIGroupType.SystemPopUp},
        };

        public static string GetGroupTypeByPanelName(string panelType)
        {
            UITypeGroupMap.TryGetValue(panelType, out var groupType);
            return groupType;
        }
    }
    
}
