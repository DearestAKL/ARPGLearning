using Akari.GfCore;

namespace GameMain.Runtime
{
    /// <summary>
    /// 对峙
    /// 与目标保持相对距离
    /// </summary>
    public class BtCharacterConfrontAction : ABtCharacterAction
    {
        private GfFloat3 _curTargetPoint;//当前目标点
        
        private float _distance = 3f;//对峙距离
        
        private float _confrontTime = 5f;
        private float _elapsedTime;
        
        //=====Clock Timer=====
        private const float TimerDelay = 0.1F;

        private readonly float cos;
        private readonly float sin;
        
        public BtCharacterConfrontAction() : base("BtCharacterConfrontAction")
        {
            var angleDegrees = 30F;
            float angleRadians = angleDegrees * GfMathf.Deg2Rad;
            cos = GfMathf.Cos(angleRadians);
            sin = GfMathf.Sin(angleRadians);
        }
        
        protected override void DoStart()
        {
            Accessor.Condition.IsMoving = true;
            Accessor.Condition.IsWalk = true;
            Accessor.Condition.HasLockTarget = true;
            
            _elapsedTime = 0;
            
            Clock.AddTimer(TimerDelay, 0, -1, OnUpdateTimer);
            
            GfLog.Debug("对峙开始");
        }
        
        protected override void DoStop()
        {
            Accessor.Condition.IsMoving  = false;
            Accessor.Condition.IsWalk = false;
            Accessor.Condition.HasLockTarget = false;
            
            Clock.RemoveTimer(OnUpdateTimer);
            Stopped(true);
            
            GfLog.Debug("对峙结束");
        }

        private void OnUpdateTimer()
        {
            _elapsedTime += TimerDelay;
            if (_elapsedTime > _confrontTime || Director.Target == null)
            {
                _elapsedTime = 0f;
                //没有目标
                Stop();
                return;
            }

            Accessor.Condition.CharacterDirection =
                (Director.Target.Entity.Transform.Position - Entity.Transform.Position).ToXZFloat2();
            
            _curTargetPoint = GetNextTargetPoint();
            if (NavMesh != null) 
            {
                NavMesh.SetTargetPos(_curTargetPoint);
            }
        }
        
        private GfFloat3 GetNextTargetPoint()
        {
            if (Director.Target == null)
            {
                return GfFloat3.Zero;
            }
            
            var targetPosition = Director.Target.Entity.Transform.Position;
            var minePosition = Entity.Transform.Position;

            GfFloat3 direction = minePosition - targetPosition;

            float newX = direction.X * cos - direction.Z * sin;
            float newZ = direction.X * sin + direction.Z * cos;

            var tarPos = new GfFloat3(newX, 0, newZ).Normalized * _distance + targetPosition;

            return new GfFloat3(tarPos.X, minePosition.Y, tarPos.Z);
        }
    }
}