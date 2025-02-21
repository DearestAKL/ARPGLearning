namespace GameMain.Runtime
{
    public class GfBtData
    {
        public int StartNodeIndex;
        public BtNodeData[] NodeDatas;
        public BtPropertyData[] PropertyDatas;

        public GfBtData(int startNodeIndex,BtNodeData[] nodeDatas, BtPropertyData[] propertyDatas)
        {
            StartNodeIndex = startNodeIndex;
            NodeDatas = nodeDatas;
            PropertyDatas = propertyDatas;
        }

        public BtStartNodeData GetStartNodeData()
        {
            var nodeData = NodeDatas[StartNodeIndex];
            if (nodeData is BtStartNodeData startNodeData)
            {
                return startNodeData;
            }

            return null;
        }
    }
}