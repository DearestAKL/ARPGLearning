using Akari.GfCore;
using Cysharp.Threading.Tasks;
using YooAsset;

namespace GameMain.Runtime
{
	public class PatchClearCacheBundle : PatchBaseState
	{
		public static int Type => (int)PatchStateType.ClearCacheBundle;

		public override int StateType => Type;

		protected override PatchStateType NextState => PatchStateType.StartGame;

		public PatchClearCacheBundle(PatchConfigurationModel configurationModel) : base(configurationModel)
		{
		}

		public override void OnEnter(AGfFsmState prevState, bool reenter)
		{
			base.OnEnter(prevState, reenter);

			AsyncManager.Instance.StartAsync(ClearCache);
		}

		private async UniTask ClearCache()
		{
			EventManager.Instance.PatchEvent.OnPatchStatesChangeEvent.Invoke("清理未使用的缓存文件！");
			var packageName = ConfigurationModel.PackageName;
			var package = YooAssets.GetPackage(packageName);
			var operation = package.ClearCacheFilesAsync(EFileClearMode.ClearUnusedBundleFiles);
			await operation;

			Next();
		}
	}
}