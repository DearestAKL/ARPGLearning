namespace NPBehave
{
    /// <summary>
    /// 特殊节点，等待停止节点
    /// 等待被其他节点停止。它通常用于Selector的末尾，等待任何before头的同级BlackboardCondition、BlackboardQuery或Condition变为活动状态。
    /// </summary>
    public class WaitUntilStopped : Task
    {
        private bool sucessWhenStopped;
        public WaitUntilStopped(bool sucessWhenStopped = false) : base("WaitUntilStopped")
        {
            this.sucessWhenStopped = sucessWhenStopped;
        }

        protected override void DoStop()
        {
            this.Stopped(sucessWhenStopped);
        }
    }
}