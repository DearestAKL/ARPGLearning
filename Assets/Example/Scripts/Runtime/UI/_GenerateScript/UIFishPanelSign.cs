// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-07-23 15:23:46.819
//------------------------------------------------------------

namespace GameMain.Runtime
{
    public class UIFishPanelSign : UIPanel
    {
		//---UI---
		protected UnityEngine.UI.Image imgCatchingBar = null;
		protected UnityEngine.UI.Image imgCatchingForce = null;
		protected UnityEngine.UI.Image imgCatchingFrame = null;
		protected UnityEngine.UI.Image imgProgress = null;
		protected TMPro.TextMeshProUGUI txtInfo = null;
		
		
        public override void OnInit(string name, UnityEngine.GameObject go, UnityEngine.Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);
            InitUIData();
        }

		public void InitUIData()
        {

            ReferenceCollector rc = RootGo.GetComponent<ReferenceCollector>();
			
            imgCatchingBar = rc.Get<UnityEngine.UI.Image>("imgCatchingBar");
			imgCatchingForce = rc.Get<UnityEngine.UI.Image>("imgCatchingForce");
			imgCatchingFrame = rc.Get<UnityEngine.UI.Image>("imgCatchingFrame");
			imgProgress = rc.Get<UnityEngine.UI.Image>("imgProgress");
			txtInfo = rc.Get<TMPro.TextMeshProUGUI>("txtInfo");
			
        }
    }
}
