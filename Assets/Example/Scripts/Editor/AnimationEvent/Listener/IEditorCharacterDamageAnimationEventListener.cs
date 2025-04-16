using Akari.GfUnityEditor.AnimationEventBundler;
using GameMain.Runtime;

namespace GameMain.Editor
{
    [GfListenerType(typeof(ICharacterDamageAnimationEventListener))]
    public interface IEditorCharacterDamageAnimationEventListener : IAnimationEventListener
    {
        [Unique(GfListenerMethodId.RegisterAttack)]
        [InvokeTrigger(InvokeTriggerType.Level)]
        [Skippable(true)]
        void RegisterAttack(EditorAnimationEventParameterAttackId param);
        
        [Unique(GfListenerMethodId.RegisterAttackWarning)]
        [InvokeTrigger(InvokeTriggerType.Level)]
        [Skippable(true)]
        void RegisterAttackWarning(EditorAnimationEventParameterAttackId param);
        
        [Unique(GfListenerMethodId.RegisterCustomWarning)]
        [InvokeTrigger(InvokeTriggerType.Level)]
        [Skippable(true)]
        void RegisterCustomWarning(EditorAnimationEventParameterAttackId param);

    }
}
