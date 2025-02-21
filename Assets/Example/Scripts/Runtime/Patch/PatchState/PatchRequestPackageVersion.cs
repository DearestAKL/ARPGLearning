using Akari.GfCore;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

namespace GameMain.Runtime
{
	public class PatchRequestPackageVersion : PatchBaseState
	{
		public static int Type => (int)PatchStateType.RequestPackageVersion;

		public override int StateType => Type;

		protected override PatchStateType NextState => PatchStateType.UpdatePackageManifest;

		public PatchRequestPackageVersion(PatchConfigurationModel configurationModel) : base(configurationModel)
		{
		}

		public override void OnEnter(AGfFsmState prevState, bool reenter)
		{
			base.OnEnter(prevState, reenter);
			AsyncManager.Instance.StartAsync(UpdatePackageVersion);
		}

		private async UniTask UpdatePackageVersion()
		{
			var packageName = ConfigurationModel.PackageName;
			var package = YooAssets.GetPackage(packageName);
			var operation = package.RequestPackageVersionAsync();
			await operation;

			if (operation.Status != EOperationStatus.Succeed)
			{
				Debug.LogWarning(operation.Error);
				EventManager.Instance.PatchEvent.OnPackageVersionUpdateFailedEvent.Invoke();
			}
			else
			{
				GfLog.Debug($"Request package version : {operation.PackageVersion}");
				ConfigurationModel.PackageVersion = operation.PackageVersion;
				Next();
			}
		}
	}
}