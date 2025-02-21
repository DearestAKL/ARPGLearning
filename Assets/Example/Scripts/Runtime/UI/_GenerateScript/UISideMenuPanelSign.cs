// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-07-02 16:41:47.233
//------------------------------------------------------------

namespace GameMain.Runtime
{
    public class UISideMenuPanelSign : UIPanel
    {
		//---UI---
		protected UnityEngine.UI.Button btnCharacter = null;
		protected UnityEngine.UI.Button btnSetting = null;
		protected UnityEngine.UI.Button btnSave = null;
		protected UnityEngine.UI.Button btnLobby = null;
		protected UnityEngine.UI.Button btnBackpack = null;
		protected UnityEngine.GameObject goBattleMenu = null;
		protected UnityEngine.GameObject goLobbyMenu = null;
		protected UnityEngine.UI.Button btnClose = null;
		
		
        public override void OnInit(string name, UnityEngine.GameObject go, UnityEngine.Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);
            InitUIData();
        }

		public void InitUIData()
        {

            ReferenceCollector rc = RootGo.GetComponent<ReferenceCollector>();
			
            btnCharacter = rc.Get<UnityEngine.UI.Button>("btnCharacter");
			btnSetting = rc.Get<UnityEngine.UI.Button>("btnSetting");
			btnSave = rc.Get<UnityEngine.UI.Button>("btnSave");
			btnLobby = rc.Get<UnityEngine.UI.Button>("btnLobby");
			btnBackpack = rc.Get<UnityEngine.UI.Button>("btnBackpack");
			goBattleMenu = rc.Get<UnityEngine.GameObject>("goBattleMenu");
			goLobbyMenu = rc.Get<UnityEngine.GameObject>("goLobbyMenu");
			btnClose = rc.Get<UnityEngine.UI.Button>("btnClose");
			
        }
    }
}
