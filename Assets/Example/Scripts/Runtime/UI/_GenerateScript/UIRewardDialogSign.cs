// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2025-02-11 22:07:21.665
//------------------------------------------------------------

namespace GameMain.Runtime
{
    public class UIRewardDialogSign : UIPanel
    {
		//---UI---
		protected UnityEngine.UI.Button btnBg = null;
		protected UnityEngine.UI.LoopHorizontalScrollRect itemScrollView = null;
		protected UnityEngine.UI.HorizontalLayoutGroup layoutGroup = null;
		
		
        public override void OnInit(string name, UnityEngine.GameObject go, UnityEngine.Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);
            InitUIData();
        }

		public void InitUIData()
        {

            ReferenceCollector rc = RootGo.GetComponent<ReferenceCollector>();
			
            btnBg = rc.Get<UnityEngine.UI.Button>("btnBg");
			itemScrollView = rc.Get<UnityEngine.UI.LoopHorizontalScrollRect>("itemScrollView");
			layoutGroup = rc.Get<UnityEngine.UI.HorizontalLayoutGroup>("layoutGroup");
			
        }
    }
}
