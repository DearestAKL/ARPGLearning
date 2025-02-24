using Akari.GfCore;
using Akari.GfGame;
using Akari.GfUnity;
using Cysharp.Threading.Tasks;
using Ryu.InGame.Unity;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Random = UnityEngine.Random;

namespace GameMain.Runtime
{
    public class BattleMain : BaseMain
    {
        public new GameObject camera;
        public bool IsShowProInputCommandsStr;
        public string proInputCommandsStr;
        [Range(1001,1001)]
        public int testCharacterId = 1001;

        //DEBUG
        public bool isFixedUpdate = false;//启用固定帧，防止卡顿导致伤害丢失
        public bool isDebugUpdate = false;//启用调试帧，每秒50次
        
        public bool enableProInput = false;
        public float proInputTime = 0.5f;
        
        private int _fixedCount = 0;
        
        private BattleGameAdministrator _battleGameAdministrator;

        private void Awake()
        {
            BattleHelper.InitAllGfPbFactory();

            var entityComponentSystem = new GfEntityComponentSystem();
            var rootRandomGenerator = new RootRandomGenerator((uint)Random.Range(1, 9999));
            var battleFactory = new BattleUnityFactory();
            var battleGameAdministrator = new BattleGameAdministrator(entityComponentSystem, rootRandomGenerator, battleFactory);
            BattleAdmin.Init(battleGameAdministrator);

            BattleHelper.InitAllComponentSystem();
            
            _battleGameAdministrator = battleGameAdministrator;
            
            BattleUnityAdmin.CreateInstance();
        }

        protected async void Start()
        {
            await CheckYooAssetsInit();
            await ManagerHelper.InitCommonManager(true);

            WorldManager.CreateInstance();
            
            //跳转场景时清空除system外的所有ui栈
            UIManager.Instance.CloseAllExcludeSystem();
            
            EventManager.Instance.BattleEvent.OnBattleStartEvent.GfSubscribe(BattleStart);
            EventManager.Instance.BattleEvent.OnEnterRoomEvent.GfSubscribe(OnEnterRoomEvent);
            EventManager.Instance.BattleEvent.OnExitRoomEvent.GfSubscribe(OnExitRoomEvent);
            
            EventManager.Instance.BattleEvent.OnBattleStartEvent.Invoke();
        }

        private void FixedUpdate()
        {
            if (isFixedUpdate)
            {
                _fixedCount++;
            }
        }

        private void Update()
        {
            if (isFixedUpdate)
            {
                for (int i = 0; i < _fixedCount; i++)
                {
                    _battleGameAdministrator.OnBeginUpdate(Time.fixedDeltaTime);
                    _battleGameAdministrator.OnUpdate(Time.fixedDeltaTime);
                    _battleGameAdministrator.OnEndUpdate(Time.fixedDeltaTime);
                }
                _fixedCount = 0;
            }
            else if(isDebugUpdate)
            {
                _battleGameAdministrator.OnBeginUpdate(0.02f);
                _battleGameAdministrator.OnUpdate(0.02f);
                _battleGameAdministrator.OnEndUpdate(0.02f);
            }
            else
            {
                _battleGameAdministrator.OnBeginUpdate(Time.deltaTime);
                _battleGameAdministrator.OnUpdate(Time.deltaTime);
                _battleGameAdministrator.OnEndUpdate(Time.deltaTime);
            } 

            if (UIManager.HasInstance)
            {
                UIManager.Instance.Update(Time.deltaTime, Time.deltaTime);
            }

            if (WorldManager.HasInstance)
            {
                WorldManager.Instance.Update(Time.deltaTime);
            }
            
            if (BattleUnityAdmin.BattleInput != null && IsShowProInputCommandsStr) 
            {
                BattleUnityAdmin.BattleInput.EnableProInput = enableProInput;
                BattleUnityAdmin.BattleInput.ProInputTime = proInputTime;
                
                var proInputCommands = BattleUnityAdmin.BattleInput.ProInputCommands;
                proInputCommandsStr = "Root";
                for (int i = 0; i < proInputCommands.Count; i++)
                {
                    if (proInputCommands.Nodes[i].Element.Time > 0)
                    {
                        proInputCommandsStr = $"{proInputCommandsStr}=>{proInputCommands.Nodes[i].Element.Type}";
                    }
                }
            }
        }

        private void OnGUI()
        {
            if (IsShowProInputCommandsStr)
            {
                // 设置文本显示的位置
                float x = Screen.width - 250; // 从右边缘向左偏移150像素
                float y = 10; // 从顶部向下偏移10像素

                // 绘制文本
                GUI.Label(new Rect(x, y, 200, 50), proInputCommandsStr);
            }
        }

        private void OnApplicationQuit()
        {
            SettingManager.Instance?.Dispose();
            UserDataManager.Instance?.Dispose();
            WorldManager.Instance?.Dispose();
        }

        private async void BattleStart()
        {
            await CreateBattleInput();
            
            //预载BattleCommonEffect
            foreach (var effectName in BattleCommonEffect.EffectDictionary.Values)
            {
                await AssetManager.Instance.PreloadAsset<GameObject>(AssetPathHelper.GetBattleCommonEffectPath(effectName));
            }
            
            var characterData = RuntimeDataHelper.CreateCharacterData(testCharacterId);
            var entity = await BattleAdmin.Factory.Character.CreateUserCharacter(new GameCharacterModel(characterData),
                GfFloat3.Zero,
                GfQuaternion.Identity);
            BattleAdmin.PlayerEntity = entity;
            CreatBattleCamera();
            
            await WorldManager.Instance.ChangeWorld("World_1");

            await UIManager.Instance.OpenUIPanel(UIType.UIBattlePanel);
            
            //AudioManager.Instance.PlayBgm(Constant.Sound.BgmFondness);

            //召唤物伙伴
            var summonerData = new SummonerData(9001,10);
            await BattleAdmin.Factory.Character.CreateCharacterSummoner(new GameCharacterModel(summonerData),
                GfFloat3.Right,
                GfQuaternion.Identity,"summonerKey");

            UIHelper.EndLoading();
        }

        private GfEntity CreatBattleCamera()
        {
            var entity = BattleAdmin.EntityComponentSystem.Create(0, GfEntityGroupId.Camera, camera.name);
            entity.AddComponent(new GfActorComponent(camera.transform.CreateGfUnityTransform()));

            var cameraActionComponent = new BattleMainCameraActionComponent(new BattleMainCameraActionContext(entity));
            entity.AddComponent(cameraActionComponent);
            cameraActionComponent.Add(BattleMainCameraBasicAction.ActionType, new BattleMainCameraBasicAction());
            cameraActionComponent.ForceTransition(
                new GfFsmStateTransitionRequest(BattleMainCameraBasicAction.ActionType,
                    BattleMainCameraBasicActionData.Create()));

            var battleMainCameraUnityView = camera.gameObject.GetComponent<BattleMainCameraUnityView>();
            var cameraAccessorComponent =
                new BattleMainCameraAccessorComponent(battleMainCameraUnityView, cameraActionComponent);
            battleMainCameraUnityView.SetSelfHandle(entity.ThisHandle);
            entity.AddComponent(cameraAccessorComponent);

            var lookTransform = (BattleAdmin.Player.Transform.Transform as GfUnityTransform)?.GetUnityTransform();
            battleMainCameraUnityView.SetFreeLookFollow(lookTransform);
            battleMainCameraUnityView.SetFreeLookLookAt(lookTransform);

            BattleUnityAdmin.Instance.SetBattleMainCameraUnityView(battleMainCameraUnityView);
            return entity;
        }

        private async UniTask CreateBattleInput()
        {
            var inputGo = await AssetManager.Instance.Instantiate(AssetPathHelper.GetOtherPath("PlayerInput"));
            var playerInput = inputGo.GetComponent<PlayerInput>();
            
            var inputComponentSystem = new BattleInputComponentSystem(playerInput);
            BattleAdmin.EntityComponentSystem.AddComponentSystem(inputComponentSystem);
            BattleUnityAdmin.Instance.SetInput(playerInput, inputComponentSystem);

            InitInputAction();
        }
        
        private void OnEnterRoomEvent()
        {
            BattleAdmin.Instance.ClearBattle();
            WorldManager.Instance.SetWorldActive(false);
        }

        private void OnExitRoomEvent()
        {
            BattleAdmin.Player.Transform.ResetToRecordWorldPosition();
            WorldManager.Instance.SetWorldActive(true);
        }

        #region Input

        private void InitInputAction()
        {
            //=====OtherInput=====
            BattleUnityAdmin.PlayerInput.actions[Constant.InputDef.Return].started += OnReturnStarted;
            BattleUnityAdmin.PlayerInput.actions[Constant.InputDef.GMCommon].started += OnGMCommonStarted;
            BattleUnityAdmin.PlayerInput.actions[Constant.InputDef.GMBattle].started += OnGMBattleStarted;
            
            BattleUnityAdmin.PlayerInput.actions[Constant.InputDef.ShowCursor].performed += OnShowCursorPerformed;
            BattleUnityAdmin.PlayerInput.actions[Constant.InputDef.ShowCursor].canceled += OnShowCursorCanceled;
        }

        //=================Exit=================
        private void OnReturnStarted(InputAction.CallbackContext context)
        {
            if (UIManager.Instance.HasPopUp)
            {
                //有弹窗时暂时屏蔽esc
                return;
            }
            UIManager.Instance.CloseBaseStackPopPanel();
        }
        
        //=================GMCommon=================
        private async void OnGMCommonStarted(InputAction.CallbackContext context)
        {
            if (UIManager.Instance.HasPopUp)
            {
                //有弹窗时暂时屏蔽
                return;
            }

            await UIManager.Instance.OpenUIPanel(UIType.UIGMCommonPanel);
        }
        
        //=================GMBattle=================
        private async void OnGMBattleStarted(InputAction.CallbackContext context)
        {
            if (UIManager.Instance.HasPopUp)
            {
                //有弹窗时暂时屏蔽
                return;
            }
            
            await UIManager.Instance.OpenUIPanel(UIType.UIGMBattlePanel);
        }
        
        private void OnShowCursorPerformed(InputAction.CallbackContext context)
        {
            if (context.interaction is HoldInteraction)
            {
                BattleUnityAdmin.BattleInput.SetBattleInputDisable(true);
            }
        }
        
        private void OnShowCursorCanceled(InputAction.CallbackContext context)
        {
            if (context.interaction is HoldInteraction)
            {
                BattleUnityAdmin.BattleInput.SetBattleInputDisable(false);
            }
        }
        #endregion
    }
}
