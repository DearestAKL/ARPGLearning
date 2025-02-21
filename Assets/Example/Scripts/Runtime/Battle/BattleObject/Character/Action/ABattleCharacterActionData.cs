namespace GameMain.Runtime
{
    public abstract partial class ABattleCharacterActionData : BattleActionData
    {
        public abstract int ActionType { get; }
        
        public bool                            IsRequestCanceled     { get; private set; }
        
        public virtual int Priority => 1000 - ActionType;
        
        public virtual bool CanTransitionFrom(ABattleCharacterAction currentAction)
        {
            return true;
        }
    }
}