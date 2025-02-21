using Akari.GfCore;

namespace GameMain.Runtime
{
    public sealed class BattleMainCameraBasicActionData : ABattleMainCameraActionData
    {
        public override int ActionType => BattleMainCameraBasicAction.ActionType;

        public static BattleMainCameraBasicActionData Create()
        {
            var data = Create(() => new BattleMainCameraBasicActionData());
            return data;
        }
    }

    public sealed class BattleMainCameraBasicAction : ABattleMainCameraAction
    {
        private BattleMainCameraBasicActionData _actionData;

        public const int ActionType = (int)BattleMainCameraActionType.Basic;
        public override void OnEnter(AGfFsmState prevAction, bool reenter)
        {
            base.OnEnter(prevAction, reenter);

            _actionData = GetActionData<BattleMainCameraBasicActionData>();
        }
    }
}
