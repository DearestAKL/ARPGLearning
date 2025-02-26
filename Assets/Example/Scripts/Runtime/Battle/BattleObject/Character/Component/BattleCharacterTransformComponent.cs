using Akari.GfCore;
using Akari.GfGame;
using Akari.GfUnity;

namespace GameMain.Runtime
{
    public sealed class BattleCharacterTransformComponent : AGfGameComponent<BattleCharacterTransformComponent>
    {
        private GfComponentCache<BattleCharacterViewComponent> _viewCache;

        private BattleCharacterViewComponent View => Entity.GetComponent(ref _viewCache);
        
        private float _curYSpeed;

        private GfFloat3 _worldPositionCache;

        public bool IsGrounded
        {
            get
            {
                return Entity.Transform.Rotation.Y <= 0.0001f;
            }
        }
        

        public BattleCharacterTransformComponent() 
        {

        }

        public override void OnStart()
        {
            
        }

        public override void OnBeginUpdate(float deltaTime)
        {
            base.OnBeginUpdate(deltaTime);
        }

        public override void OnEndUpdate(float deltaTime)
        {
            base.OnEndUpdate(deltaTime);
            Entity.Transform.UpdateTransform();
        }

        public void MovePosition(GfFloat3 diffPosition)
        {
            View.UnityView?.CharacterController.Move(diffPosition.ToVector3());
        }

        public void SetTransform(GfFloat3 position ,GfQuaternion rotation)
        {
            Entity.Transform.Position = position;
            Entity.Transform.Rotation = rotation;
            UnityEngine.Physics.SyncTransforms();
        }

        public void RecordWorldPosition()
        {
            _worldPositionCache = Entity.Transform.Position;
        }
        
        public void ResetToRecordWorldPosition()
        {
            SetTransform(_worldPositionCache,GfQuaternion.Euler(0, 180, 0));
        }
    }
}
