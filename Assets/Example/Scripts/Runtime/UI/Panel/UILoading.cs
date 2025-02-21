using DG.Tweening;

namespace GameMain.Runtime
{
    public class UILoading: UILoadingSign
    {
        private bool _isDoAnimationComplete;
        private bool _isFinish;

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            _isFinish = false;
            _isDoAnimationComplete = false;
            DOVirtual.DelayedCall(1F, DoAnimationComplete);
        }

        public void SetFinish()
        {
            _isFinish = true;
            CheckClose();
        }

        private void DoAnimationComplete()
        {
            _isDoAnimationComplete = true;
            CheckClose();
        }

        private void CheckClose()
        {
            if (_isDoAnimationComplete && _isFinish)
            {
                Close();
            }
        }
    }
}