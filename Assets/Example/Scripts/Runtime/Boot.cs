using System.Collections;
using Akari.GfCore;
using Akari.GfUnity;
using UnityEngine;
using YooAsset;

namespace GameMain.Runtime
{
    public class Boot : MonoBehaviour
    {
        /// <summary>
        /// 资源系统运行模式
        /// </summary>
        public EPlayMode PlayMode = EPlayMode.EditorSimulateMode;

        void Awake()
        {
            Debug.Log($"资源系统运行模式：{PlayMode}");
            Application.targetFrameRate = 60;
            Application.runInBackground = true;
            DontDestroyOnLoad(this.gameObject);
        }
        
        IEnumerator Start()
        {
            GfLog.Add(new GfUnityLogWriter());
            
            // 初始化UniTask异步管理器
            AsyncManager.CreateInstance();
            
            // 初始化事件管理器
            EventManager.CreateInstance();
            
            // 初始化YooAssets资源系统
            YooAssets.Initialize();

            // 加载更新页面
            var go = Resources.Load<GameObject>("PatchWindow");
            Instantiate(go);

            // 开始补丁更新流程
            var  configurationModel = new PatchConfigurationModel("DefaultPackage", PlayMode);
            PatchOperation operation = new PatchOperation(configurationModel);
            YooAssets.StartOperation(operation);
            yield return operation;

            // 设置默认的资源包
            var gamePackage = YooAssets.GetPackage("DefaultPackage");
            YooAssets.SetDefaultPackage(gamePackage);

            //初始化依托于YooAssets的资源管理器
            AssetManager.CreateInstance();
            
            ManagerHelper.IsInitYooAssets = true;

            //切换到下个场景
            AssetManager.Instance.LoadSceneAsync("Assets/Example/GameRes/Scene/HomeScene");
        }
    }
}
