using System;
using Akari.GfCore;
using YooAsset;

namespace GameMain.Runtime
{
    public class InitializeFailedEvent : GfEvent
    {
    }
    
    public class PatchStatesChangeEvent : GfEvent<string>
    {
    }

    public class FoundUpdateFilesEvent : GfEvent<int, long>
    {
    }
    
    public class DownloadProgressUpdateEvent : GfEvent<DownloadUpdateData>
    {
    }
    
    public class PackageVersionUpdateFailedEvent : GfEvent
    {
    }
    
    public class PatchManifestUpdateFailedEvent : GfEvent
    {
    }
    
    public class WebFileDownloadFailedEvent : GfEvent<DownloadErrorData>
    {
    }

    public class UserTryInitialize : GfEvent
    {
    }
    
    public class UserBeginDownloadWebFiles : GfEvent
    {
    }
    
    public class UserTryUpdatePackageVersion : GfEvent
    {
    }
    
    public class UserTryUpdatePatchManifest : GfEvent
    {
    }
    
    public class UserTryDownloadWebFiles : GfEvent
    {
    }

    
    public class PatchEventContainer: IDisposable
    {
        public InitializeFailedEvent    OnInitializeFailedEvent    { get; } = new InitializeFailedEvent();
        public PatchStatesChangeEvent    OnPatchStatesChangeEvent    { get; } = new PatchStatesChangeEvent();
        public FoundUpdateFilesEvent    OnFoundUpdateFilesEvent    { get; } = new FoundUpdateFilesEvent();
        public DownloadProgressUpdateEvent    OnDownloadProgressUpdateEvent    { get; } = new DownloadProgressUpdateEvent();
        public PackageVersionUpdateFailedEvent    OnPackageVersionUpdateFailedEvent    { get; } = new PackageVersionUpdateFailedEvent();
        public PatchManifestUpdateFailedEvent    OnPatchManifestUpdateFailedEvent    { get; } = new PatchManifestUpdateFailedEvent();
        public WebFileDownloadFailedEvent    OnWebFileDownloadFailedEvent    { get; } = new WebFileDownloadFailedEvent();
        
        public UserTryInitialize    OnUserTryInitialize    { get; } = new UserTryInitialize();
        public UserBeginDownloadWebFiles    OnUserBeginDownloadWebFiles    { get; } = new UserBeginDownloadWebFiles();
        public UserTryUpdatePackageVersion    OnUserTryUpdatePackageVersion    { get; } = new UserTryUpdatePackageVersion();
        public UserTryUpdatePatchManifest    OnUserTryUpdatePatchManifest    { get; } = new UserTryUpdatePatchManifest();
        public UserTryDownloadWebFiles    OnUserTryDownloadWebFiles    { get; } = new UserTryDownloadWebFiles();

        public void Dispose()
        {
            
        }
    }
}