using Akari.GfUnity;
using cfg;
using UnityEngine;

namespace GameMain.Runtime
{
    /// <summary>
    /// 商店物品
    /// 购买完成，改变表现，关闭交互
    /// </summary>
    public class ShopItem : AClickInteractiveObject
    {
        [SerializeField] private Transform showPosTrans = default;
        
        private ShopItemData _shopItemData;
        
        private UINameTips _tips;
        
        private DropItem _dropItem;

        public override string InteractionTips => "Buy";
        protected override void OnInteracting(CharacterInteractive characterInteractive)
        {
            if (!UserDataManager.Instance.Battle.IsEnoughCoin(_shopItemData.Cost))
            {
                //todo: show tips
                ResetStateData();
                return;
            }
            
            //消耗Coin
            UserDataManager.Instance.Battle.SubtractCoin(_shopItemData.Cost);
            //关闭交互
            SetCanInteract(false);

            _dropItem.gameObject.SetActive(false);
            
            characterInteractive.RemoveInteractiveObject(this);
        }
        
        private void OnDestroy()
        {
            if (_tips != null)
            {
                Destroy(_tips.gameObject); 
            }
        }
        
        public async void SetData(ShopItemData shopItemData)
        {
            _shopItemData = shopItemData;
            
            if (_tips == null)
            {
                _tips = await UIManager.Instance.Factory.GetUITipsItem<UINameTips>("UINameTips");
            }
            
            _tips.transform.localPosition = UIHelper.WorldPositionToBattleUI(transform.position, new Vector2(0, 100f));
            _tips.UpdateView($"Cost:{_shopItemData.Cost}");  
            _tips.gameObject.SetActive(true);

            if (_shopItemData.RewardType == RewardType.Coin)
            {
                _dropItem = await AssetManager.Instance.InstantiateFormPool<DropItem>(
                    AssetPathHelper.GetInteractiveObjectPath("Heart"));
            }
            // else if (_shopItemData.RewardType == RewardType.Gem)
            // {
            //     _dropItem = await AssetManager.Instance.InstantiateFormPool<DropItem>(
            //         AssetPathHelper.GetInteractiveObjectPath("Boost"));
            // }
            else
            {
                _dropItem = await AssetManager.Instance.InstantiateFormPool<DropItem>(
                    AssetPathHelper.GetInteractiveObjectPath("Ammo"));
            }

            _dropItem.transform.position = showPosTrans.position;
            _dropItem.SetData(_shopItemData.RewardType, _shopItemData.ItemNum, _shopItemData.ItemId);
            _dropItem.SetCanInteract(false);
        }
        
        public void ReturnToPool()
         {
             //状态还原
             ResetStateData();
             
             if (_dropItem != null)
             {
                 _dropItem.ReturnToPool();
                 _dropItem = null;
             }
             _tips.gameObject.SetActive(false);
             GfPrefabPool.Return(this);
         }
    }
}