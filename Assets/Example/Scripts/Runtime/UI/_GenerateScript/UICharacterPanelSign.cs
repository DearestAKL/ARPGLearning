// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-07-09 20:21:10.291
//------------------------------------------------------------

namespace GameMain.Runtime
{
    public class UICharacterPanelSign : UIPanel
    {
		//---UI---
		protected GameMain.Runtime.UICustomToggleGroupEx CustomToggleGroupEx = null;
		protected GameMain.Runtime.UICharacterDetailView detailView = null;
		protected GameMain.Runtime.UICharacterEquipmentView characterEquipmentView = null;
		protected UnityEngine.UI.Button btnClose = null;
		protected TMPro.TextMeshProUGUI txtTitle = null;
		
		
        public override void OnInit(string name, UnityEngine.GameObject go, UnityEngine.Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);
            InitUIData();
        }

		public void InitUIData()
        {

            ReferenceCollector rc = RootGo.GetComponent<ReferenceCollector>();
			
            CustomToggleGroupEx = rc.Get<GameMain.Runtime.UICustomToggleGroupEx>("toggleGroupEx");
			detailView = rc.Get<GameMain.Runtime.UICharacterDetailView>("detailView");
			btnClose = rc.Get<UnityEngine.UI.Button>("btnClose");
			txtTitle = rc.Get<TMPro.TextMeshProUGUI>("txtTitle");
			
        }
    }
}
