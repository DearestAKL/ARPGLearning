using Akari.GfUnityEditor.AnimationEventBundler;
using GameMain.Runtime;

namespace GameMain.Editor
{
    [GfListenerType(typeof(ICharacterSoundAnimationEventListener))]
    public interface IEditorCharacterSoundAnimationEventListener : IAnimationEventListener
    {
        [Unique(GfListenerMethodId.RegisterCommonSoundPlay)]
        [InvokeTrigger(InvokeTriggerType.Edge)]
        [Skippable(false)]
        [UnityOnly]
        void RegisterCommonSoundPlay(EditorAnimationEventParameterString param);
    }
}