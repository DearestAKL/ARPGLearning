using System;

namespace DialogueGraph.Serialize {
    [Serializable]
    public class Edge {
        public string FromNode;
        public string FromPort;
        public string ToNode;
        public string ToPort;
    }
}