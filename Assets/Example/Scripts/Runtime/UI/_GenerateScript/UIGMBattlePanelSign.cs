// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2025-02-26 18:00:22.350
//------------------------------------------------------------

namespace GameMain.Runtime
{
    public class UIGMBattlePanelSign : UIPanel
    {
		//---UI---
		protected UnityEngine.UI.Button btnClose = null;
		protected UnityEngine.GameObject btnMod = null;
		protected UnityEngine.RectTransform contentBtns = null;
		protected UnityEngine.RectTransform contentSingleToggles = null;
		protected UnityEngine.GameObject toggleMod = null;
		
		
        public override void OnInit(string name, UnityEngine.GameObject go, UnityEngine.Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);
            InitUIData();
        }

		public void InitUIData()
        {

            ReferenceCollector rc = RootGo.GetComponent<ReferenceCollector>();
			
            btnClose = rc.Get<UnityEngine.UI.Button>("btnClose");
			btnMod = rc.Get<UnityEngine.GameObject>("btnMod");
			contentBtns = rc.Get<UnityEngine.RectTransform>("contentBtns");
			contentSingleToggles = rc.Get<UnityEngine.RectTransform>("contentSingleToggles");
			toggleMod = rc.Get<UnityEngine.GameObject>("toggleMod");
			
        }
    }
}
