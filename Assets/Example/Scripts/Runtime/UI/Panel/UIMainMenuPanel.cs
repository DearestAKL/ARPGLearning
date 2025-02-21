namespace GameMain.Runtime
{
    public class UIMainMenuPanel : UIMainMenuPanelSign
    {
        public override void OnInit(string name, UnityEngine.GameObject go, UnityEngine.Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);
                
            btnStart.onClick.AddListener(ClickStart);
            btnContinue.onClick.AddListener(ClickContinue);
                
            btnSetting.onClick.AddListener(ClickSetting);
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
                
            //SoundManager.Instance.PlayBgm(Constant.Sound.MenuBgm);
        }

        private async void ClickStart()
        {
            await UIHelper.StartLoading();
            
            //切换到战斗场景
            AssetManager.Instance.LoadSceneAsync("Assets/Example/GameRes/Scene/BattleScene");
            Close();
        }

        private void ClickContinue()
        {

        }

        private void ClickSetting()
        {
            //UIManager.Instance.OpenUIPanel(UIType.UISettingDialog, new UISettingDialog.Params(false));
        }
    }
}