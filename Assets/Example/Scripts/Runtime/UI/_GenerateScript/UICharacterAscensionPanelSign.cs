// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-07-04 16:09:09.991
//------------------------------------------------------------

namespace GameMain.Runtime
{
    public class UICharacterAscensionPanelSign : UIPanel
    {
		//---UI---
		protected UnityEngine.UI.Button btnClose = null;
		protected UnityEngine.UI.Button btnAscension = null;
		protected TMPro.TextMeshProUGUI txtAscension = null;
		protected UnityEngine.GameObject goAscension = null;
		protected UnityEngine.GameObject goLevelUp = null;
		protected TMPro.TextMeshProUGUI txtTitle = null;
		protected TMPro.TextMeshProUGUI txtLevelAdd = null;
		protected TMPro.TextMeshProUGUI txtExpAdd = null;
		protected UnityEngine.GameObject goMax = null;
		protected TMPro.TextMeshProUGUI txtExp = null;
		protected UnityEngine.UI.Image imgExp = null;
		protected TMPro.TextMeshProUGUI txtLevel = null;
		protected TMPro.TextMeshProUGUI txtLevelLimit = null;
		protected UnityEngine.UI.Button btnTestAddExpItem = null;
		protected UnityEngine.UI.Button btnTestRemoveExpItem = null;
		protected GameMain.Runtime.UIAttributeAscensionItem ascensionItemHp = null;
		protected GameMain.Runtime.UIAttributeAscensionItem ascensionItemAttack = null;
		protected GameMain.Runtime.UIAttributeAscensionItem ascensionItemDefense = null;
		protected GameMain.Runtime.UIAttributeAscensionItem ascensionItemBonus = null;
		protected GameMain.Runtime.UIStarCollection ascensionStarCollection = null;
		
		
        public override void OnInit(string name, UnityEngine.GameObject go, UnityEngine.Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);
            InitUIData();
        }

		public void InitUIData()
        {

            ReferenceCollector rc = RootGo.GetComponent<ReferenceCollector>();
			
            btnClose = rc.Get<UnityEngine.UI.Button>("btnClose");
			btnAscension = rc.Get<UnityEngine.UI.Button>("btnAscension");
			txtAscension = rc.Get<TMPro.TextMeshProUGUI>("txtAscension");
			goAscension = rc.Get<UnityEngine.GameObject>("goAscension");
			goLevelUp = rc.Get<UnityEngine.GameObject>("goLevelUp");
			txtTitle = rc.Get<TMPro.TextMeshProUGUI>("txtTitle");
			txtLevelAdd = rc.Get<TMPro.TextMeshProUGUI>("txtLevelAdd");
			txtExpAdd = rc.Get<TMPro.TextMeshProUGUI>("txtExpAdd");
			goMax = rc.Get<UnityEngine.GameObject>("goMax");
			txtExp = rc.Get<TMPro.TextMeshProUGUI>("txtExp");
			imgExp = rc.Get<UnityEngine.UI.Image>("imgExp");
			txtLevel = rc.Get<TMPro.TextMeshProUGUI>("txtLevel");
			txtLevelLimit = rc.Get<TMPro.TextMeshProUGUI>("txtLevelLimit");
			btnTestAddExpItem = rc.Get<UnityEngine.UI.Button>("btnTestAddExpItem");
			btnTestRemoveExpItem = rc.Get<UnityEngine.UI.Button>("btnTestRemoveExpItem");
			ascensionItemHp = rc.Get<GameMain.Runtime.UIAttributeAscensionItem>("ascensionItemHp");
			ascensionItemAttack = rc.Get<GameMain.Runtime.UIAttributeAscensionItem>("ascensionItemAttack");
			ascensionItemDefense = rc.Get<GameMain.Runtime.UIAttributeAscensionItem>("ascensionItemDefense");
			ascensionItemBonus = rc.Get<GameMain.Runtime.UIAttributeAscensionItem>("ascensionItemBonus");
			ascensionStarCollection = rc.Get<GameMain.Runtime.UIStarCollection>("ascensionStarCollection");
			
        }
    }
}
