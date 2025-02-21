using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameMain.Runtime
{
    public class UIFishPanel : UIFishPanelSign
    {
        public class Params
        {
            public FishData FishData;
            public FishingRodData FishingRodData;

            public Params(FishData fishData,FishingRodData fishingRodData)
            {
                FishData = fishData;
                FishingRodData = fishingRodData;
            }
        }
        
        private RectTransform _rectCatchingBar;//锚点Pivot 为（0.5，0.5）,不可x轴拉伸
        private RectTransform _rectCatchingForce;//锚点Pivot 为（0，0.5）,不可x轴拉伸
        private RectTransform _rectCatchingFrame;//锚点Pivot 为（0，0.5）,不可x轴拉伸
        
        private float BarWidth => _rectCatchingBar.sizeDelta.x;
        private float BarOrigin => _rectCatchingBar.anchoredPosition.x - _rectCatchingBar.sizeDelta.x / 2;
        private float FrameWidth => _rectCatchingFrame.sizeDelta.x;

        private float _curForce;//当前拉力
        private bool _isHandling = false;
        
        private float _escapeTime;//逃脱经过时间
        private float _changeTime;//改变位置经过时间
        private float _energy;//当前精力
        
        private FishStatus _status = FishStatus.None;
        
        private Params _data;
        
        public override void OnInit(string name, GameObject go, Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);
            
            _rectCatchingBar = imgCatchingBar.GetComponent<RectTransform>();
            _rectCatchingForce = imgCatchingForce.GetComponent<RectTransform>();
            _rectCatchingFrame = imgCatchingFrame.GetComponent<RectTransform>();

            InitInputAction();
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            if (userData is Params data)
            {
                _data = data;
                
                _curForce = 0;
                _isHandling = false;
                
                _escapeTime = 0;
                _changeTime = 0;
                _energy = _data.FishData.Energy;
                
                _status = FishStatus.None;
                
                txtInfo.text = "等待点击屏幕开始";
                imgProgress.fillAmount = 0F;
                _rectCatchingForce.sizeDelta = new Vector2(0, _rectCatchingForce.sizeDelta.y);
            
                // 确认拉力框宽度
                float frameWidth = FishDef.CalculateFrameWidth(data.FishData,data.FishingRodData,BarWidth);
                _rectCatchingFrame.sizeDelta = new Vector2(frameWidth, _rectCatchingFrame.sizeDelta.y);
            
                UpdateCatchingFrameView(0f);
            }
            
            //钓鱼中
            BattleUnityAdmin.BattleInput.SetBattleInputDisable(true);
        }

        public override void OnClose()
        {
            base.OnClose();
            
            //钓鱼结束
            BattleUnityAdmin.BattleInput.SetBattleInputDisable(false);
        }

        public override void OnUpdate(float elapseSeconds)
        {
            base.OnUpdate(elapseSeconds);
            
            if (_status == FishStatus.None || _status == FishStatus.Escape || _status == FishStatus.Catch)
            {
                return;
            }

            //Change Catching Frame
            _changeTime += elapseSeconds;
            if (_changeTime > FishDef.FishChangeTime)
            {
                _changeTime = 0;
                UpdateCatchingFrameView();
            }
            
            //ForceView
            _curForce += _isHandling ? elapseSeconds * FishDef.AddForceCoefficient : -elapseSeconds * FishDef.SubForceCoefficient;
            _curForce = Mathf.Clamp(_curForce, 0, 1F);
            UpdateForceView();

            //Energy
            if (_status == FishStatus.InBar)
            {
                _energy -= elapseSeconds * _data.FishingRodData.DragPower;
            }
            else if (_status == FishStatus.OutBar)
            {
                if (_energy < _data.FishData.Energy)
                {
                    _energy += elapseSeconds * FishDef.EnergyRecovery;
                }
                else
                {
                    _escapeTime += elapseSeconds;
                    if (_escapeTime > FishDef.FishEscapeTime)
                    {
                        SetStatus(FishStatus.Escape);
                    }
                }
            }

            if (_energy <= 0)
            {
                SetStatus(FishStatus.Catch);
            }

            // 显示当前进度
            imgProgress.fillAmount = (_data.FishData.Energy - _energy) / _data.FishData.Energy;
        }
        
        private void UpdateForceView()
        {
            // 计算捕捉力条的宽度
            float forceWidth = BarWidth * _curForce;

            // 更新捕捉力条的尺寸
            _rectCatchingForce.sizeDelta = new Vector2(forceWidth, _rectCatchingForce.sizeDelta.y);

            // 计算捕捉框架的左右边界位置
            float frameLeft = _rectCatchingFrame.anchoredPosition.x - BarOrigin;
            float frameRight = frameLeft + FrameWidth;

            // 检查捕捉力条是否在捕捉框架内
            bool inBar = forceWidth >= frameLeft && forceWidth <= frameRight;

            SetStatus(inBar ? FishStatus.InBar : FishStatus.OutBar);
        }

        private void UpdateCatchingFrameView(float duration = 0.5f)
        {
            float startPos = Random.Range(0, BarWidth - FrameWidth);
            _rectCatchingFrame.DOKill();

            // 计算最小力的位置
            float minForceX = BarOrigin + startPos;
            if (duration > 0f)
            {
                _rectCatchingFrame.DOLocalMoveX(minForceX, 0.5F);
            }
            else
            {
                _rectCatchingFrame.SetLocalPosX(minForceX);
            }
        }

        private void SetStatus(FishStatus status)
        {
            if (_status == status) return;

            _status = status;

            switch (_status)
            {
                case FishStatus.InBar:
                    txtInfo.text = "InBar";
                    break;
                case FishStatus.OutBar:
                    txtInfo.text = "OutBar";
                    break;
                case FishStatus.Escape:
                    txtInfo.text = "逃脱(TEST:R键重新开始)";
                    //TODO:表现
                    Close();
                    break;
                case FishStatus.Catch:
                    txtInfo.text = "捕获(TEST:R键重新开始)";
                    //TODO:表现
                    Close();
                    break;
            }
        }
        
        #region Input

        private void InitInputAction()
        {
            BattleUnityAdmin.PlayerInput.actions[Constant.InputDef.Fishing].started += OnFireStarted;
            BattleUnityAdmin.PlayerInput.actions[Constant.InputDef.Fishing].canceled  += OnFireCanceled;
        }

        private void OnFireStarted(InputAction.CallbackContext context)
        {
            if (!IsActive) { return; }
            
            if (_status == FishStatus.None)
            {
                SetStatus(FishStatus.Start);
            }
                
            _isHandling = true;
        }
        
        private void OnFireCanceled(InputAction.CallbackContext context)
        {
            if (!IsActive) { return; }
            
            _isHandling = false;
        }
        #endregion
    }
}