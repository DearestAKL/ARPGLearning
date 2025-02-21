using System;

namespace DialogueGraph.Serialize {
    [Serializable]
    public class Property {
        public string Guid;
        public string ReferenceName;
        public string DisplayName;
        public PropertyType Type;
    }
}