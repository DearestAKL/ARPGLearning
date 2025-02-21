using Akari.GfCore;
using UnityEngine.InputSystem;

namespace GameMain.Runtime
{
    public sealed class BattleUnityAdmin : GfSingleton<BattleUnityAdmin>
    {
        private BattleMainCameraUnityView _battleMainCameraView = default;
        private BattleInputComponentSystem _battleInput = default;//战斗包含场景输入，当ui处于HasPopUp时将失效
        private PlayerInput _playerInput = default;//全局输入,UI可以使用这个
        private HouseRoom _curHouseRoom = default;
        
        public static BattleMainCameraUnityView     BattleMainCameraView        => Instance._battleMainCameraView;
        public static BattleInputComponentSystem      BattleInput        => Instance._battleInput;
        public static PlayerInput      PlayerInput        => Instance._playerInput;

        public static HouseRoom CurHouseRoom => Instance._curHouseRoom;
        
        public void SetBattleMainCameraUnityView(BattleMainCameraUnityView battleMainCameraUnityView)
        {
            _battleMainCameraView = battleMainCameraUnityView;
        }

        public void SetInput(PlayerInput playerInput, BattleInputComponentSystem battleInput)
        {
            _playerInput = playerInput;
            _battleInput = battleInput;
        }
        
        public void SetHouseRoom(HouseRoom houseRoom)
        {
            _curHouseRoom = houseRoom;
        }
    }
}