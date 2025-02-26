using Akari.GfCore;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public interface ICreateShellAnimationEventListener : IGfAnimationEventListener
    {
        void CreateShell(AnimationEventParameterShell param, GfAnimationEventCallInfo info);
        void CreateShellWarning(AnimationEventParameterShell param, GfAnimationEventCallInfo info);
    }
    
    public class CreateShellAnimationEventListener : ACharacterAnimationEventListener, ICreateShellAnimationEventListener
    {
        public GfRunTimeTypeId RttId { get; }
        
        public CreateShellAnimationEventListener(GfEntity entity) : base(entity)
        {
            RttId   = GfRunTimeTypeOf<CreateShellAnimationEventListener>.Id;
        }

        public void CreateShell(AnimationEventParameterShell param, GfAnimationEventCallInfo info)
        {
            GfFloat3 position = GetPosition(param);
            GfFloat3 direction = GetDirection(param);
            CreateShell(param.Id, position, direction);
        }
        
        public async void CreateShellWarning(AnimationEventParameterShell param, GfAnimationEventCallInfo info)
        {
            GfFloat3 basePosition = GetPosition(param);
            GfFloat3 direction = GetDirection(param);
            
            //默认 根据shell配置的碰撞范围 显示预警范围
            var shellDefinitionMessage = await PbDefinitionHelper.GetShellDefinitionMessage(param.Id);
            var collisions = shellDefinitionMessage.AttackDefinitionInfo.Collisions;
            var extent = collisions.Count > 0 ? collisions[0].Extents.ToGfFloat2() : GfFloat2.One;
            
            Accessor.Entity.Request(new CreatDamageWarningRequest(1f, basePosition.ToXZFloat2(), extent, GfQuaternion.LookRotation(direction)));
        }

        private async void CreateShell(int shellId, GfFloat3 position, GfFloat3 direction)
        {
            var shellDefinitionMessage = await PbDefinitionHelper.GetShellDefinitionMessage(shellId);
            var attackDefinitionInfoData = AttackDefinitionInfoData.CreatData(shellDefinitionMessage, shellDefinitionMessage.AttackDefinitionInfo);
            var shellDamageCauserHandler = new BattleShellDamageCauserHandler(Accessor.Entity, Accessor.Condition.TeamId,attackDefinitionInfoData);
            
            var shellEntity = BattleAdmin.Factory.Shell.CreateShell((uint)shellId, shellDefinitionMessage, shellDamageCauserHandler, position, direction);
            BattleAdmin.Factory.Shell.CreateShellEffect(Accessor.Entity.ThisHandle, shellEntity, shellDefinitionMessage.EffectId);
        }

        private GfFloat3 GetPosition(AnimationEventParameterShell param)
        {
            GfFloat3 position = GfFloat3.Zero;
            if (param.IsLockTarget)
            {
                //TODO:及时选择目标
                var targetAccessor = Accessor.Condition.Target;
                if (targetAccessor != null && targetAccessor.Entity != null)
                {
                    position = targetAccessor.Entity.Transform.Position;
                    position = GfTransformHelper.TransformPoint(position, targetAccessor.Entity.Transform.Rotation, targetAccessor.Entity.Transform.Scale, param.OffsetPosition);
                }
            }
            else
            {
                position = Accessor.Entity.Transform.Position;
                position = GfTransformHelper.TransformPoint(position, Accessor.Entity.Transform.Rotation, Accessor.Entity.Transform.Scale, param.OffsetPosition);
            }

            return position;
        }
        
        private GfFloat3 GetDirection(AnimationEventParameterShell param)
        {
            GfFloat3 direction = GfFloat3.Zero;
            if (param.IsLockTarget)
            {
                var targetAccessor = Accessor.Condition.Target;
                if (targetAccessor != null && targetAccessor.Entity != null)
                {
                    var lookRotation = GfQuaternion.LookRotation(targetAccessor.Entity.Transform.Position - Accessor.Entity.Transform.Position);
                    direction = lookRotation * GfQuaternion.Euler(param.OffsetRotation) * GfFloat3.Forward;
                }
            }
            else
            {
                direction = Accessor.Entity.Transform.Rotation * GfQuaternion.Euler(param.OffsetRotation) * GfFloat3.Forward;
            }
            
            return direction;
        }
    }
}