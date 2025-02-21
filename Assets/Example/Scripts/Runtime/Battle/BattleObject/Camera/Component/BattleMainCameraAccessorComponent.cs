using Akari.GfCore;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public interface IBattleCameraAccessorComponent
    {
        BattleMainCameraUnityView View { get; }

        GfEntity Entity { get; }

        BattleMainCameraActionComponent ActionComponent { get; }
    }
    public sealed class BattleMainCameraAccessorComponent : AGfGameComponent<BattleMainCameraAccessorComponent>, IBattleCameraAccessorComponent
    {
        public BattleMainCameraUnityView View { get; }

        public BattleMainCameraActionComponent ActionComponent { get; }

        public BattleMainCameraAccessorComponent(BattleMainCameraUnityView view, BattleMainCameraActionComponent cameraActionComponent)
        {
            View = view;
            ActionComponent = cameraActionComponent;
        }
    }
}
