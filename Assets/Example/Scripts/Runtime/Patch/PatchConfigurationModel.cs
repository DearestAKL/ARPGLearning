using YooAsset;

namespace GameMain.Runtime
{
    public class PatchConfigurationModel
    {
        public readonly string PackageName;
        public readonly EPlayMode PlayMode;

        public string PackageVersion;
        public ResourceDownloaderOperation Downloader;

        public PatchConfigurationModel(string packageName, EPlayMode playMode)
        {
            PackageName = packageName;
            PlayMode = playMode;
        }
    }
}