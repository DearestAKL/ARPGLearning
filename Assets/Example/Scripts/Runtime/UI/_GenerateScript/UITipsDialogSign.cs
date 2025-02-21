// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-07-02 19:11:24.297
//------------------------------------------------------------

namespace GameMain.Runtime
{
    public class UITipsDialogSign : UIPanel
    {
		//---UI---
		protected UnityEngine.UI.Button btnBackground = null;
		protected TMPro.TextMeshProUGUI txtContent = null;
		
		
        public override void OnInit(string name, UnityEngine.GameObject go, UnityEngine.Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);
            InitUIData();
        }

		public void InitUIData()
        {

            ReferenceCollector rc = RootGo.GetComponent<ReferenceCollector>();
			
            btnBackground = rc.Get<UnityEngine.UI.Button>("btnBackground");
			txtContent = rc.Get<TMPro.TextMeshProUGUI>("txtContent");
			
        }
    }
}
