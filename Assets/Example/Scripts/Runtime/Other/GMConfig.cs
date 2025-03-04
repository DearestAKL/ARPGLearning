using Akari.GfGame;
using GameMain.Runtime;

namespace GameMain.Runtime
{
    public static class GMConfig
    {
        public static bool IgnoreSkillCd { get; private set; }
        public static bool ApplyRootMotion { get; private set; } = true;

        public static void SetIgnoreSkillCd(bool enable)
        {
            IgnoreSkillCd = enable;
            BattleAdmin.Player?.Condition.IgnoreSkillCd(enable);
        }
        
        public static void SetApplyRootMotion(bool enable)
        {
            ApplyRootMotion = enable;
            if (BattleAdmin.Player != null)
            {
                BattleAdmin.Player.Entity.GetComponent<GfAnimationComponent>().GetTrack<GfRootMotionTrack>().IsEnabled = enable;
            }
        }
    }
}