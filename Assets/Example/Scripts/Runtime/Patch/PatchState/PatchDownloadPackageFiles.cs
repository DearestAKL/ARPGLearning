using Akari.GfCore;
using Cysharp.Threading.Tasks;
using YooAsset;

namespace GameMain.Runtime
{
	public class PatchDownloadPackageFiles : PatchBaseState
	{
		public static int Type => (int)PatchStateType.DownloadPackageFiles;

		public override int StateType => Type;

		protected override PatchStateType NextState => PatchStateType.DownloadPackageOver;

		public PatchDownloadPackageFiles(PatchConfigurationModel configurationModel) : base(configurationModel)
		{
		}

		public override void OnEnter(AGfFsmState prevState, bool reenter)
		{
			base.OnEnter(prevState, reenter);

			EventManager.Instance.PatchEvent.OnPatchStatesChangeEvent.Invoke("开始下载补丁文件！");
			AsyncManager.Instance.StartAsync(BeginDownload);
		}

		private async UniTask BeginDownload()
		{
			var downloader = ConfigurationModel.Downloader;

			downloader.DownloadErrorCallback = (errorData) =>
			{
				EventManager.Instance.PatchEvent.OnWebFileDownloadFailedEvent.Invoke(errorData);
			};

			downloader.DownloadUpdateCallback = (updateData) =>
			{
				EventManager.Instance.PatchEvent.OnDownloadProgressUpdateEvent.Invoke(updateData);
			};

			downloader.BeginDownload();
			await downloader;

			// 检测下载结果
			if (downloader.Status != EOperationStatus.Succeed)
			{
				return;
			}

			Next();
		}
	}
}