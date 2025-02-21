using System;
using DialogueGraph.Runtime;
using DialogueGraph.Serialize;

namespace DialogueGraph {
    [Serializable]
    public class CheckProperty : AbstractProperty {
        public CheckProperty() {
            DisplayName = "Check";
            Type = PropertyType.Check;
        }

        public override AbstractProperty Copy() {
            return new CheckProperty {
                DisplayName = DisplayName,
                Hidden = Hidden
            };
        }
    }
}