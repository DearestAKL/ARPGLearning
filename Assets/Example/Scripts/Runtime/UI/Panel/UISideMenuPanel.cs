using System;
using Akari.GfCore;
using UnityEngine;

namespace GameMain.Runtime
{
    public class UISideMenuPanel : UISideMenuPanelSign
    {
        public override void OnInit(string name, GameObject go, Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);

            btnCharacter.onClick.AddListener(OnCharacter);
            btnSetting.onClick.AddListener(OnSetting);
            btnBackpack.onClick.AddListener(OnBackpack);
            
            btnClose.onClick.AddListener(Close);
            
            btnSave.onClick.AddListener(OnSave);
            btnLobby.onClick.AddListener(OnLobby);
        }
        
        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            
            goBattleMenu.gameObject.SetActive(RoguelikeRoomManager.HasInstance);
        }

        private async void OnCharacter()
        {
            await UIManager.Instance.OpenUIPanel(UIType.UICharacterPanel,new UICharacterPanel.Params(new UICharacterModel(BattleAdmin.Player.Condition)));
        }

        private async void OnSetting()
        {
            await UIManager.Instance.OpenUIPanel(UIType.UISettingPanel);
        }
        
        private async void OnBackpack()
        {
            await UIManager.Instance.OpenUIPanel(UIType.UIBackpackPanel, new UIBackpackPanel.Params());
        }
        
        private async void OnSave()
        {
            await UIHelper.OpenCommonMessageDialog( "是否保存", () =>
            {
                RoguelikeRoomManager.Instance.Save();
            });
        }
        
        private async void OnLobby()
        {
            await UIHelper.OpenCommonMessageDialog( "是否返回大厅,未保存内容将丢失", () =>
            {
                Close();
                RoguelikeRoomManager.Instance.Exit();
            });
        }
    }
}