using Akari.GfCore;

namespace GameMain.Runtime
{
    //TODO 碰到建筑物体也要销毁,这应该怎么处理呢
    public sealed class BattleShellBulletComponent : ABattleShellComponent
    {
        private readonly GfFloat3 _direction;
        private readonly float _duration;
        private readonly float _speed;
        private readonly BulletType _bulletType;
        private float _elapsedTime = 0f;
        
        public BattleShellBulletComponent(
            BattleShellDamageCauserHandler shellDamageCauserHandler,float duration,float speed,BulletType bulletType,
            GfFloat3 direction) 
            : base(shellDamageCauserHandler)
        {
            _duration = duration;
            _speed = speed;
            _bulletType = bulletType;
            _direction = direction;
            
            shellDamageCauserHandler.SetMoveDirection(_direction);
        }

        protected override void OnAwakeInternal()
        {
            Entity.Transform.Rotation = GfQuaternion.LookRotation(_direction);
            Entity.On<DeleteShellRequest>(OnDeleteShellRequest);
        }

        protected override void OnStartInternal()
        {
            RegisterAttack(true);
        }

        protected override void OnUpdateInternal(float deltaTime)
        {
            _elapsedTime += deltaTime;
            
            Entity.Transform.Position += _direction * deltaTime * _speed;

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
            //TODO:接入ShellDefinition
            //mCurrentAttackId = AttackId.CreateForShell(mShellDefinition.Id, 1);
            Entity.Request(new BattleRegisterColliderAttackRequest(isOn, AttackId.CreateForShell((uint)ShellDamageCauserHandler.AttackDefinition.AttackId)));
        }

        protected override void DidCauseDamage(in BattleDidCauseDamageRequest request)
        {
            base.DidCauseDamage(in request);
            
            if (_bulletType == BulletType.Normal)
            {
                //击中目标后delete
                _elapsedTime = 0;
                RegisterAttack(false);
                Delete();
            }
        }

        private void OnDeleteShellRequest(in DeleteShellRequest request)
        {
            _elapsedTime = 0;
            RegisterAttack(false);
            Delete();
        }
    }
}