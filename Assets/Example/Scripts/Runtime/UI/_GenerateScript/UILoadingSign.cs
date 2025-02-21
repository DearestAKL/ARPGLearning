// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-11-29 14:54:14.968
//------------------------------------------------------------

namespace GameMain.Runtime
{
    public class UILoadingSign : UIPanel
    {
		//---UI---
		protected TMPro.TextMeshProUGUI txtLoading = null;
		protected UnityEngine.UI.Image imgProgress = null;
		
		
        public override void OnInit(string name, UnityEngine.GameObject go, UnityEngine.Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);
            InitUIData();
        }

		public void InitUIData()
        {

            ReferenceCollector rc = RootGo.GetComponent<ReferenceCollector>();
			
            txtLoading = rc.Get<TMPro.TextMeshProUGUI>("txtLoading");
			imgProgress = rc.Get<UnityEngine.UI.Image>("imgProgress");
			
        }
    }
}
