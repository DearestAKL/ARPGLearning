using Akari.GfUnityEditor.AnimationEventBundler;
using GameMain.Runtime;

namespace GameMain.Editor
{
    [GfListenerType(typeof(ICharacterEffectAnimationEventListener))]
    public interface IEditorCharacterEffectAnimationEventListener : IAnimationEventListener
    {
        [Unique(GfListenerMethodId.RegisterEffectPlay)]
        [InvokeTrigger(InvokeTriggerType.Edge)]
        [Skippable(false)]
        [UnityOnly]
        void RegisterEffectPlay(EditorAnimationEventParameterEffectPlay param);
        
        [Unique(GfListenerMethodId.RegisterLoopEffectPlay)]
        [InvokeTrigger(InvokeTriggerType.Level)]
        [Skippable(false)]
        [UnityOnly]
        void RegisterLoopEffectPlay(EditorAnimationEventParameterEffectPlay param);
        
        [Unique(GfListenerMethodId.RegisterEffectAttach)]
        [InvokeTrigger(InvokeTriggerType.Edge)]
        [Skippable(false)]
        [UnityOnly]
        void RegisterEffectAttach(EditorAnimationEventParameterEffectPlay param);
        
        [Unique(GfListenerMethodId.RegisterEffectDetach)]
        [InvokeTrigger(InvokeTriggerType.Edge)]
        [Skippable(false)]
        [UnityOnly]
        void RegisterEffectDetach(AnimationEventParameterString param);
    }
}
