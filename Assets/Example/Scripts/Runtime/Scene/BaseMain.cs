using System;
using System.Collections;
using Akari.GfCore;
using Akari.GfUnity;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

namespace GameMain.Runtime
{
    public class BaseMain : MonoBehaviour
    {
        /// <summary>
        /// 编辑器下 不需要Boot 场景快速启动方案
        /// </summary>
        protected async UniTask CheckYooAssetsInit()
        {
            if (ManagerHelper.IsInitYooAssets)
            {
                return ; 
            }
            
            GfLog.Add(new GfUnityLogWriter());
            
            // 初始化UniTask异步管理器
            AsyncManager.CreateInstance();
            
            // 初始化事件管理器
            EventManager.CreateInstance();
            
            // 初始化YooAssets资源系统
            YooAssets.Initialize();

            // 开始补丁更新流程
            var  configurationModel = new PatchConfigurationModel("DefaultPackage",EPlayMode.EditorSimulateMode);
            PatchOperation operation = new PatchOperation(configurationModel);
            YooAssets.StartOperation(operation);
            await operation.ToUniTask();
            
            // 设置默认的资源包
            var gamePackage = YooAssets.GetPackage("DefaultPackage");
            YooAssets.SetDefaultPackage(gamePackage);

            //初始化依托于YooAssets的资源管理器
            AssetManager.CreateInstance();
            AssetManager.Instance.SetGamePackage(gamePackage);
            
            ManagerHelper.IsInitYooAssets = true;
            
        }
    }
}