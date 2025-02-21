// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-06-28 17:58:50.961
//------------------------------------------------------------

namespace GameMain.Runtime
{
    public class UICommonMessageDialogSign : UIPanel
    {
		//---UI---
		protected UnityEngine.GameObject goTitle = null;
		protected UnityEngine.GameObject goButtonDown = null;
		protected TMPro.TextMeshProUGUI txtContent = null;
		protected UnityEngine.UI.Button btnConfirm = null;
		protected UnityEngine.UI.Button btnCancel = null;
		protected TMPro.TextMeshProUGUI txtTitle = null;
		protected TMPro.TextMeshProUGUI txtCancel = null;
		protected TMPro.TextMeshProUGUI txtConfirm = null;
		protected UnityEngine.UI.Button btnBackground = null;
		protected TMPro.TextMeshProUGUI txtBackground = null;
		
		
        public override void OnInit(string name, UnityEngine.GameObject go, UnityEngine.Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);
            InitUIData();
        }

		public void InitUIData()
        {

            ReferenceCollector rc = RootGo.GetComponent<ReferenceCollector>();
			
            goTitle = rc.Get<UnityEngine.GameObject>("goTitle");
			goButtonDown = rc.Get<UnityEngine.GameObject>("goButtonDown");
			txtContent = rc.Get<TMPro.TextMeshProUGUI>("txtContent");
			btnConfirm = rc.Get<UnityEngine.UI.Button>("btnConfirm");
			btnCancel = rc.Get<UnityEngine.UI.Button>("btnCancel");
			txtTitle = rc.Get<TMPro.TextMeshProUGUI>("txtTitle");
			txtCancel = rc.Get<TMPro.TextMeshProUGUI>("txtCancel");
			txtConfirm = rc.Get<TMPro.TextMeshProUGUI>("txtConfirm");
			btnBackground = rc.Get<UnityEngine.UI.Button>("btnBackground");
			txtBackground = rc.Get<TMPro.TextMeshProUGUI>("txtBackground");
			
        }
    }
}
