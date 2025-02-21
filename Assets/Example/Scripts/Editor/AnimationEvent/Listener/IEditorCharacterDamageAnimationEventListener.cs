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

    }
}
