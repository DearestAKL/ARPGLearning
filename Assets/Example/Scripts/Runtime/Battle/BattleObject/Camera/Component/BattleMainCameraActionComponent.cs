using Akari.GfCore;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public sealed class BattleMainCameraActionContext : IGfActionContext
    {
        public GfEntity Entity { get; set; }

        public BattleMainCameraActionContext(GfEntity entity)
        {
            Entity = entity;
        }
    }

    public readonly struct CameraShakeRequest : IGfRequest
    {
        public          GfRunTimeTypeId                       RttId => GfRunTimeTypeOf<CameraShakeRequest>.Id;
        public readonly BattleCameraShakeParam.ShakePower     Power;
        public readonly BattleCameraShakeParam.ShakeDirection Direction;

        public CameraShakeRequest(BattleCameraShakeParam.ShakePower power, BattleCameraShakeParam.ShakeDirection direction)
        {
            Power     = power;
            Direction = direction;
        }
    }
    
    public sealed class BattleMainCameraActionComponent : GfActionComponent<BattleMainCameraActionComponent, BattleMainCameraActionContext>
    {
        private GfComponentCache<BattleMainCameraAccessorComponent> _accessorCache;
        private BattleMainCameraAccessorComponent Accessor => Entity.GetComponent(ref _accessorCache);
        
        public BattleMainCameraActionComponent(BattleMainCameraActionContext context, bool enablePrioritySort = true) : base(context, enablePrioritySort)
        {

        }
        
        public override void OnAwake()
        {
            Entity.On<CameraShakeRequest>(OnCameraShakeRequest);
            base.OnAwake();
        }
        
        private void OnCameraShakeRequest(in CameraShakeRequest request)
        {
            Accessor.View.ShakeCamera(request.Power, request.Direction);
        }
    }
}
