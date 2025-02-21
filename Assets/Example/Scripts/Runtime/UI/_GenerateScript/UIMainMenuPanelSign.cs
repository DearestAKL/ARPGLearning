// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-11-29 14:54:11.891
//------------------------------------------------------------

namespace GameMain.Runtime
{
    public class UIMainMenuPanelSign : UIPanel
    {
		//---UI---
		protected UnityEngine.UI.Button btnStaff = null;
		protected UnityEngine.UI.Button btnLanguage = null;
		protected UnityEngine.UI.Button btnSetting = null;
		protected UnityEngine.UI.Button btnInherit = null;
		protected UnityEngine.UI.Button btnCollection = null;
		protected UnityEngine.UI.Button btnStart = null;
		protected UnityEngine.UI.Button btnContinue = null;
		protected UnityEngine.UI.Button btnQuit = null;
		
		
        public override void OnInit(string name, UnityEngine.GameObject go, UnityEngine.Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);
            InitUIData();
        }

		public void InitUIData()
        {

            ReferenceCollector rc = RootGo.GetComponent<ReferenceCollector>();
			
            btnStaff = rc.Get<UnityEngine.UI.Button>("btnStaff");
			btnLanguage = rc.Get<UnityEngine.UI.Button>("btnLanguage");
			btnSetting = rc.Get<UnityEngine.UI.Button>("btnSetting");
			btnInherit = rc.Get<UnityEngine.UI.Button>("btnInherit");
			btnCollection = rc.Get<UnityEngine.UI.Button>("btnCollection");
			btnStart = rc.Get<UnityEngine.UI.Button>("btnStart");
			btnContinue = rc.Get<UnityEngine.UI.Button>("btnContinue");
			btnQuit = rc.Get<UnityEngine.UI.Button>("btnQuit");
			
        }
    }
}
