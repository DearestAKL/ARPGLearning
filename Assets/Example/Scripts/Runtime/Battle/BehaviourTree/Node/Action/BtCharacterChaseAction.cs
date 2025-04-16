using Akari.GfCore;

namespace GameMain.Runtime
{
    /// <summary>
    /// 追踪
    /// </summary>
    public class BtCharacterChaseAction : ABtCharacterAction
    {
        private GfFloat3 _curTargetPoint;//当前目标点
        
        //=====Clock Timer=====
        private const float TimerDelay = 0.1F;
        
        public BtCharacterChaseAction() : base("BtCharacterChaseAction")
        {
            
        }
        
        protected override void DoStart()
        {
            Clock.AddTimer(TimerDelay, 0, -1, OnUpdateTimer);
            
            GfLog.Debug("追踪开始");
        }
        
        protected override void DoStop()
        {
            Clock.RemoveTimer(OnUpdateTimer);
            Stopped(false);
            
            GfLog.Debug("追踪结束");
        }

        private void OnUpdateTimer()
        {
            if (Director.Target == null)
            {
                //没有目标
                return;
            }
            
            //接近目标
            Accessor.Condition.IsMoving = true;
            Accessor.Condition.IsWalk = true;//todo:根据初始距离远近切换walk和run

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
                return Entity.Transform.Position;
            }
            return Director.Target.Entity.Transform.Position;
        }
    }
}