namespace GameMain.Runtime
{
    public class ActiveSkillData
    {
        //技能由 Start，Loop，End动画组合而成，至少拥有一个动画
        //Once 技能，所有动画只执行一次，IsRepeatLoop为false
        //Loop 技能，Loop动画会循环播放，循环时间结束或者长按取消停止
        
        public bool IsRepeatLoop;//技能是否需要循环loop动画
        public float LoopDurationTime;
        
        public bool CanRotate;//技能过程中，是否可以旋转
        public bool CanMove;//技能过程中，是否可以移动


        public static ActiveSkillData CreateOnceSkillData()
        {
            var activeSkillData = new ActiveSkillData();
            
            return activeSkillData;
        }
        
        public static ActiveSkillData CreateLoopSkillData()
        {
            var activeSkillData = new ActiveSkillData();

            activeSkillData.IsRepeatLoop = true;
            activeSkillData.LoopDurationTime = 0.5f;
            
            activeSkillData.CanRotate = true;
            activeSkillData.CanMove = true;
            
            return activeSkillData;
        }
    }
}