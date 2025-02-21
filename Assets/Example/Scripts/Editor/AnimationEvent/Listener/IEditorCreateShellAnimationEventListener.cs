using Akari.GfUnityEditor.AnimationEventBundler;
using GameMain.Runtime;

namespace GameMain.Editor
{
    [GfListenerType(typeof(ICreateShellAnimationEventListener))]
    public interface IEditorCreateShellAnimationEventListener : IAnimationEventListener
    {
        [Unique(GfListenerMethodId.RegisterCreateShell)]
        [InvokeTrigger(InvokeTriggerType.Edge)]
        [Skippable(false)]
        void CreateShell(EditorAnimationEventParameterShell param);
        
        [Unique(GfListenerMethodId.RegisterCreateWarningShell)]
        [InvokeTrigger(InvokeTriggerType.Edge)]
        [Skippable(false)]
        void CreateWarningShell(EditorAnimationEventParameterShell param);
    }
}