namespace GameMain.Runtime
{
    /// <summary>
    /// 角色详情界面
    /// </summary>
    public class UICharacterPanel : UICharacterPanelSign
    {
        public class Params
        {
            public TabType TabType;
            public UICharacterModel CharacterModel;
            
            public Params(UICharacterModel characterModel,TabType tabType = TabType.Detail)
            {
                CharacterModel = characterModel;
                TabType = tabType;
            }
        }
        
        public enum TabType
        {
            Detail,//详情
            Equipment,//装备
        }
        
        private readonly TabType[] _tabTypes = new TabType[]
        {
            TabType.Detail,
            TabType.Equipment,
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

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            
            if (userData is Params data)
            {
                _data = data;
                
                txtTitle.text = _data.CharacterModel.CharacterData.Config.Name;
                
                if (!toggleGroupEx.SetToggleOn((int)_data.TabType))
                {
                    //Toggle没有Changed 这里需要主动调用UpdateView
                    _curType = _data.TabType;
                    UpdateView();
                }
            }
            else if(_data != null)
            {
                //返回界面
                UpdateView();
            }
        }

        private void OnToggleChanged(int toggleIndex)
        {
            _curType = (TabType)toggleIndex;
            UpdateView();
        }

        private void UpdateView()
        {
            detailView.gameObject.SetActive(_curType == TabType.Detail);
            //characterEquipmentView.gameObject.SetActive(_curType == TabType.Equipment);
            
            if (_curType == TabType.Detail)
            {
                //角色详情
                detailView.UpdateView(_data.CharacterModel);
            }
            else if (_curType == TabType.Equipment)
            {
                //装备详情
                //equipmentView
            }
            
        }
    }
}