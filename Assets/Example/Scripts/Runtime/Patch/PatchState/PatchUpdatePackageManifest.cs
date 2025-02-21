using Akari.GfCore;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

namespace GameMain.Runtime
{

	public class PatchUpdatePackageManifest : PatchBaseState
	{
		public static int Type => (int)PatchStateType.UpdatePackageManifest;

		public override int StateType => Type;

		protected override PatchStateType NextState => PatchStateType.CreateDownloader;

		public PatchUpdatePackageManifest(PatchConfigurationModel configurationModel) : base(configurationModel)
		{
		}

		public override void OnEnter(AGfFsmState prevState, bool reenter)
		{
			base.OnEnter(prevState, reenter);
			AsyncManager.Instance.StartAsync(UpdateManifest);
		}

		private async UniTask UpdateManifest()
		{
			var packageName = ConfigurationModel.PackageName;
			var packageVersion = ConfigurationModel.PackageVersion;
			var package = YooAssets.GetPackage(packageName);
			var operation = package.UpdatePackageManifestAsync(packageVersion);
			await operation;

			if (operation.Status != EOperationStatus.Succeed)
			{
				Debug.LogWarning(operation.Error);
				EventManager.Instance.PatchEvent.OnPatchManifestUpdateFailedEvent.Invoke();
			}
			else
			{
				GfLog.Debug("Update manifest succeed");
				Next();
			}
		}
	}
}