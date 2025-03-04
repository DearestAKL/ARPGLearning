using Akari.GfCore;
using NPBehave;

namespace GameMain.Runtime
{
    /// <summary>
    /// 跟随Action
    /// </summary>
    public class BtCharacterFollowAction : ABtCharacterAction
    {
        private float _minDistance;
        private float _maxDistance;
        
        public BtCharacterFollowAction(float minDistance,float maxDistance) : base("BtCharacterFollowAction")
        {
            _minDistance = minDistance;
            _maxDistance = maxDistance;
        }

        protected override void DoStart()
        {
            OnUpdateTimer();
            Clock.AddTimer(0.2f, 0f, -1, OnUpdateTimer);
        }

        protected override void DoStop()
        {
            StopAndCleanUp(false);
        }

        private void OnUpdateTimer()
        {
            //先默认跟随主角
            if (BattleAdmin.Player == null)
            {
                return;
            }
            Director.SetTarget(BattleAdmin.Player);

            var toTarget = Director.Target.Entity.Transform.Position - Accessor.Entity.Transform.Position;
            if (toTarget.Magnitude > _maxDistance)
            {
                Accessor.Condition.IsMoving = true;
                //靠近
                var directionToTarget  = Director.Target.Entity.Transform.Position - Accessor.Entity.Transform.Position;
                Accessor.Condition.MoveDirection = directionToTarget .ToXZFloat2().Normalized;
            }
            else if(toTarget.Magnitude < _minDistance)
            {
                Accessor.Condition.IsMoving = true;
                //远离
                var directionToTarget  =  Accessor.Entity.Transform.Position - Director.Target.Entity.Transform.Position;
                Accessor.Condition.MoveDirection = directionToTarget .ToXZFloat2().Normalized;
            }
            else
            {
                Accessor.Condition.IsMoving = false;
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