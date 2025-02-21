using UnityEngine;

namespace GameMain.Runtime
{
    public class ChestPoint : MonoBehaviour
    {
        [SerializeField] private int chestId;
        
        private Chest _chest;

        private void Awake()
        {
            Create();
        }

        private async void Create()
        {
            if (chestId == 0)
            {
                return;
            }
            
            if (UserDataManager.Instance.Player.HasGotChest(chestId))
            {
                return;
            }
            
            var chestConfig = LubanManager.Instance.Tables.TbChest.Get(chestId);
            
            _chest = await AssetManager.Instance.InstantiateFormPool<Chest>(AssetPathHelper.GetInteractiveObjectPath("Chest"));
            _chest.SetData(chestConfig);
        }

        private void OnDestroy()
        {
            if (_chest != null)
            {
                _chest.ReturnToPool();
            }
        }
        
        private void OnDrawGizmos()
        {
            transform.DrawBox(1, 1, Color.cyan);
        }
    }
}