using Akari.GfUnityEditor.AnimationEventBundler;
using GameMain.Runtime;

namespace GameMain.Editor
{
    [GfListenerType(typeof(ICharacterActionCancelAnimationEventListener))]
    public interface IEditorCharacterActionCancelAnimationEventListener : IAnimationEventListener
    {
        [Unique(GfListenerMethodId.RegisterCanMove)]
        [InvokeTrigger(InvokeTriggerType.Level)]
        [Skippable(true)]
        void RegisterCanMove(EditorAnimationEventParameterNull param);

        [Unique(GfListenerMethodId.RegisterCanAttack)]
        [InvokeTrigger(InvokeTriggerType.Level)]
        [Skippable(true)]
        void RegisterCanAttack(EditorAnimationEventParameterNull param);
        
        [Unique(GfListenerMethodId.RegisterCanDash)]
        [InvokeTrigger(InvokeTriggerType.Level)]
        [Skippable(true)]
        void RegisterCanDash(EditorAnimationEventParameterNull param);
        
        [Unique(GfListenerMethodId.RegisterIsDamageImmunity)]
        [InvokeTrigger(InvokeTriggerType.Level)]
        [Skippable(true)]
        void RegisterIsDamageImmunity(EditorAnimationEventParameterNull param);
        
        [Unique(GfListenerMethodId.RegisterIsSuperArmor)]
        [InvokeTrigger(InvokeTriggerType.Level)]
        [Skippable(true)]
        void RegisterIsSuperArmor(EditorAnimationEventParameterNull param);
        
        [Unique(GfListenerMethodId.RegisterIsDodge)]
        [InvokeTrigger(InvokeTriggerType.Level)]
        [Skippable(true)]
        void RegisterIsDodge(EditorAnimationEventParameterNull param);
    }
}
