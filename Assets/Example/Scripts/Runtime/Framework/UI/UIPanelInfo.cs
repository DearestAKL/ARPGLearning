namespace GameMain.Runtime
{
    public sealed class UIPanelInfo
    {
        private UIPanel _uiPanel;
        private bool _paused;
        private bool _covered;

        public UIPanelInfo()
        {
            _uiPanel = null;
            _paused = false;
            _covered = false;
        }

        public UIPanel UIPanel
        {
            get
            {
                return _uiPanel;
            }
        }

        public bool Paused
        {
            get
            {
                return _paused;
            }
            set
            {
                _paused = value;
            }
        }

        public bool Covered
        {
            get
            {
                return _covered;
            }
            set
            {
                _covered = value;
            }
        }

        public static UIPanelInfo Create(UIPanel uiPanel)
        {

            UIPanelInfo uiPanelInfo = new UIPanelInfo();
            uiPanelInfo._uiPanel = uiPanel;
            uiPanelInfo._paused = true;
            uiPanelInfo._covered = true;
            return uiPanelInfo;
        }

        public void Clear()
        {
            _uiPanel = null;
            _paused = false;
            _covered = false;
        }
    }
}
