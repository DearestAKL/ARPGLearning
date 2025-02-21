using Akari.GfUnity;
using UnityEngine;

namespace GameMain.Runtime
{
    public enum BattleUnityEffectOffsetType
    {
        BoundsCenter = 0,
        Root,
        RootAttach
    }
    
    public class BattleUnityEffectView  : GfUnityVfxView
    {
        [SerializeField] private BattleUnityEffectOffsetType offsetType = BattleUnityEffectOffsetType.BoundsCenter;

        public BattleUnityEffectOffsetType OffsetType => offsetType;
        
#if UNITY_EDITOR
        public override void Preparation()
        {
            base.Preparation();
        }
#endif
    }
}