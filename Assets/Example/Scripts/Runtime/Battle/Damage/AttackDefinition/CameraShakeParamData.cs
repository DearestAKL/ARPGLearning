namespace GameMain.Runtime
{
    public sealed class CameraShakeParamData
    {
        public BattleCameraShakeParam.ShakeDirection Direction { get; }
        public BattleCameraShakeParam.ShakePower Power   { get; }

        public CameraShakeParamData(BattleCameraShakeParam.ShakePower power, BattleCameraShakeParam.ShakeDirection direction )
        {
            Power = power;
            Direction = direction;
        }
        
        public CameraShakeParamData(CameraShakeParameterMessage message)
        {
            if (message == null)
            {
                Power = BattleCameraShakeParam.ShakePower.NONE;
                Direction = BattleCameraShakeParam.ShakeDirection.RANDOM;
            }
            else
            {
                Power     = (BattleCameraShakeParam.ShakePower)message.ShakePower;
                Direction = (BattleCameraShakeParam.ShakeDirection)message.ShakeDirection;
            }
        }
        
        

        public override string ToString()
        {
            return $"{nameof(Power)}: {Power}, {nameof(Direction)}: {Direction}";
        }
    }
}