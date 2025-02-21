using UnityEngine;

namespace GameMain.Runtime
{
    /// <summary>
    /// 用于生成回收Roguelike Door
    /// </summary>
    public class RoguelikeDoorPoint : MonoBehaviour
    {
        private RoguelikeDoor _roguelikeDoor;
        
        public async void Create(RoguelikeRoomData roguelikeRoomData)
        {
            _roguelikeDoor = await AssetManager.Instance.InstantiateFormPool<RoguelikeDoor>(AssetPathHelper.GetInteractiveObjectPath("Door"));
            _roguelikeDoor.SetData(roguelikeRoomData);
            _roguelikeDoor.transform.position = transform.position;
            _roguelikeDoor.transform.rotation = transform.rotation;
        }

        private void OnDestroy()
        {
            if (_roguelikeDoor != null)
            {
                _roguelikeDoor.ReturnToPool();
            }
        }
        
        private void OnDrawGizmos()
        {
            transform.DrawBox(3, 2, Color.cyan);
        }
    }
}