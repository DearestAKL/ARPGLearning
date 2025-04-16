using NPBehave;

namespace GameMain.Runtime
{
    /// <summary>
    /// Debug 节点
    /// </summary>
    public class DebugLog : Task
    {
        public string Message;
        public string BlackboardKey;
        
        public static DebugLog CreatDebugLogByMessage(string message)
        {
            var debugLog = new DebugLog();
            debugLog.Message = message;
            return debugLog;
        }
        
        public static DebugLog CreatDebugLogByPropertyKey(string blackboardKey)
        {
            var debugLog = new DebugLog();
            debugLog.BlackboardKey = blackboardKey;
            return debugLog;
        }

        public DebugLog() : base("Debug")
        {

        }
        
        // public DebugLog(string message) : base("Debug")
        // {
        //     this.message = message;
        // }
        //
        // public DebugLog(int propertyIndex) : base("Debug")
        // {
        //     this.blackboardKey = propertyIndex.ToString();
        // }

        protected override void DoStart()
        {
            // if (!string.IsNullOrEmpty(BlackboardKey))
            // {
            //     var data = Blackboard.Get<BtStringPropertyData>(BlackboardKey);
            //     UnityEngine.Debug.Log(data.Content);
            // }
            // else
            {
                UnityEngine.Debug.Log(Message);
            }

            Stopped(true);
        }
    }
}