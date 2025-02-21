using Akari.GfCore;
using Cysharp.Threading.Tasks;
using YooAsset;

namespace GameMain.Runtime
{
	public class PatchCreateDownloader : PatchBaseState
	{
		public static int Type => (int)PatchStateType.CreateDownloader;

		public override int StateType => Type;

		protected override PatchStateType NextState => PatchStateType.DownloadPackageFiles;

		public PatchCreateDownloader(PatchConfigurationModel configurationModel) : base(configurationModel)
		{
		}

		public override void OnEnter(AGfFsmState prevState, bool reenter)
		{
			base.OnEnter(prevState, reenter);
			AsyncManager.Instance.StartAsync(CreateDownloader);
		}
		
		private async UniTask CreateDownloader()
		{
			await UniTask.DelayFrame(1);

			var packageName = ConfigurationModel.PackageName;
			var package = YooAssets.GetPackage(packageName);
			int downloadingMaxNum = 10;
			int failedTryAgain = 3;
			var downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);

			ConfigurationModel.Downloader = downloader;

			if (downloader.TotalDownloadCount == 0)
			{
				GfLog.Debug("Not found any download files !");
				Next();
			}
			else
			{
				// 发现新更新文件后，挂起流程系统
				// 注意：开发者需要在下载前检测磁盘空间不足
				int totalDownloadCount = downloader.TotalDownloadCount;
				long totalDownloadBytes = downloader.TotalDownloadBytes;
				EventManager.Instance.PatchEvent.OnFoundUpdateFilesEvent.Invoke(totalDownloadCount, totalDownloadBytes);
			}
		}
	}
}