using System;
using System.Collections.Generic;

namespace DialogueGraph.Serialize {
    [Serializable]
    public class ConversationLine {
        public string Message;
        public string Next;
        public string TriggerPort;
        public string CheckPort;
        public List<string> Triggers;
        public List<string> Checks;
        public List<CheckTree> CheckTrees;
    }
}