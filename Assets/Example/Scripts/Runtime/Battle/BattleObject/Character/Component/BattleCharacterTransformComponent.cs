using Akari.GfCore;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public sealed class BattleCharacterTransformComponent : AGfGameComponent<BattleCharacterTransformComponent>
    {
        private GfComponentCache<BattleCharacterViewComponent> _viewCache;

        private BattleCharacterViewComponent View => Entity.GetComponent(ref _viewCache);
        
        private float _verticalVelocity;
        private GfFloat3 _worldPositionCache;

        public bool IsGrounded
        {
            get
            {
                return View.UnityView.CharacterController.IsStableOnGround;
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
        
        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
        }

        public override void OnEndUpdate(float deltaTime)
        {
            base.OnEndUpdate(deltaTime);
        }

        public void MovePosition(GfFloat3 diffPosition)
        {
            Entity.Transform.Position += diffPosition;
        }

        public void SetTransform(GfFloat3 position ,GfQuaternion rotation)
        {
            Entity.Transform.Position = position;
            Entity.Transform.Rotation = rotation;
            Entity.Request(new SetPositionAndRotationRequest(position, rotation));
            
            //动画也需要切换为idle
            // var actionData = BattleCharacterIdleActionData.Create();
            // Entity.Request(new GfChangeActionRequest<BattleObjectActionComponent>(actionData.ActionType, actionData, actionData.Priority));
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
    
    public readonly struct SetPositionAndRotationRequest : IGfRequest
    {
        public GfRunTimeTypeId RttId => GfRunTimeTypeOf<SetPositionAndRotationRequest>.Id;
        public readonly GfFloat3 Position;
        public readonly GfQuaternion Rotation;
        public SetPositionAndRotationRequest(GfFloat3 position,GfQuaternion rotation)
        {
            Position     = position;
            Rotation     = rotation;
        }
    }
}
