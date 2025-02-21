namespace GameMain.Runtime
{
    public abstract class ABattleMainCameraActionData : BattleActionData
    {
        public abstract int ActionType { get; }
        public virtual int Priority => 1000 - ActionType;
    }
}
