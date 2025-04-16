using Akari.GfCore;
using Akari.GfGame;
using Akari.GfUnity;

namespace GameMain.Runtime
{
    public sealed class BattleCharacterDashActionData : ABattleCharacterActionData
    {
        public static BattleCharacterDashActionData Create()
        {
            return Create(() => new BattleCharacterDashActionData());
        }

        public override int ActionType => BattleCharacterDashAction.ActionType;

        public override bool CanTransitionFrom(ABattleCharacterAction currentAction)
        {
            return currentAction.Accessor.Condition.Frame.CanDash.Current;
        }
    }

    public sealed class BattleCharacterDashAction : ABattleCharacterAction
    {
        private BattleCharacterDashActionData _actionData;
        public override ABattleCharacterActionData ActionData => _actionData;

        public const int ActionType = (int)BattleCharacterActionType.Dash;
        
        private static readonly string DashAnimationName = "Dash";
        private static readonly string DashBackAnimationName = "DashBack";

        private bool _canWitchTime = false;
        private bool _hasWitchTime = false;

        public override void OnEnter(AGfFsmState prevAction, bool reenter)
        {
            base.OnEnter(prevAction, reenter);

            _actionData = GetActionData<BattleCharacterDashActionData>();
            
            // 冲刺动作开始时 调整方向
            if (Accessor.Condition.IsMoving)
            {
                Rotate(Accessor.Condition.MoveDirection);
            }

            //处于预警区域 触发魔女时间
            if (Accessor.Condition.IsWarning)
            {
                _canWitchTime = true;
                //切换后处理效果到WitchTime
                BattleAdmin.Instance.ChangeRenderToWitchTime();
                GfLog.Debug("ChangeRenderToWitchTime");
            }
        }
        
        public override void OnStart()
        {
            base.OnStart();
            
            AnimationClipIndex = GetAnimationClipIndex();
            Animation.Play(AnimationClipIndex);


            AudioManager.Instance.PlaySound(Constant.Sound.GetDashSound(Accessor.Condition.IsWarning), true, Accessor.Entity.Transform.Position.ToVector3());
            // BattleAdmin.Factory.Effect.CreateEffectByEntity(Entity,
            //     CommonEffectType.Dash.GetPath(), EffectGroup.OneShot, GfFloat3.Zero,
            //     GfQuaternion.Identity, GfFloat3.One, GfVfxDeleteMode.Delete, GfVfxPriority.High);
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_canWitchTime && Accessor.Condition.Frame.CanDash.Current && Animation.NowFrame > 0)
            {
                _canWitchTime = false;
                _hasWitchTime = true;
                //时间迟缓生效
                BattleAdmin.Instance.StartWitchTime(Entity);
                GfLog.Debug("StartWitchTime");
            }
            
            if (EndActionCheck())
            {
                return;
            }

            if (Accessor.Condition.IsMoving)
            {
                Rotate(Accessor.Condition.MoveDirection);
            }
        }

        public override void OnExit(AGfFsmState nextAction)
        {
            base.OnExit(nextAction);

            if (_hasWitchTime)
            {
                //退出dash 自身速度复原
                _hasWitchTime = false;
                BattleAdmin.Instance.ResetWitchTimeSpeed(Entity);
            }
        }

        private int GetAnimationClipIndex()
        {
            return Animation.GetClipIndex(Accessor.Condition.IsMoving ? DashAnimationName : DashBackAnimationName);
        }

        private void Test()
        {
            foreach (var entity in BattleAdmin.EntityComponentSystem.EntityManager.EntityHandleManager.Buffers)
            {
                if (!entity.IsValid() || entity.Tag == GfEntityTagId.TeamA)
                {
                    continue;
                }

                var time = 3f;
                RenderManager.Instance?.StartWitchTime(time);
                
                entity.Request(new GfChangeSpeedMagnificationRequest(9001,
                    0.1F,
                    time,
                    true));
            }
        }
    }
}