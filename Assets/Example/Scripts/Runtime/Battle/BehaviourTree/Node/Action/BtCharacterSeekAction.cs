using Akari.GfCore;
using NPBehave;

namespace GameMain.Runtime
{
    /// <summary>
    /// 寻敌Action,和Idle 巡逻 并行
    /// 敌人在视野范围内，自己或者周围的友方单位被攻击，都会锁定敌人
    /// </summary>
    public class BtCharacterSeekAction : ABtCharacterAction
    {
        //最好是扇形,不使用unity如何去检测扇形范围内的目标呢 
        //不需要一直检测 1s检测一次就行
        private float _radius;
        private float _angle;
        
#if UNITY_EDITOR
        private GizmosData _gizmosData;
#endif
        public BtCharacterSeekAction(float radius,int angle) : base("BtCharacterSeekAction")
        {
            _radius = radius;
            _angle = angle;
        }

        public override void SetRoot(Root rootNode)
        {
            base.SetRoot(rootNode);
            
            Entity.On<BattleReceivedDamageRequest>(OnBattleReceivedDamageRequest);
        }

        protected override void DoStart()
        {
            OnUpdateTimer();
            Clock.AddTimer(1f, 0f, -1, OnUpdateTimer);
#if UNITY_EDITOR
            _gizmosData = GizmosData.CreateCircleGizmosData(Accessor.Transform.Transform, _radius, _angle);
            GizmosManager.Instance.AddGizmosData(_gizmosData);
#endif
        }
        
        protected override void DoStop()
        {
            Clock.RemoveTimer(OnUpdateTimer);
#if UNITY_EDITOR
            GizmosManager.Instance.RemoveGizmosData(_gizmosData);
#endif
            
            Stopped(false);
        }
        
        private void OnUpdateTimer()
        {
            if (BattleAdmin.Player == null)
            {
                return;
            }
            
            //检测范围内是否有敌对目标
            var toTarget = BattleAdmin.Player.Transform.CurrentPosition - Accessor.Transform.Transform.Position;
            if (toTarget.Magnitude <= _radius)
            {
                float targetAngle = GfFloat3.Angle(Accessor.Transform.Transform.Forward,  toTarget.Normalized);
                if (targetAngle <= _angle / 2)
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
                        Director.SetTarget(accessor);
                    }
                }
            }
        }
    }
}