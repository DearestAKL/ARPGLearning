namespace GameMain.Runtime
{
    public class PassiveSkillAddBufferData
    {
        public int BufferId;
        public SelectTargetType SelectTargetType;
        public ISelectTargetFilter[] Filters;
        public PassiveSkillAddBufferData(int bufferId,SelectTargetType selectTargetType,ISelectTargetFilter[] filters)
        {
            BufferId = bufferId;
            SelectTargetType = selectTargetType;
            Filters = filters;
        }
    }
}