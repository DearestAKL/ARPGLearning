using System;
using Cysharp.Threading.Tasks;

namespace GameMain.Runtime
{
    public class AsyncHandler
    {
        public void StartAsync(Func<UniTask> handler)
        {
            StartAsync(handler.Invoke());
        }
        
        public void StartAsync(UniTask asyncTask)
        {
            asyncTask.ToCoroutine();
        }
    }
}
