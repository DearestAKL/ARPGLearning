namespace GameMain.Runtime
{
    //区域类shell
    public class BattleShellAreaComponent : ABattleShellComponent
    {
        private readonly float _duration;

        private float _elapsedTime = 0f;
        
        public BattleShellAreaComponent(BattleShellDamageCauserHandler shellDamageCauserHandler,float duration) : base(shellDamageCauserHandler)
        {
            _duration = duration;
        }

        protected override void OnAwakeInternal()
        {

        }

        protected override void OnStartInternal()
        {
            RegisterAttack(true);
        }

        protected override void OnUpdateInternal(float deltaTime)
        {
            _elapsedTime += deltaTime;
            if (_elapsedTime > _duration)
            {
                _elapsedTime = 0;
                RegisterAttack(false);
                Delete();
            }
        }

        protected override void OnDeleteInternal()
        {

        }
        
        private void RegisterAttack(bool isOn)
        {
            Entity.Request(new BattleRegisterColliderAttackRequest(isOn, AttackId.CreateForShell((uint)ShellDamageCauserHandler.AttackDefinition.AttackId)));
        }
    }
}