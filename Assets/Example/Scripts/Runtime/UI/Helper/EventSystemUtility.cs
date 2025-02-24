using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace GameMain.Runtime
{
    public static class EventSystemUtility
    {
        private static InputSystemUIInputModule _module;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            _module = null;
        }

        public static bool IsPointerOverGUIAction()
        {
            if (!_module)
            {
                if (!EventSystem.current)
                {
                    return false;
                }
                _module = (InputSystemUIInputModule)EventSystem.current.currentInputModule;
            }

            return _module.GetLastRaycastResult(Pointer.current.deviceId).isValid;
        }
    }
}