using GameMain.Runtime;

namespace GameMain.Runtime
{
    public static class GMConfig
    {
        public static bool IgnoreSkillCd { get; private set; }

        public static void SetIgnoreSkillCd(bool enable)
        {
            IgnoreSkillCd = enable;
            BattleAdmin.Player?.Condition.IgnoreSkillCd(enable);
        }
    }
}