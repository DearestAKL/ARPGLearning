using System;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public static class AnimationEventCallReasonHelper
    {
        public static bool ToIsOn(this GfAnimationEventCallReason reason)
        {
            if (reason == GfAnimationEventCallReason.Begin)
            {
                return true;
            }
            else if (reason == GfAnimationEventCallReason.End)
            {
                return false;
            }

            throw new ArgumentException($"[{nameof(ToIsOn)}] Invalid GfAnimationEventCallReason: {reason}");
        }
    }
}
