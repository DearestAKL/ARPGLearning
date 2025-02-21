namespace DialogueGraph.Runtime
{
    public enum NodeType 
    {
        DIALOGUE,
        OPTION,
        PROP,

        BOOLEAN_NOT,
        BOOLEAN_AND,
        BOOLEAN_OR,
        BOOLEAN_XOR,
        BOOLEAN_NAND,
        BOOLEAN_NOR,
        BOOLEAN_XNOR,

        BOOLEAN_START = BOOLEAN_NOT,
        BOOLEAN_END = BOOLEAN_XNOR,
    }
}