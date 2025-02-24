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
        
        [Unique(GfListenerMethodId.RegisterCreateShellWarning)]
        [InvokeTrigger(InvokeTriggerType.Edge)]
        [Skippable(false)]
        void CreateShellWarning(EditorAnimationEventParameterShell param);
    }
}