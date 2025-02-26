// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2025-02-26 18:02:45.047
//------------------------------------------------------------

namespace GameMain.Runtime
{
    public class UIGMCommonPanelSign : UIPanel
    {
		//---UI---
		protected UnityEngine.UI.Button btnAddItem = null;
		protected TMPro.TMP_InputField inputAddItemId = null;
		protected TMPro.TMP_InputField inputAddItemNum = null;
		protected TMPro.TMP_Dropdown dropdownItem = null;
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
			
            btnAddItem = rc.Get<UnityEngine.UI.Button>("btnAddItem");
			inputAddItemId = rc.Get<TMPro.TMP_InputField>("inputAddItemId");
			inputAddItemNum = rc.Get<TMPro.TMP_InputField>("inputAddItemNum");
			dropdownItem = rc.Get<TMPro.TMP_Dropdown>("dropdownItem");
			btnClose = rc.Get<UnityEngine.UI.Button>("btnClose");
			btnMod = rc.Get<UnityEngine.GameObject>("btnMod");
			contentBtns = rc.Get<UnityEngine.RectTransform>("contentBtns");
			contentSingleToggles = rc.Get<UnityEngine.RectTransform>("contentSingleToggles");
			toggleMod = rc.Get<UnityEngine.GameObject>("toggleMod");
			
        }
    }
}
