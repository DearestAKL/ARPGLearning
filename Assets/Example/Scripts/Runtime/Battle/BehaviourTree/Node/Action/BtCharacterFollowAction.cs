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

        private GfFloat3 _curTargetPos;

        public BtCharacterFollowAction(float minDistance,float maxDistance) : base("BtCharacterFollowAction")
        {
            _minDistance = minDistance;
            _maxDistance = maxDistance;
        }

        protected override void DoStart()
        {
            Clock.AddTimer(0.1f, 0f, -1, OnUpdateTimer);
        }

        protected override void DoStop()
        {
            Clock.RemoveTimer(OnUpdateTimer);
            
            Accessor.Condition.IsMoving = false;
            Accessor.Condition.MoveDirection = GfFloat2.Zero;
            
            Stopped(false);
        }

        private void OnUpdateTimer()
        {
            if (Director.Target == null)
            {
                return;
            }

            GfFloat3 newTargetPos = GfFloat3.Zero;
            var curDistance = GfFloat3.DistanceXZ(Director.Target.Entity.Transform.Position, Accessor.Entity.Transform.Position);
            if (curDistance > _maxDistance)
            {
                Accessor.Condition.IsMoving = true;
                //靠近
                newTargetPos = Director.Target.Entity.Transform.Position;
            }
            else if(curDistance < _minDistance)
            {
                Accessor.Condition.IsMoving = true;
                //远离
                var toTarget = Director.Target.Entity.Transform.Position - Accessor.Entity.Transform.Position;
                newTargetPos = Accessor.Entity.Transform.Position - toTarget.ToXZFloat2().Normalized * (_maxDistance - curDistance);
            }
            else
            {
                Accessor.Condition.IsMoving = false;
            }

            if (Accessor.Condition.IsMoving)
            {
                if (GfFloat3.DistanceXZ(_curTargetPos, newTargetPos) >_maxDistance - _minDistance)
                {
                    //目标点变化幅度大于0.1 则重新发送请求更新目标点
                    _curTargetPos = newTargetPos;
                    
                    if (NavMesh != null) 
                    {
                        NavMesh.SetTargetPos(_curTargetPos);
                    }
                }
            }
        }
    }
}