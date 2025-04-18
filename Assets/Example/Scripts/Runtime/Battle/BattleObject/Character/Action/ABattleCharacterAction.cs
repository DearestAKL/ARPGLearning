using System.Collections.Generic;
using Akari.GfCore;
using Akari.GfGame;
using UnityEngine;

namespace GameMain.Runtime
{
    public enum BattleCharacterActionType : int
    {
        None = 0,
        
        // =========================
        
        // 主动动作
        // =========================
        Idle = 1,
        
        MoveWalk = 2,
        MoveRun = 3,
        MoveSprint = 4,
        
        //Turn = 5, 
        MoveMixed = 6, 
        
        Dash = 9,

        LightAttack = 10,
        ChargeAttack = 11,
        SpecialAttack = 12,
        Ultimate = 13,
        
        Defend = 20,

        PlayAnimationState = 30,
        
        JumpTo = 40,

        // =========================
        // 被动动作
        // =========================
        GetHit = 100, // 受击小
        KnockBack = 101, // 被击退
        KnockUp = 102, // 被击飞
        
        DefendHit = 105,//防御状态下受击
        
        Dizzy = 110,//眩晕
        
        Die = 120, // 死亡
    }


    public abstract class ABattleCharacterAction : AGfActionState<BattleObjectActionContext>
    {
        private class Transition : IGfFsmStateTransition
        {
            public    int                    JudgeOrder => 0;
            protected ABattleCharacterAction Action     { get; private set; }

            public Transition(ABattleCharacterAction action)
            {
                Action = action;
            }

            public virtual bool CanTransition(GfFsmStateTransitionRequest request)
            {
                var actionData = request.NextUserData as ABattleCharacterActionData;
                if (actionData == null)
                {
                    return false;
                }

                if (actionData.IsRequestCanceled)
                {
                    return false;
                }

                var ret = actionData.CanTransitionFrom(Action);
                return ret;
            }

            public void Dispose()
            {
                Action = null;
            }
        }
        
        private BattleCharacterAccessorComponent  _accessorCache;
        private GfAnimationComponent              _animationCache;

        public BattleCharacterAccessorComponent Accessor => _accessorCache;
        protected GfAnimationComponent Animation => _animationCache;
        protected int AnimationClipIndex { get; set; }
        
        private List<GfHandle> _effHandles = new List<GfHandle>();

        //public bool IsReadyToTransitionToInitiativeActions { get; set; }

        public abstract ABattleCharacterActionData ActionData { get; }
        public override void OnAwake()
        {
            SetTransition();

            _accessorCache = GetComponent<BattleCharacterAccessorComponent>();
            _animationCache = GetComponent<GfAnimationComponent>();
        }

        public override void OnEnter(AGfFsmState prevAction, bool reenter)
        {
            //IsReadyToTransitionToInitiativeActions = false;
            _effHandles.Clear();
        }

        public override void OnStart()
        {
        }

        public override void OnBeginUpdate(float deltaTime)
        {
        }

        public override void OnUpdate(float deltaTime)
        {
        }

        public override void OnEndUpdate(float deltaTime)
        {
        }

        public override void OnExit(AGfFsmState nextAction)
        {
            RemoveAllEffect();
        }

        public override void OnDelete()
        {

        }
        
        public void AddEffectHandle(GfHandle handle)
        {
            _effHandles.Add(handle);
        }
        
        private void RemoveAllEffect()
        {
            foreach (var handle in _effHandles)
            {
                if (BattleAdmin.VfxManager.IsAlive(handle))
                {
                    BattleAdmin.VfxManager.ForceRemove(handle);
                }
            }

            _effHandles.Clear();
        }
        
        protected void SetTransition()
        {
            AddTransition(new Transition(this));
        }
        
        protected void RequestForChange(ABattleCharacterActionData actionData)
        {
            RequestForChangeAction<BattleObjectActionComponent>(actionData.ActionType, actionData, actionData.Priority);
        }

        protected void EndActionAndRequestForChange(ABattleCharacterActionData actionData)
        {
            //IsReadyToTransitionToInitiativeActions = true;

            RequestForChange(actionData);
        }

        protected bool EndActionCheck()
        {
            ABattleCharacterActionData nextActionData = null;
            
            if (Accessor.Condition.IsMoving && Accessor.Condition.Frame.CanMove.Current)
            {
                if (ActionData.ActionType == (int)BattleCharacterActionType.Dash)
                {
                    nextActionData = BattleCharacterMoveSprintActionData.Create();
                }
                else if (Accessor.Condition.HasLockTarget)
                {
                    nextActionData = BattleCharacterMoveMixedActionData.Create();
                }
                else if (Accessor.Condition.IsWalk)
                {
                    nextActionData = BattleCharacterMoveWalkActionData.Create();
                }
                else
                {
                    nextActionData = BattleCharacterMoveRunActionData.Create();
                }
            }
            else
            {
                if (!Animation.IsThatPlaying(AnimationClipIndex))
                {
                    nextActionData = BattleCharacterIdleActionData.Create();
                }
            }


            if (nextActionData != null && nextActionData.ActionType != ActionData.ActionType) 
            {
                EndActionAndRequestForChange(nextActionData);
                return true;
            }

            return false;
        }

        protected void HorizontalMove(float deltaTime, GfFloat2 vector, float velocity)
        {
            var diffPosition = vector * velocity * deltaTime;
            
            Accessor.Transform.MovePosition(diffPosition.ToXZFloat3());
        }

        protected void VerticalMove(float deltaTime, float velocity)
        {
            var diffPosition = velocity * deltaTime;

            Accessor.Transform.MovePosition(new GfFloat3(0, diffPosition, 0));
        }

        protected void UpdateMoveRotate()
        {
            if (Accessor.Condition.HasLockTarget)
            {
                Rotate(Accessor.Condition.CharacterDirection);
            }
            else
            {
                Rotate(Accessor.Condition.MoveDirection);
            }
        }
        
        protected void Rotate(GfFloat2 targetRotation)
        {
            if (targetRotation.SqrMagnitude <= 0.1f)
            {
                return;
            }

            var transform = Accessor.Entity.Transform;
            transform.Rotation = GfQuaternion.LookRotation(targetRotation.ToXZFloat3());
        }

        /// <summary>
        /// 如果是键鼠，则会根据鼠标方向调整
        /// 如果是手柄，则会根据手柄移动输入方向调整
        /// 如果是AI，则会根据目标方向调整
        /// </summary>
        protected void AttackAutoRotate()
        {
            if (Accessor.Condition.BattleCharacterType == BattleCharacterType.Enemy)
            {
                var targetAccessor = Accessor.Condition.Target;
                if (targetAccessor != null && targetAccessor.Entity != null)
                {
                    var direction = targetAccessor.Entity.Transform.Position - Accessor.Entity.Transform.Position;
                    Rotate(direction.ToXZFloat2().Normalized); 
                    return;
                }
            }
            
            if (true)
            {
                //键鼠
                Rotate(Accessor.Condition.MouseDirection);
            }
            else
            {
                //手柄
                Rotate(Accessor.Condition.MoveDirection);
            }
        }
    }
}