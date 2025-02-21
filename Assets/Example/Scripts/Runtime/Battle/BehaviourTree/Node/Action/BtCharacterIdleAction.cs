namespace GameMain.Runtime
{
    public class BtCharacterIdleAction : ABtCharacterAction
    {
        public BtCharacterIdleAction() : base("BtCharacterIdleAction")
        {
        }

        protected override void DoStart()
        {
            var actionData = BattleCharacterIdleActionData.Create();
            SendRequest(actionData);
        }

        protected override void DoStop()
        {
            Stopped(false);
        }
    }
}