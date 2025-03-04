using Akari.GfCore;

namespace GameMain.Runtime
{
    public class BtCharacterAdjustmentAction : ABtCharacterAction
    {
        private readonly float _duration;
        private float _elapsedTime;
        
        public float distance = 5f; // 保持距离
        
        public BtCharacterAdjustmentAction(float duration) : base("BtCharacterAdjustmentAction")
        {
            _duration = duration;
        }

        protected override void DoStart()
        {
            Accessor.Condition.IsMoving = true;
            OnUpdateTimer();
            
            Clock.AddTimer(0.1f, 0f, -1, OnUpdateTimer);
        }

        protected override void DoStop()
        {
            StopAndCleanUp(false);
        }
        
        private void OnUpdateTimer()
        {
            var directionToTarget = Director.Target.Entity.Transform.Position - Accessor.Entity.Transform.Position;
            // 计算当前位置与目标的距离
            float distanceToTarget = directionToTarget.Magnitude;
            if (distanceToTarget > distance)
            {
                Accessor.Condition.MoveDirection = directionToTarget.ToXZFloat2().Normalized;
            }
            else
            {
                Accessor.Condition.MoveDirection = directionToTarget.ToXZFloat2().Normalized * -1;
            }
            
            
            _elapsedTime += 0.1f;
            if (_elapsedTime > _duration)
            {
                StopAndCleanUp(true);
            }
        }

        private void StopAndCleanUp(bool result)
        {
            Accessor.Condition.IsMoving = false;
            Accessor.Condition.MoveDirection = GfFloat2.Zero;
            
            Clock.RemoveTimer(OnUpdateTimer);
            Stopped(result);
        }
    }
}