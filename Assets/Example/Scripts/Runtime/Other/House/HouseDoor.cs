using Akari.GfCore;
using Akari.GfUnity;
using UnityEngine;

namespace GameMain.Runtime
{
    /// <summary>
    /// 房子门
    /// </summary>
    public class HouseDoor : AClickInteractiveObject
    {
        public int curRoomId;
        public int nextRoomId;
        public Transform point;

        public GfFloat3 PointPosition => (point ? point : transform).position.ToGfFloat3();
        public GfQuaternion PointRotation => (point ? point : transform).rotation.ToGfQuaternion();
        
        public override string InteractionTips => nextRoomId == 0 ? "离开" : "进入";
        
        protected override async void OnInteracting(CharacterInteractive characterInteractive)
        {
            characterInteractive.RemoveInteractiveObject(this);
            GfLog.Debug($"进入 Room:{nextRoomId}");
            await UIHelper.StartLoading();
            HouseRoom nextRoom = null;
            if (curRoomId == 0)
            {
                //在室外
                
                //室外进入室内
                //隐藏室外场景,实例化室内场景 设置pos
                BattleAdmin.Player.Transform.RecordWorldPosition();
                
                EventManager.Instance.BattleEvent.OnEnterRoomEvent.Invoke();
                nextRoom = await AssetManager.Instance.Instantiate<HouseRoom>($"Assets/Example/GameRes/Prefabs/HouseRoom/HouseRoom_{nextRoomId}");
                nextRoom.Enter(curRoomId);
            }
            else
            {
                //在室内
                if (nextRoomId == 0)
                {
                    //室外进入室内
                    //隐藏室外场景,实例化室内场景 设置pos
                    EventManager.Instance.BattleEvent.OnExitRoomEvent.Invoke();
                }
                else
                {
                    //室内进入室内
                    //删除当前室内场景，实例化next室内场景 设置pos 
                    nextRoom = await AssetManager.Instance.Instantiate<HouseRoom>($"Assets/Example/GameRes/Prefabs/HouseRoom/HouseRoom_{nextRoomId}");
                    nextRoom.Enter(curRoomId);
                }
                
            }
            ResetStateData();
            
            UIHelper.EndLoading();
            
            if (BattleUnityAdmin.CurHouseRoom != null)
            {
                BattleUnityAdmin.CurHouseRoom.Clear();
            }
            BattleUnityAdmin.Instance.SetHouseRoom(nextRoom);
        }
    }
} 