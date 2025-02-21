using Akari.GfUnity;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameMain.Runtime
{
    public class UIFactory
    {
        public async UniTask<GameObject> CreatUIPanel(string panelName)
        {
            var path = AssetPathHelper.GetUIPanelPath(panelName);
            return await AssetManager.Instance.Instantiate(path);
        }
        
        public async UniTask<TComponent> GetUIBattleItem<TComponent>(string itemName,bool isPool = false)  where TComponent : Component
        {
            var path = AssetPathHelper.GetUIBattleItemPath(itemName);
            return await GetUIItem<TComponent>(path, isPool);
        }
        
        public async UniTask<TComponent> GetUITipsItem<TComponent>(string itemName,bool isPool = false)  where TComponent : Component
        {
            var path = AssetPathHelper.GetUITipsItemPath(itemName);
            return await GetUIItem<TComponent>(path, isPool);
        }

        private async UniTask<TComponent> GetUIItem<TComponent>(string path,bool isPool) where TComponent : Component
        {
            var result = await CreateUIItem<TComponent>(path, isPool);
            
            if (result.transform.parent != UIManager.Instance.BattleGroupTransform)
            {
                Transform transform;
                (transform = result.transform).SetParent(UIManager.Instance.BattleGroupTransform);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                transform.localScale = Vector3.one;
            }

            return result;
        }

        private async UniTask<TComponent> CreateUIItem<TComponent>(string path, bool isPool)
            where TComponent : Component
        {
            TComponent result;
            if (isPool)
            {
                var go = await AssetManager.Instance.LoadAsset<GameObject>(path);
                var component = go.GetComponent<TComponent>();
                result = GfPrefabPool.IssueOrCreatPool(component);
            }
            else
            {
                result = await AssetManager.Instance.Instantiate<TComponent>(path);
            }

            return result;
        }
    }
}