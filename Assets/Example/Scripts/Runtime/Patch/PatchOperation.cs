using Akari;
using Akari.GfCore;
using YooAsset;

namespace GameMain.Runtime
{
    public class PatchOperation : GameAsyncOperation
    {
        private enum ESteps
        {
            None,
            Update,
            Done,
        }
        
        private GfCompositeDisposable _subscriptions;
        private GfFsmStateMachine _stateMachine;
        private ESteps _steps = ESteps.None;

        public PatchOperation(PatchConfigurationModel configurationModel)
        {
            _subscriptions = new GfCompositeDisposable();

            //注册事件
            EventManager.Instance.PatchEvent.OnUserTryInitialize.GfSubscribe(OnUserTryInitialize).AddTo(_subscriptions);
            EventManager.Instance.PatchEvent.OnUserBeginDownloadWebFiles.GfSubscribe(OnUserBeginDownloadWebFiles).AddTo(_subscriptions);
            EventManager.Instance.PatchEvent.OnUserTryUpdatePackageVersion.GfSubscribe(OnUserTryUpdatePackageVersion).AddTo(_subscriptions);
            EventManager.Instance.PatchEvent.OnUserTryUpdatePatchManifest.GfSubscribe(OnUserTryUpdatePatchManifest).AddTo(_subscriptions);
            EventManager.Instance.PatchEvent.OnUserTryDownloadWebFiles.GfSubscribe(OnUserTryDownloadWebFiles).AddTo(_subscriptions);

            _stateMachine = new GfFsmStateMachine();

            _stateMachine.Add(PatchInitializePackage.Type, new PatchInitializePackage(configurationModel));
            _stateMachine.Add(PatchRequestPackageVersion.Type, new PatchRequestPackageVersion(configurationModel));
            _stateMachine.Add(PatchUpdatePackageManifest.Type, new PatchUpdatePackageManifest(configurationModel));
            _stateMachine.Add(PatchCreateDownloader.Type, new PatchCreateDownloader(configurationModel));
            _stateMachine.Add(PatchDownloadPackageFiles.Type, new PatchDownloadPackageFiles(configurationModel));
            _stateMachine.Add(PatchDownloadPackageOver.Type, new PatchDownloadPackageOver(configurationModel));
            _stateMachine.Add(PatchClearCacheBundle.Type, new PatchClearCacheBundle(configurationModel));
            _stateMachine.Add(PatchStartGame.Type, new PatchStartGame(configurationModel));
        }

        protected override void OnStart()
        {
            _stateMachine.ForceTransition(new GfFsmStateTransitionRequest(PatchInitializePackage.Type));
            _steps = ESteps.Update;
        }

        protected override void OnUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
                return;

            if(_steps == ESteps.Update)
            {
                _stateMachine.OnBeginUpdate();
                _stateMachine.OnUpdate();
                _stateMachine.OnEndUpdate();
                
                if(_stateMachine.CurrentStateId == PatchStartGame.Type)
                {
                    _subscriptions?.Dispose();
                    Status = EOperationStatus.Succeed;
                    _steps = ESteps.Done;
                    
                    GfLog.Debug("PatchOperation Succeed");
                }
            }
        }

        protected override void OnAbort()
        {

        }

        private void OnUserTryInitialize()
        {
            _stateMachine.ForceTransition(new GfFsmStateTransitionRequest(PatchInitializePackage.Type));
        }

        private void OnUserBeginDownloadWebFiles()
        {
            _stateMachine.ForceTransition(new GfFsmStateTransitionRequest(PatchDownloadPackageFiles.Type));
        }

        private void OnUserTryUpdatePackageVersion()
        {
            _stateMachine.ForceTransition(new GfFsmStateTransitionRequest(PatchRequestPackageVersion.Type));
        }

        private void OnUserTryUpdatePatchManifest()
        {
            _stateMachine.ForceTransition(new GfFsmStateTransitionRequest(PatchUpdatePackageManifest.Type));
        }

        private void OnUserTryDownloadWebFiles()
        {
            _stateMachine.ForceTransition(new GfFsmStateTransitionRequest(PatchCreateDownloader.Type));
        }
    }
}