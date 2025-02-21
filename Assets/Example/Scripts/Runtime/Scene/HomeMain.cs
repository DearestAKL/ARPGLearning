using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameMain.Runtime
{
    public class HomeMain : BaseMain
    {
        private async void Start()
        {
            await CheckYooAssetsInit();
            await ManagerHelper.InitCommonManager();
            await UIManager.Instance.OpenUIPanel(UIType.UIMainMenuPanel);
        }
        
        private void OnApplicationQuit()
        {
            SettingManager.Instance?.Dispose();
        }
    }
}