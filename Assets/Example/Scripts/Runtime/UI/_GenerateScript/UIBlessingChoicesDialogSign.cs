// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-12-22 20:01:36.177
//------------------------------------------------------------

namespace GameMain.Runtime
{
    public class UIBlessingChoicesDialogSign : UIPanel
    {
		//---UI---
		protected GameMain.Runtime.UIBlessingItem blessingItem_1 = null;
		protected GameMain.Runtime.UIBlessingItem blessingItem_2 = null;
		protected GameMain.Runtime.UIBlessingItem blessingItem_3 = null;
		
		
        public override void OnInit(string name, UnityEngine.GameObject go, UnityEngine.Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);
            InitUIData();
        }

		public void InitUIData()
        {

            ReferenceCollector rc = RootGo.GetComponent<ReferenceCollector>();
			
            blessingItem_1 = rc.Get<GameMain.Runtime.UIBlessingItem>("blessingItem_1");
			blessingItem_2 = rc.Get<GameMain.Runtime.UIBlessingItem>("blessingItem_2");
			blessingItem_3 = rc.Get<GameMain.Runtime.UIBlessingItem>("blessingItem_3");
			
        }
    }
}
