namespace GameMain.Runtime
{
    public class UISettingPanel : UISettingPanelSign
    {
        public class Params
        {
            public TabType TabType;
            public UICharacterModel CharacterModel;
            
            public Params(TabType tabType = TabType.Graphics)
            {
            }
        }
        
        public enum TabType
        {
            Graphics,//显示
            Input,//输入
            Sound,//声音
            Language,//语言
        }
        
        private readonly TabType[] _tabTypes = new TabType[]
        {
            TabType.Graphics,
            TabType.Input,
            TabType.Sound,
            TabType.Language,
        };
        
        private Params _data;
        private TabType _curType;
        
        public override void OnInit(string name, UnityEngine.GameObject go, UnityEngine.Transform parent,
            object userData)
        {
            base.OnInit(name, go, parent, userData);
            
            btnClose.onClick.AddListener(Close);
            
            toggleGroupEx.Clear();
            for (int i = 0; i < _tabTypes.Length; i++)
            {
                toggleGroupEx.CreateToggleEx((int)_tabTypes[i]);
            }
            
            toggleGroupEx.OnToggleChanged.AddListener(OnToggleChanged);
        }
        
        private void OnToggleChanged(int toggleIndex)
        {
            _curType = (TabType)toggleIndex;
            UpdateView();
        }
        
        private void UpdateView()
        {
            graphics.gameObject.SetActive(_curType == TabType.Graphics);
            input.gameObject.SetActive(_curType == TabType.Input);
            sound.gameObject.SetActive(_curType == TabType.Sound);
            language.gameObject.SetActive(_curType == TabType.Language);
        }
    }
}