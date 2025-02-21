using System;
using Akari.GfCore;
using Cysharp.Threading.Tasks;

namespace GameMain.Runtime
{
    public class AsyncManager : GfSingleton<AsyncManager>
    {
        private AsyncHandler _asyncHandler;
        
        protected override void OnCreated()
        {
            _asyncHandler = new AsyncHandler();
            base.OnCreated();
        }

        protected override void OnDisposed()
        {
            
        }
        
        public void StartAsync(Func<UniTask> handler)
        {
            _asyncHandler.StartAsync(handler);
        }
        
        public void StartAsync(UniTask asyncTask)
        {
            _asyncHandler.StartAsync(asyncTask);
        }
    }
}
