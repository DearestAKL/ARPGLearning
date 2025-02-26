// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-07-26 15:40:13.223
//------------------------------------------------------------

namespace GameMain.Runtime
{
    public class UISettingPanelSign : UIPanel
    {
		//---UI---
		protected UnityEngine.UI.Button btnClose = null;
		protected GameMain.Runtime.UISettingGraphicsView graphics = null;
		protected GameMain.Runtime.UISettingInputView input = null;
		protected GameMain.Runtime.UISettingSoundView sound = null;
		protected GameMain.Runtime.UISettingLanguageView language = null;
		protected GameMain.Runtime.UICustomToggleGroupEx CustomToggleGroupEx = null;
		
		
        public override void OnInit(string name, UnityEngine.GameObject go, UnityEngine.Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);
            InitUIData();
        }

		public void InitUIData()
        {

            ReferenceCollector rc = RootGo.GetComponent<ReferenceCollector>();
			
            btnClose = rc.Get<UnityEngine.UI.Button>("btnClose");
			graphics = rc.Get<GameMain.Runtime.UISettingGraphicsView>("graphics");
			input = rc.Get<GameMain.Runtime.UISettingInputView>("input");
			sound = rc.Get<GameMain.Runtime.UISettingSoundView>("sound");
			language = rc.Get<GameMain.Runtime.UISettingLanguageView>("language");
			CustomToggleGroupEx = rc.Get<GameMain.Runtime.UICustomToggleGroupEx>("toggleGroupEx");
			
        }
    }
}
