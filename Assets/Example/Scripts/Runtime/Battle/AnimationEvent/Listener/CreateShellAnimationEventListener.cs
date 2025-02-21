using Akari.GfCore;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public interface ICreateShellAnimationEventListener : IGfAnimationEventListener
    {
        void CreateShell(AnimationEventParameterShell param, GfAnimationEventCallInfo info);
        void CreateWarningShell(AnimationEventParameterShell param, GfAnimationEventCallInfo info);
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
            GfFloat3 basePosition = GetBasePosition(param);
            GfFloat3 direction = GetDirection(param);
            
            CreateShell(param.Id, basePosition, param.OffsetPosition, direction);
        }
        
        public async void CreateWarningShell(AnimationEventParameterShell param, GfAnimationEventCallInfo info)
        {
            GfFloat3 basePosition = GetBasePosition(param);
            GfFloat3 direction = GetDirection(param);
            
            //默认 根据shell配置的碰撞范围 显示预警范围
            var shellDefinitionMessage = await PbDefinitionHelper.GetShellDefinitionMessage(param.Id);
            var collisions = shellDefinitionMessage.AttackDefinitionInfo.Collisions;
            var extent = collisions.Count > 0 ? collisions[0].Extents.ToGfFloat2() : GfFloat2.One;
            
            Accessor.DamageWarning.CreatDamageWarning(1, basePosition.ToXZFloat2(), extent, GfQuaternion.LookRotation(direction),
                () =>
                {
                    CreateShell(param.Id, basePosition, param.OffsetPosition, direction);
                });
        }

        private async void CreateShell(int shellId, GfFloat3 basePosition, GfFloat3 offset, GfFloat3 direction)
        {
            var shellDefinitionMessage = await PbDefinitionHelper.GetShellDefinitionMessage(shellId);
            var attackDefinitionInfoData = AttackDefinitionInfoData.CreatData(shellDefinitionMessage, shellDefinitionMessage.AttackDefinitionInfo);
            var shellDamageCauserHandler = new BattleShellDamageCauserHandler(Accessor.Entity, Accessor.Condition.TeamId,attackDefinitionInfoData);
            
            var shellEntity = BattleAdmin.Factory.Shell.CreateShell((uint)shellId, shellDefinitionMessage, shellDamageCauserHandler, basePosition, offset, direction);
            BattleAdmin.Factory.Shell.CreateShellEffect(Accessor.Entity.ThisHandle, shellEntity, shellDefinitionMessage.EffectId);
        }

        private GfFloat3 GetBasePosition(AnimationEventParameterShell param)
        {
            GfFloat3 basePosition = GfFloat3.Zero;
            if (param.IsLockTarget)
            {
                //TODO:及时选择目标
                var targetAccessor = Accessor.Condition.Target;
                if (targetAccessor != null && targetAccessor.Entity != null)
                {
                    basePosition = targetAccessor.Transform.CurrentPosition;
                }
            }
            else
            {
                basePosition = Accessor.Transform.CurrentPosition;
            }

            return basePosition;
        }
        
        private GfFloat3 GetDirection(AnimationEventParameterShell param)
        {
            GfFloat3 direction = GfFloat3.Zero;
            if (param.IsLockTarget)
            {
                var targetAccessor = Accessor.Condition.Target;
                if (targetAccessor != null && targetAccessor.Entity != null)
                {
                    var lookRotation = GfQuaternion.LookRotation(targetAccessor.Transform.CurrentPosition - Accessor.Transform.CurrentPosition);
                    direction = lookRotation * GfQuaternion.Euler(param.OffsetRotation) * GfFloat3.Forward;
                }
            }
            else
            {
                direction = Accessor.Transform.Transform.Rotation * GfQuaternion.Euler(param.OffsetRotation) * GfFloat3.Forward;
            }
            
            return direction;
        }
    }
}