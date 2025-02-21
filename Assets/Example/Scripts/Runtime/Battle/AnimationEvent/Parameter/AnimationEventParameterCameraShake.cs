using Akari.GfCore;
using Akari.GfGame;
using Google.Protobuf;

namespace GameMain.Runtime
{
    public class AnimationEventParameterCameraShake : IGfAnimationEventParameter
    {
        public GfRunTimeTypeId RttId { get; }

        public BattleCameraShakeParam.ShakeDirection direction;
        public BattleCameraShakeParam.ShakePower power;

        public AnimationEventParameterCameraShake()
        {
            RttId = GfRunTimeTypeOf<AnimationEventParameterCameraShake>.Id;
        }
    }

    public sealed class AnimationEventParameterCameraShakeFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new AnimationEventParameterMessageCameraShake();
        }
    
        public object CreateInstance(IMessage message)
        {
            var m = (AnimationEventParameterMessageCameraShake)message;
            var i = new AnimationEventParameterCameraShake();
            i.power = ( BattleCameraShakeParam.ShakePower )m.Param.ShakePower;
            i.direction  = ( BattleCameraShakeParam.ShakeDirection )m.Param.ShakeDirection;
            return i;
        }
    }
}
