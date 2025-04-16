using Akari.GfCore;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public abstract class ABattleCharacterLoopAttackAction : ABattleCharacterAction
    {
        protected enum Status
        {
            Start,
            Loop,
            End
        }
        
        protected abstract string AttackStartAnimationName { get; }
        protected abstract string AttackLoopAnimationName { get; }
        protected abstract string AttackEndAnimationName { get; }
        
        private bool _isRepeatLoop;
        private float _moveVelocity;
        
        protected Status CurStatus;
        
        //TODO:以下参数通过技能配置来填充
        protected float LoopRemainedTime;
        protected bool CanRotateWhenLoop;
        protected bool CanMoveWhenLoop;
        protected bool HasMoveTest;
        
        public override void OnEnter(AGfFsmState prevAction, bool reenter)
        {
            base.OnEnter(prevAction, reenter);
            CurStatus = Status.Start;
            
            HasMoveTest = Animation.HasClipIndex("MoveTest");
            LoopRemainedTime = 5F;
            CanRotateWhenLoop = true;
            CanMoveWhenLoop = true;
            if (CanMoveWhenLoop)
            {
                _moveVelocity = 0.5f * (Accessor.Condition.MoveSpeedProperty.TotalValue / 100);
            }

            var loopAnimationClipIndex = Animation.GetClipIndex(AttackLoopAnimationName);
            if (loopAnimationClipIndex != GfAnimationComponent.InvalidClipIndex)
            {
                _isRepeatLoop = Animation.GetAnimationClipInfo(loopAnimationClipIndex).IsRepeat;
            }
            else
            {
                _isRepeatLoop = false;
            }
            
            // 根据敌人位置 自动调整方向
            AttackAutoRotate();
        }
        
        public override void OnStart()
        {
            base.OnStart();
            
            AnimationClipIndex = GetAnimationClipIndex();
            Play();
            
            //根据MoveDirection 和 角色朝向 计算是移动权重
            if (CanMoveWhenLoop && HasMoveTest)
            {
                Animation.PlayOtherLayer(1,"MoveTest");
                Animation.SetParameter("Forward", 0.5f);
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            deltaTime *= Animation.Speed;
            
            if (CurStatus == Status.Start)
            {
                if (!Animation.IsThatPlaying(AnimationClipIndex))
                {
                    NextStatus();
                }
                else
                {
                    if (_isRepeatLoop)
                    {
                        RotateWhenLoop();
                        MoveWhenLoop(deltaTime);
                    }
                    EndActionCheck();
                }
            }
            else if(CurStatus == Status.Loop)
            {
                if (_isRepeatLoop)
                {
                    LoopRemainedTime -= deltaTime;
                    if (LoopRemainedTime <= 0)
                    {
                        //Accessor?.Condition.Frame.CanMove.SetBuffer(true, true);
                        NextStatus();
                    }
                    else
                    {
                        RotateWhenLoop();
                        MoveWhenLoop(deltaTime);
                    }
                }
                else
                {
                    EndActionCheck();
                }
            }
            else if (CurStatus == Status.End)
            {
                EndActionCheck();
            }
        }

        public override void OnExit(AGfFsmState nextAction)
        {
            base.OnExit(nextAction);
            if (CanMoveWhenLoop && HasMoveTest)
            {
                Animation.StopOtherLayer(1);
            }
        }

        private void Play()
        {
            if (GfAnimationComponent.InvalidClipIndex == AnimationClipIndex)
            {
                //无效ClipIndex，直接进入下一阶段
                NextStatus();
                return;
            }
            
            Animation.Play(AnimationClipIndex);
        }

        private int GetAnimationClipIndex()
        {
            string animationStateName = string.Empty;
            switch (CurStatus)
            {
                case Status.Start:
                    animationStateName = AttackStartAnimationName;
                    break;
                case Status.Loop:
                    animationStateName = AttackLoopAnimationName;
                    break;
                case Status.End:
                    animationStateName = AttackEndAnimationName;
                    break;
            }
            return Animation.GetClipIndex(animationStateName);
        }

        private void NextStatus()
        {
            if (CurStatus == Status.End)
            {
                return;
            }
            
            CurStatus++;
            AnimationClipIndex = GetAnimationClipIndex();
            Play();
        }

        private void RotateWhenLoop()
        {
            if (!CanRotateWhenLoop)
            {
                return;
            }
            
            if (Accessor.Condition.BattleCharacterType == BattleCharacterType.Enemy)
            {
                return;
            }

            if (true)
            {
                //键鼠
                Rotate(Accessor.Condition.MouseDirection);
            }
            else
            {
                //手柄,需要使用到手柄的右摇杆
                
            }
        }

        private void MoveWhenLoop(float deltaTime)
        {
            if (!CanMoveWhenLoop)
            {
                return;
            }
            
            if (Accessor.Condition.BattleCharacterType == BattleCharacterType.Enemy)
            {
                return;
            }

            if (Accessor.Condition.IsMoving)
            {
                HorizontalMove(deltaTime, Accessor.Condition.MoveDirection, _moveVelocity);

                var localMoveDirection = Accessor.Entity.Transform.Rotation * Accessor.Condition.MoveDirection;
                if (HasMoveTest)
                {
                    //Back(0) => Idle(0.5f) => Forward(1)
                    Animation.SetParameter("Forward", localMoveDirection.Y > 0 ? 0.75f : 0.25F);//蓄力时移动速度减半 所以动画权重是0.75和0.25，移动动画速度减少一半
                }
            }
            else
            {
                if (HasMoveTest)
                {
                    Animation.SetParameter("Forward", 0.5f);
                }
            }
        }
    }
}