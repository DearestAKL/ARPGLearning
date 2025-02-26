using Akari.GfCore;

namespace GameMain.Runtime
{
    /// <summary>
    /// 巡逻 在巡逻点之间移动或者待机
    /// </summary>
    public class BtCharacterPatrolAction : ABtCharacterAction
    {
        private GfFloat3 _targetPos;//目标点
        private float _intervalTime = 3f;//移动到目标点后，执行下一段移动的间隔时间
        private float _idleTime;
        
#if UNITY_EDITOR
        private GizmosData _gizmosData;
#endif
        
        public BtCharacterPatrolAction() : base("BtCharacterPatrolAction")
        {
        }

        protected override void DoStart()
        {
            OnUpdateTimer();
            Clock.AddTimer(0.1f, 0, -1, OnUpdateTimer);
        }
        
        protected override void DoStop()
        {
            Clock.RemoveTimer(OnUpdateTimer);
            Stopped(false);
            
#if UNITY_EDITOR
            GizmosManager.Instance.RemoveGizmosData(_gizmosData);
#endif
        }
        
        private void OnUpdateTimer()
        {
            if (Accessor.Condition.IsMoving)
            {
                var directionToTarget  = _targetPos - Accessor.Entity.Transform.Position;
                Accessor.Condition.MoveDirection = directionToTarget.ToXZFloat2().Normalized;
                
                if (directionToTarget.Magnitude <= 1F)
                {
                    //到达目标点，进入idle
                    var actionData = BattleCharacterIdleActionData.Create();
                    SendRequest(actionData);
                    
                    _idleTime = 0;
                    Accessor.Condition.IsMoving  = false;
                    
#if UNITY_EDITOR
                    GizmosManager.Instance.RemoveGizmosData(_gizmosData);
#endif
                }
            }
            else
            {
                _idleTime += 0.1f;
                if (_idleTime >= _intervalTime)
                {
                    //等待结束 开始移动到下一个目标点
                    UpdateTargetPos();
                    
                    var actionData = BattleCharacterMoveRunActionData.Create();
                    SendRequest(actionData);
                    Accessor.Condition.IsMoving = true;
                    
#if UNITY_EDITOR
                    _gizmosData = GizmosData.CreateLineGizmosData(Accessor.Entity.Transform, _targetPos);
                    GizmosManager.Instance.AddGizmosData(_gizmosData);
#endif
                }
            }
        }

        private void UpdateTargetPos()
        {
            float x = BattleAdmin.RandomGenerator.Range(-20f, 20f);
            float y = BattleAdmin.RandomGenerator.Range(-20f, 20f);
            
            _targetPos = new GfFloat3(x, 0, y);
        }
    }
}