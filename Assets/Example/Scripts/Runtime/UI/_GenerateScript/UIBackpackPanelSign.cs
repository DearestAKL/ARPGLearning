// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2025-02-11 21:31:14.086
//------------------------------------------------------------

namespace GameMain.Runtime
{
    public class UIBackpackPanelSign : UIPanel
    {
		//---UI---
		protected UnityEngine.UI.Button btnFunction = null;
		protected GameMain.Runtime.UICustomToggleGroupEx CustomToggleGroupEx = null;
		protected TMPro.TextMeshProUGUI txtTitle = null;
		protected TMPro.TextMeshProUGUI txtLimit = null;
		protected UnityEngine.UI.LoopVerticalScrollRect itemScrollView = null;
		protected UnityEngine.UI.Button btnClose = null;
		protected GameMain.Runtime.UIItemTips itemTips = null;
		protected TMPro.TextMeshProUGUI txtFunction = null;
		protected UnityEngine.GameObject goHasEquip = null;
		protected GameMain.Runtime.UISortOptions sortOptions = null;
		protected UnityEngine.UI.Button btnFilter = null;
		
		
        public override void OnInit(string name, UnityEngine.GameObject go, UnityEngine.Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);
            InitUIData();
        }

		public void InitUIData()
        {

            ReferenceCollector rc = RootGo.GetComponent<ReferenceCollector>();
			
            btnFunction = rc.Get<UnityEngine.UI.Button>("btnFunction");
			CustomToggleGroupEx = rc.Get<GameMain.Runtime.UICustomToggleGroupEx>("toggleGroupEx");
			txtTitle = rc.Get<TMPro.TextMeshProUGUI>("txtTitle");
			txtLimit = rc.Get<TMPro.TextMeshProUGUI>("txtLimit");
			itemScrollView = rc.Get<UnityEngine.UI.LoopVerticalScrollRect>("itemScrollView");
			btnClose = rc.Get<UnityEngine.UI.Button>("btnClose");
			itemTips = rc.Get<GameMain.Runtime.UIItemTips>("itemTips");
			txtFunction = rc.Get<TMPro.TextMeshProUGUI>("txtFunction");
			goHasEquip = rc.Get<UnityEngine.GameObject>("goHasEquip");
			sortOptions = rc.Get<GameMain.Runtime.UISortOptions>("sortOptions");
			btnFilter = rc.Get<UnityEngine.UI.Button>("btnFilter");
			
        }
    }
}
