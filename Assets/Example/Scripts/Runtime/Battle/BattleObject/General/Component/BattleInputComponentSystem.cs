using System.Collections.Generic;
using Akari.GfCore;
using Akari.GfGame;
using Akari.GfUnity;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Interactions;
using Utils;

namespace GameMain.Runtime
{
    /// <summary>
    /// 战斗输入管理系统
    /// </summary>
    public sealed class BattleInputComponentSystem : AGfManagerTypeComponentSystem
    {
        private PlayerInput _playerInput;
        private InputAction _moveInputAction;
        private InputAction _scrollInputAction;

        private IBattleCharacterAccessorComponent PlayerAccessor => BattleAdmin.Player;


        private LayerMask _layerMask;

        private bool _isMoving = false;
        private GfFloat2 _moveDirection = GfFloat2.Zero;
        private GfFloat2 _mouseDirection = GfFloat2.Zero;
        
        //长按状态
        private bool _fireHolding = false;
        private bool _dashHolding = false;
        //private bool _defendHolding = false;

        private readonly GfRequestResult _curProInputRequestResult = new GfRequestResult();
        private bool _hasProInputRequestResult;

        public bool EnableProInput;
        public float ProInputTime;

        private bool _disableBattleInput;

        private bool DisableBattleInput => BattleAdmin.Time.IsPaused || _disableBattleInput || PlayerAccessor == null;
        
        public struct ProInputCommand
        {
            public InputType Type;
            public int Priority;
            public float Time;

            public ProInputCommand(InputType type,float time)
            {
                Type = type;

                Priority = 10 - (int)type;
                Time = time;
            }
        }
        
        
        //输入指令
        public enum InputType
        {
            LightAttack,
            ChargeAttack,
            SpecialAttack,
            //Dash,//冲刺不适合预先输入 直接响应就行
            Ultimate,
        }

        private List<ProInputCommand> _proInputCommandAddCache = new List<ProInputCommand>();
        private PriorityQueue<ProInputCommand,int> _proInputCommands = new PriorityQueue<ProInputCommand,int>();
        public PriorityQueue<ProInputCommand, int> ProInputCommands => _proInputCommands;

        public BattleInputComponentSystem(PlayerInput playerInput,int sortingOrder = DefaultOrder): base(sortingOrder)
        {
            _playerInput = playerInput;
            
            //=====BattleInput=====
            _playerInput.actions[Constant.InputDef.BasicAttack].started += OnBasicAttackStarted;
            _playerInput.actions[Constant.InputDef.BasicAttack].performed += OnBasicAttackPerformed;
            _playerInput.actions[Constant.InputDef.BasicAttack].canceled  += OnBasicAttackCanceled;
            
            _playerInput.actions[Constant.InputDef.Dash].started += OnDashStarted;
            _playerInput.actions[Constant.InputDef.Dash].performed += OnDashPerformed;
            _playerInput.actions[Constant.InputDef.Dash].canceled += OnDashCanceled;
            
            _playerInput.actions[Constant.InputDef.SpecialAttack].started += OnSpecialAttackStarted;
            _playerInput.actions[Constant.InputDef.Ultimate].started += OnUltimateStarted;
            
            _playerInput.actions[Constant.InputDef.WalkSwitch].started += OnWalkSwitchStarted;
            
            _moveInputAction = _playerInput.actions[Constant.InputDef.Move];
            _layerMask = LayerMask.GetMask(Constant.Layer.Ground);
        }


        public void SetBattleInputDisable(bool isDisable)
        {
            _disableBattleInput = isDisable;
        }

        public override void OnBeginUpdate(float deltaTime)
        {
            if (EnableProInput)
            {
                if (_hasProInputRequestResult)
                {
                    if (_curProInputRequestResult.HasResult)
                    {
                        _hasProInputRequestResult = false;
                        if (_curProInputRequestResult.IsSuccess)
                        {
                            if (_proInputCommands.Count > 0)
                            {
                                _proInputCommands.Dequeue();
                            }
                        }

                        for (int i = 0; i < _proInputCommandAddCache.Count; i++)
                        {
                            AddInputCommand(_proInputCommandAddCache[i]);
                        }
                        _proInputCommandAddCache.Clear();
                    }
                }
                
                if (_proInputCommands.Count > 0)
                {
                    ProcessProInput(_proInputCommands.Peek());
                }
            
                while (_proInputCommands.Count > 0)
                {
                    var input = _proInputCommands.Peek();
                    if (Time.time - input.Time > ProInputTime)
                    {
                        // 超时，丢弃输入指令
                        _proInputCommands.Dequeue();
                    }
                    else
                    {
                        // 指令尚未超时，停止检查
                        break;
                    }
                }
            }
            else
            {
                if (_proInputCommands.Count > 0)
                {
                    _proInputCommands.Clear();
                }
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            if (PlayerAccessor == null)
            {
                return;
            }

            if (DisableBattleInput)
            {
                _isMoving = false;
                _moveDirection = GfFloat2.Zero;
                _mouseDirection = GfFloat2.Zero;
                _dashHolding = false;
                _fireHolding = false;
            }
            else
            {
                //Player
                var moveInput = _moveInputAction.ReadValue<Vector2>();
                if (moveInput.SqrMagnitude() > 0.01F)
                {
                    moveInput = moveInput.normalized;
                
                    _isMoving = true;
                
                    var transform = BattleUnityAdmin.BattleMainCameraView.MainCamera.transform;
                    var forward = transform.forward;
                    var right = transform.right;

                    Vector3 moveDirection = (moveInput.x * right + moveInput.y * forward).normalized;
                    _moveDirection = moveDirection.ToGfFloat3().ToXZFloat2().Normalized;
                }
                else
                {
                    _isMoving = false;
                    _moveDirection = GfFloat2.Zero;
                }
            
                //Mouse
                Ray ray = UIManager.Instance.MainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray,out  RaycastHit raycastHit,float.MaxValue,_layerMask))
                {
                    _mouseDirection = new GfFloat2(raycastHit.point.x - PlayerAccessor.Entity.Transform.Position.X,
                        raycastHit.point.z - PlayerAccessor.Entity.Transform.Position.Z).Normalized;
                }
                else
                {
                    _mouseDirection = GfFloat2.Zero;
                }
            }

            PlayerAccessor.Condition.UpdateInput(_isMoving, _moveDirection, _mouseDirection, _dashHolding,
                _fireHolding);
        }

        public override void OnEndUpdate(float deltaTime)
        {
 
        }
        
        //=================BasicAttack=================
        
        private void OnBasicAttackStarted(InputAction.CallbackContext context)
        {
            if (DisableBattleInput || EventSystemUtility.IsPointerOverGUIAction()) { return; }

            if (context.interaction is TapInteraction)
            {
                if (EnableProInput)
                {
                    AddInputCommand(InputType.LightAttack);
                }
                else
                {
                    var nextActionData = BattleCharacterLightAttackActionData.Create();
                    RequestForChangeAction(nextActionData);
                }
            }
        }

        private void OnBasicAttackPerformed(InputAction.CallbackContext context)
        {
            if (DisableBattleInput || EventSystemUtility.IsPointerOverGUIAction()) { return; }
            
            if (context.interaction is HoldInteraction)
            {
                _fireHolding = true;
                if (EnableProInput)
                {
                    AddInputCommand(InputType.ChargeAttack);
                }
                else
                {
                    var nextActionData = BattleCharacterChargeAttackActionData.Create();
                    RequestForChangeAction(nextActionData);
                }
            }
        }

        private void OnBasicAttackCanceled(InputAction.CallbackContext context)
        {
            if (DisableBattleInput || EventSystemUtility.IsPointerOverGUIAction()) { return; }
            
            if (context.interaction is HoldInteraction)
            {
                _fireHolding = false;
            }
        }
        //=================Dash=================
        
        private void OnDashStarted(InputAction.CallbackContext context)
        {
            if (DisableBattleInput) { return; }
            
            if (context.interaction is TapInteraction)
            {
                var nextActionData = BattleCharacterDashActionData.Create();
                RequestForChangeAction(nextActionData);
            }
        }

        private void OnDashPerformed(InputAction.CallbackContext context)
        {
            if (DisableBattleInput) { return; }
            
            if (context.interaction is HoldInteraction)
            {
                _dashHolding = true;
            }
        }
        
        private void OnDashCanceled(InputAction.CallbackContext context)
        {
            if (DisableBattleInput) { return; }
            
            if (context.interaction is HoldInteraction)
            {
                _dashHolding = false;
            }
        }
        
        //=================Defend=================
        
        // private void OnDefendStarted(InputAction.CallbackContext context)
        // {
        //     var nextActionData = BattleCharacterDefendActionData.Create();
        //     RequestForChangeAction(nextActionData);
        // }
        //
        // private void OnDefendPerformed(InputAction.CallbackContext context)
        // {
        //     if (context.interaction is HoldInteraction)
        //     {
        //         _dashHolding = true;
        //     }
        // }
        //
        // private void OnDefendCanceled(InputAction.CallbackContext context)
        // {
        //     if (context.interaction is HoldInteraction)
        //     {
        //         _dashHolding = false;
        //     }
        // }
        
        //=================SpecialAttack=================
        
        private void OnSpecialAttackStarted(InputAction.CallbackContext context)
        {
            if (DisableBattleInput) { return; }
            
            if (context.interaction is TapInteraction)
            {
                // TODO:改用ActiveSkillComponent统一控制
                //检测是否能释放
                if (PlayerAccessor.Condition.GetTargetCoolTime(0)?.CurrentSlotNum < 1)
                {
                    //CD中
                    return;
                }
                
                if (EnableProInput)
                {
                    AddInputCommand(InputType.SpecialAttack);
                }
                else
                {
                    var nextActionData = BattleCharacterSpecialAttackActionData.Create();
                    RequestForChangeAction(nextActionData);
                }
            }
        }
        
        //=================Ultimate=================
        
        private void OnUltimateStarted(InputAction.CallbackContext context)
        {
            if (DisableBattleInput) { return; }
            
            if (context.interaction is TapInteraction)
            {
                //检测是否能释放
                if (PlayerAccessor.Condition.GetTargetCoolTime(1)?.CurrentSlotNum < 1)
                {
                    //CD中
                    return;
                }
                
                if (EnableProInput)
                {
                    AddInputCommand(InputType.Ultimate);
                }
                else
                {
                    var nextActionData = BattleCharacterUltimateActionData.Create();
                    RequestForChangeAction(nextActionData);
                }
            }
        }
        
        //=================WalkSwitch=================
        private void OnWalkSwitchStarted(InputAction.CallbackContext context)
        {
            if (DisableBattleInput) { return; }
            
            PlayerAccessor.Condition.IsWalk = !PlayerAccessor.Condition.IsWalk;
            GfLog.Debug("OnWalkSwitchStarted");
        }

        private void RequestForChangeAction(ABattleCharacterActionData actionData)
        {
            PlayerAccessor.Entity.Request(new GfChangeActionRequest<BattleObjectActionComponent>(actionData.ActionType, actionData, actionData.Priority));
        }

        private void AddInputCommand(InputType inputType)
        {
            var newProInputCommand = new ProInputCommand(inputType,Time.time);
            AddInputCommand(newProInputCommand);
        }

        private void AddInputCommand(ProInputCommand proInputCommand)
        {
            if (_hasProInputRequestResult)
            {
                _proInputCommandAddCache.Add(proInputCommand);
                return;
            }
            
            if (_proInputCommands.Count > 10)
            {
                if (proInputCommand.Priority > _proInputCommands.Peek().Priority)
                {
                    //优先级更高
                    _proInputCommands.Enqueue(proInputCommand, proInputCommand.Priority);
                }
                return;
            }

            _proInputCommands.Enqueue(proInputCommand, proInputCommand.Priority);
        }

        private void ProcessProInput(ProInputCommand inputCommand)
        {
            if (_hasProInputRequestResult)
            {
                return;
            }
            
            if (inputCommand.Type == InputType.SpecialAttack || inputCommand.Type == InputType.Ultimate)
            {
                if (!PlayerAccessor.Condition.Frame.CanDash.Current)
                {
                    return;
                }
            }
            else
            {
                if (!PlayerAccessor.Condition.Frame.CanAttack.Current)
                {
                    return;
                }
            }
            
            ABattleCharacterActionData nextActionData = null;

            if(inputCommand.Type == InputType.LightAttack)
            {
                nextActionData = BattleCharacterLightAttackActionData.Create();
            }
            else if (inputCommand.Type == InputType.ChargeAttack)
            {
                nextActionData = BattleCharacterChargeAttackActionData.Create();
            }
            else if(inputCommand.Type == InputType.SpecialAttack)
            {
                nextActionData = BattleCharacterSpecialAttackActionData.Create();
            }
            else if(inputCommand.Type == InputType.Ultimate)
            {
                nextActionData = BattleCharacterUltimateActionData.Create();
            }
            
            if (nextActionData != null)
            {
                _hasProInputRequestResult = true;
                _curProInputRequestResult.Clear();
                var request = new GfChangeActionRequest<BattleObjectActionComponent>(nextActionData.ActionType, nextActionData, nextActionData.Priority, _curProInputRequestResult);
                PlayerAccessor.Entity.Request(request);
            }
        }
    }
}
