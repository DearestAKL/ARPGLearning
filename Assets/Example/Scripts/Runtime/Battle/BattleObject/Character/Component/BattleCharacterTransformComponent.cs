using Akari.GfCore;
using Akari.GfGame;
using Akari.GfUnity;

namespace GameMain.Runtime
{
    public interface IBattleCharacterTransformComponent 
    {
        GfTransform Transform { get; }
    }

    public sealed class BattleCharacterTransformComponent : AGfGameComponent<BattleCharacterTransformComponent>, IBattleCharacterTransformComponent
    {
        private GfComponentCache<BattleCharacterViewComponent> _viewCache;

        private BattleCharacterViewComponent View => Entity.GetComponent(ref _viewCache);
        
        private float _curYSpeed;
        public GfTransform Transform { get; }

        public GfFloat3 CurrentPosition => Transform.Position;
        public GfQuaternion CurrentRotation => Transform.Rotation;

        private GfFloat3 _worldPositionCache;

        public bool IsGrounded
        {
            get
            {
                return CurrentPosition.Y <= 0.0001f;
            }
        }
        

        public BattleCharacterTransformComponent(GfTransform transform) 
        {
            Transform = transform;
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
            Transform.UpdateTransform();
        }

        public void MovePosition(GfFloat3 diffPosition)
        {
            View.UnityView?.CharacterController.Move(diffPosition.ToVector3());
        }

        public void SetTransform(GfFloat3 position ,GfQuaternion rotation)
        {
            Transform.Position = position;
            Transform.Rotation = rotation;
            UnityEngine.Physics.SyncTransforms();
        }

        public void RecordWorldPosition()
        {
            _worldPositionCache = CurrentPosition;
        }
        
        public void ResetToRecordWorldPosition()
        {
            SetTransform(_worldPositionCache,GfQuaternion.Euler(0, 180, 0));
        }
    }
}
