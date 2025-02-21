using Akari.GfCore;

namespace GameMain.Runtime
{
    public class PatchDownloadPackageOver : PatchBaseState
    {
        public static int Type => (int)PatchStateType.DownloadPackageOver;

        public override int StateType => Type;

        protected override PatchStateType NextState => PatchStateType.ClearCacheBundle;

        public PatchDownloadPackageOver(PatchConfigurationModel configurationModel) : base(configurationModel)
        {
        }

        public override void OnEnter(AGfFsmState prevState, bool reenter)
        {
            base.OnEnter(prevState, reenter);
            Next();
        }
    }
}