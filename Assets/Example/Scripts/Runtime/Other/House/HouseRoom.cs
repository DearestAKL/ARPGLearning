using System.Collections.Generic;
using UnityEngine;

namespace GameMain.Runtime
{
    /// <summary>
    /// 房子房间
    /// </summary>
    public class HouseRoom : MonoBehaviour
    {
        public int roomId = 1001;
        public List<HouseDoor> doors;

        private void Awake()
        {
            foreach (var door in doors)
            {
                door.curRoomId = roomId;
            }
        }

        public void Enter(int lastRoomId)
        {
            foreach (var door in doors)
            {
                //找到入口门
                if (door.nextRoomId == lastRoomId)
                {
                    BattleAdmin.Player.Transform.SetTransform(door.PointPosition,
                        door.PointRotation);
                }
            }
        }

        public void Clear()
        {
            Destroy(gameObject);
        }
    }
}