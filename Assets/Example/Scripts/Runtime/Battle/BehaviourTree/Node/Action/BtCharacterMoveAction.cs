using Akari.GfCore;

namespace GameMain.Runtime
{
    public class BtCharacterMoveAction : ABtCharacterAction
    {
        
#if UNITY_EDITOR
        private GizmosData _gizmosData;
#endif
        
        public BtCharacterMoveAction() : base("BtCharacterMoveAction")
        {
        }

        protected override void DoStart()
        {
            Accessor.Condition.IsMoving = true;
            OnUpdateTimer();
            
            Clock.AddTimer(0.1f, 0.02f, -1, OnUpdateTimer);
            
#if UNITY_EDITOR
            _gizmosData = GizmosData.CreateLineGizmosData(Accessor.Entity.Transform, Director.Target.Entity.Transform);
            GizmosManager.Instance.AddGizmosData(_gizmosData);
#endif
        }

        protected override void DoStop()
        {
            StopAndCleanUp(false);
            
#if UNITY_EDITOR
            GizmosManager.Instance.RemoveGizmosData(_gizmosData);
#endif
        }

        private void OnUpdateTimer()
        {
            if (Director.Target != null) 
            {
                var directionToTarget  = Director.Target.Entity.Transform.Position - Accessor.Entity.Transform.Position;
                Accessor.Condition.MoveDirection = directionToTarget .ToXZFloat2().Normalized;
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