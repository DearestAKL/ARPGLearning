// ------------------------------
// ※ 摘要
// 资源管理器
// 将第三方资源管理再封装一层,为上层资源加载业务提供接口。
// ------------------------------

using System.Collections.Generic;
using UnityEngine;
using Akari.GfCore;
using Akari.GfUnity;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using YooAsset;

namespace GameMain.Runtime
{
    public class AssetManager : GfSingleton<AssetManager>
    {
	    private ResourcePackage _gamePackage;
	    private readonly List<string> _preloadLocations = new List<string>();
	    
        protected override void OnCreated()
        {
        }
        
        protected override void OnDisposed()
        {
        }

        public void SetGamePackage(ResourcePackage resourcePackage)
        {
	        _gamePackage = resourcePackage;
	        
	        //_gamePackage.UnloadUnusedAssets();
        }

        /// <summary>
        /// 同步加载资源对象
        /// </summary>
        /// <typeparam name="TObject">资源类型</typeparam>
        /// <param name="location">资源的定位地址</param>
        public async UniTask<TObject> LoadAsset<TObject>(string location) where TObject : UnityEngine.Object
        {
	        var handle = YooAssets.LoadAssetAsync<TObject>(location);
	        await handle.ToUniTask();
	        if (handle.AssetObject == null)
	        {
		        GfLog.Error($"LoadAsset:{location},assetObject is null");
		        return null;
	        }
	        return handle.AssetObject as TObject;
        }

        public async UniTask PreloadAsset<TObject>(string location) where TObject : UnityEngine.Object
        {
	        if (_preloadLocations.Contains(location))
	        {
		        //不需要特别准确，为YooAssets底层也有基于location的判断
		        return;
	        }
	        else
	        {
		        _preloadLocations.Add(location);
	        }
	        
	        var handle = YooAssets.LoadAssetAsync<TObject>(location);
	        await handle.ToUniTask();
        }

        /// <summary>
        /// 同步加加载 + 同步实例化对象
        /// </summary>
        /// <param name="location">资源的定位地址</param>
        /// <param name="parent">父对象</param>
        /// <returns></returns>
        public async UniTask<GameObject> Instantiate(string location, Transform parent = null)
        {
	        var gameObject = await LoadAsset<GameObject>(location);
	        return Object.Instantiate(gameObject, parent);
        }
        
        public async UniTask<TComponent> Instantiate<TComponent>(string location, Transform parent = null) where TComponent : Component
        {
	        var gameObject = await Instantiate(location,parent);
	        return gameObject.GetComponent<TComponent>();
        }

        public async UniTask<TComponent> InstantiateFormPool<TComponent>(string location) where TComponent : Component
        {
	        var go = await LoadAsset<GameObject>(location);
	        var component = go.GetComponent<TComponent>();
	        return GfPrefabPool.IssueOrCreatPool(component);
        }
        
        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="location">场景的定位地址</param>
        /// <param name="sceneMode">场景加载模式</param>
        /// <param name="suspendLoad">场景加载到90%自动挂起</param>
        /// <param name="priority">优先级</param>
        public SceneHandle LoadSceneAsync(string location, LoadSceneMode sceneMode = LoadSceneMode.Single, LocalPhysicsMode physicsMode = LocalPhysicsMode.None,bool suspendLoad = false, uint priority = 100)
        {
	        return YooAssets.LoadSceneAsync(location, sceneMode,physicsMode, suspendLoad, priority);
        }

        /// <summary>
        /// 异步加载资源包内所有资源对象
        /// </summary>
        /// <typeparam name="TObject">资源类型</typeparam>
        /// <param name="location">资源的定位地址</param>
        public async UniTask<TObject[]> LoadAllAssetsAsync<TObject>(string location, uint priority = 0) where TObject : UnityEngine.Object
        {
	        AllAssetsHandle allAssetsHandle = YooAssets.LoadAllAssetsAsync<TObject>(location, priority);
	        await allAssetsHandle.ToUniTask();

	        TObject[] objects = new TObject[allAssetsHandle.AllAssetObjects.Count];
	        for (int i = 0; i < allAssetsHandle.AllAssetObjects.Count; i++)
	        {
		        objects[i] = allAssetsHandle.AllAssetObjects[i] as TObject;
	        }

	        return objects;
        }
    }
}