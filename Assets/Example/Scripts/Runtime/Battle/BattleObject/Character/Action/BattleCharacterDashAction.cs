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

        public override void OnEnter(AGfFsmState prevAction, bool reenter)
        {
            base.OnEnter(prevAction, reenter);

            _actionData = GetActionData<BattleCharacterDashActionData>();
            
            // 冲刺动作开始时 调整方向
            if (Accessor.Condition.IsMoving)
            {
                Rotate(Accessor.Condition.MoveDirection);
            }
        }
        
        public override void OnStart()
        {
            base.OnStart();
            
            AnimationClipIndex = GetAnimationClipIndex();
            Animation.Play(AnimationClipIndex);


            AudioManager.Instance.PlaySound(Constant.Sound.GetRandomDashSound(), true, Accessor.Entity.Transform.Position.ToVector3());
            // BattleAdmin.Factory.Effect.CreateEffectByEntity(Entity,
            //     CommonEffectType.Dash.GetPath(), EffectGroup.OneShot, GfFloat3.Zero,
            //     GfQuaternion.Identity, GfFloat3.One, GfVfxDeleteMode.Delete, GfVfxPriority.High);
        }

        public override void OnUpdate(float deltaTime)
        {
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
            //Entity.Request(new GhostEnableRequest(false));
        }

        private int GetAnimationClipIndex()
        {
            return Animation.GetClipIndex(DashAnimationName);
        }
    }
}