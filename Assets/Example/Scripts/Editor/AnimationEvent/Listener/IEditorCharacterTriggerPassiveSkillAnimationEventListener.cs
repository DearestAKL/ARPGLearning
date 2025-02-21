using Akari.GfUnityEditor.AnimationEventBundler;
using GameMain.Runtime;

namespace GameMain.Editor
{
    [GfListenerType(typeof(ICharacterTriggerPassiveSkillAnimationEventListener))]
    public interface IEditorCharacterTriggerPassiveSkillAnimationEventListener : IAnimationEventListener
    {
        [Unique(GfListenerMethodId.RegisterTriggerPassiveSkillEvent)]
        [InvokeTrigger(InvokeTriggerType.Edge)]
        [Skippable(false)]
        void RegisterTriggerPassiveSkillEvent(EditorAnimationEventParameterIntValue param);
    }
}