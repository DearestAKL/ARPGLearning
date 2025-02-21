using Akari.GfCore;

namespace GameMain.Runtime
{
    public enum PatchStateType : int
    {
        InitializePackage,
        RequestPackageVersion,
        UpdatePackageManifest,
        CreateDownloader,
        DownloadPackageFiles,
        DownloadPackageOver,
        ClearCacheBundle,
        StartGame
    }

    public abstract class PatchBaseState : AGfFsmState
    {
        public abstract int StateType { get; }
        protected virtual PatchStateType NextState { get; }

        protected PatchConfigurationModel ConfigurationModel { get; }

        protected PatchBaseState(PatchConfigurationModel configurationModel)
        {
            ConfigurationModel = configurationModel;
            // ReSharper disable once VirtualMemberCallInConstructor
            AddTransitionCondition();
        }

        public override void OnAwake()
        {

        }

        public override void OnEnter(AGfFsmState prevState, bool reenter)
        {

        }

        public override void OnStart()
        {

        }

        public override void OnExit(AGfFsmState nextState)
        {

        }

        public override void OnDelete()
        {

        }

        public override void OnBeginUpdate(float deltaTime)
        {

        }

        public override void OnUpdate(float deltaTime)
        {

        }

        public override void OnEndUpdate(float deltaTime)
        {

        }

        protected void Next()
        {
            this.RequestTransition(new GfFsmStateTransitionRequest((int)NextState));
        }
        
        protected virtual void AddTransitionCondition()
        {
            AddTransition(new GfFsmStateTransitionAlways());
        }
    }
}