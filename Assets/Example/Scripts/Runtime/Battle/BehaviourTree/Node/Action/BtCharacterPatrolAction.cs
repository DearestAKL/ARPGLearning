using Akari.GfCore;
using NPBehave;

namespace GameMain.Runtime
{
    /// <summary>
    /// 巡逻 在巡逻点之间移动或者待机
    /// </summary>
    public class BtCharacterPatrolAction : ABtCharacterAction
    {
        private GfFloat3 _curTargetPoint;//当前目标点
        
        private float _idleTime = 3F;//到达目标点后的闲置时间
        private float _elapsedIdleTime;//经过的闲置时间

        private float _maxMoveTime = 10f;//最大移动时间,移动时间超过这个时间，再次进入闲置状态
        private float _elapsedMoveTime;

        //=====Clock Timer=====
        private const float TimerDelay = 0.1F;

        public BtCharacterPatrolAction() : base("BtCharacterPatrolAction")
        {
        }

        public override void SetRoot(Root rootNode)
        {
            base.SetRoot(rootNode);
            
            Entity.On<BattleReceivedDamageRequest>(OnBattleReceivedDamageRequest);
        }
        
        protected override void DoStart()
        {
            Clock.AddTimer(TimerDelay, 0, -1, OnUpdateTimer);
            
            GfLog.Debug("巡逻开始");
        }
        
        protected override void DoStop()
        {
            Accessor.Condition.IsMoving  = false;
            Accessor.Condition.IsWalk = false;
            
            Clock.RemoveTimer(OnUpdateTimer);
            Stopped(false);
            
            GfLog.Debug("巡逻结束");
        }
        
        private void OnUpdateTimer()
        {
            if (Accessor.Condition.IsMoving)
            {
                _elapsedMoveTime += TimerDelay;
                
                //正在移动中
                if (GfFloat3.DistanceXZ(_curTargetPoint, Accessor.Entity.Transform.Position) <= 0.1F ||
                    _elapsedMoveTime >= _maxMoveTime)
                {
                    _elapsedMoveTime = 0;
                    
                    //到达目标点 切换为闲置状态
                    var actionData = BattleCharacterIdleActionData.Create();
                    SendRequest(actionData);
                    
                    Accessor.Condition.IsMoving  = false;
                    Accessor.Condition.IsWalk = false;
                }
            }
            else
            {
                //闲置状态下
                _elapsedIdleTime += TimerDelay;
                if (_elapsedIdleTime >= _idleTime)
                {
                    _elapsedIdleTime = 0;
                    
                    //闲置结束 开始移动到下一个目标点
                    _curTargetPoint = GetNextTargetPoint();
                    
                    if (NavMesh != null) 
                    {
                        NavMesh.SetTargetPos(_curTargetPoint);
                    }

                    Accessor.Condition.IsMoving = true;
                    Accessor.Condition.IsWalk = true;
                }
            }

            SeekTarget();
        }

        private GfFloat3 GetNextTargetPoint()
        {
            if (NavMesh == null)
            {
                return Entity.Transform.Position;
            }
            return NavMesh.GetRandomPoint(10f);
        }

        private void SeekTarget(float radius = 5F,float angle = 120F)
        {
            //检测视野范围内目标
            if (Director.Target != null || BattleAdmin.Player == null)
            {
                return;
            }

            //TODO:不再固定为player
            var toTarget = BattleAdmin.Player.Entity.Transform.Position - Accessor.Entity.Transform.Position;
            if (toTarget.Magnitude <= radius)
            {
                float targetAngle = GfFloat3.Angle(Accessor.Entity.Transform.Forward,  toTarget.Normalized);
                if (targetAngle <= angle / 2)
                {
                    Director.SetTarget(BattleAdmin.Player);
                }
            }
        }
        
        /// <summary>
        /// 巡逻状态下 被攻击且没有目标对象则会选定攻击拥有者为对象
        /// </summary>
        /// <param name="request"></param>
        private void OnBattleReceivedDamageRequest(in BattleReceivedDamageRequest request)
        {
            if (!IsActive || Director.Target != null)
            {
                return;
            }
            
            if (request.DamageResult.AttackCategoryType == AttackCategoryType.Damage)
            {
                var owner = request.DamageResult.AttackParameter.OwnerHandle;
                var ownerEntity = BattleAdmin.EntityComponentSystem.EntityManager.Get(owner);
                if (ownerEntity != null)
                {
                    var accessor = ownerEntity.GetComponent<BattleCharacterAccessorComponent>();
                    if (accessor != null)
                    {
                        //TODO:要进行类型判断
                        Director.SetTarget(accessor);
                    }
                }
            }
        }
    }
}