using UnityEngine;

namespace GameMain.Runtime
{
    /// <summary>
    /// 用于生成回收ShopItem
    /// </summary>
    public class RoguelikeShopItemPoint : MonoBehaviour
    {
        private ShopItem _curShopItem;
        
        public async void Create(ShopItemData shopItemData)
        {
            _curShopItem = await AssetManager.Instance.InstantiateFormPool<ShopItem>(
                AssetPathHelper.GetInteractiveObjectPath("ShopItem"));
            _curShopItem.transform.position = transform.position;
            _curShopItem.SetData(shopItemData);
        }
        
        private void OnDestroy()
        {
            if (_curShopItem != null)
            {
                _curShopItem.ReturnToPool();
            }
        }
        
        private void OnDrawGizmos()
        {
            transform.DrawBox(2, 2, Color.cyan);
        }
    }
}