using Akari.GfCore;
using Akari.GfGame;
using Akari.GfUnity;
using UnityEngine;

namespace GameMain.Runtime
{
    public class ShellLauncher : MonoBehaviour
    {
        private void Start()
        {
            Init();
        }
        
        private async void Init()
        {
            var shellDefinitionMessage = await PbDefinitionHelper.GetShellDefinitionMessage(200101);
            
            GameCharacterModel gameCharacterModel = new GameCharacterModel(100, 0, 0, 10);
            
            var entity = BattleAdmin.EntityComponentSystem.Create(GfEntityTagId.TeamB, GfEntityGroupId.Gimmick, "ShellLauncher");
            var entityTransform = transform.CreateGfUnityTransform();
            
            entity.AddComponent(new GfActorComponent(entityTransform));
            entity.AddComponent(new BattleCharacterConditionComponent(BattleCharacterType.Enemy, gameCharacterModel));
            
            entity.AddComponent(new BattleCharacterViewComponent());
            entity.AddComponent(new BattleCharacterAccessorComponent(entity));

            entity.AddComponent(new BattleDamageRangeComponent());
            entity.AddComponent(new GfColliderComponent2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter>());

            
            var shellLauncherComponent = entity.AddComponent(new ShellLauncherComponent());
            shellLauncherComponent.Init(shellDefinitionMessage);
            
            var warningComponent = entity.AddComponent(new BattleCharacterWarningComponent());
            warningComponent.SetDamageCauserHandler(shellLauncherComponent.DamageCauserHandler);
            
            WorldManager.Instance.CurWorld?.AddGfEntity(entity);
        }
        
    }
    
    /// <summary>
    /// shell 发射器 用来为场景机关生成shell
    /// </summary>
    public class ShellLauncherComponent : AGfGameComponent<ShellLauncherComponent>
    {
        enum Status
        {
            None,
            Wait,//等待
            ShowRange,//显示范围
            SetWarning,//设置伤害预警
        }
        
        private GfComponentCache<BattleCharacterAccessorComponent> _accessorCache;
        private BattleCharacterAccessorComponent Accessor => Entity.GetComponent(ref _accessorCache);
        
        
        private ShellDefinitionMessage _shellDefinitionMessage;
        private BattleShellDamageCauserHandler _shellDamageCauserHandler;
        public BattleShellDamageCauserHandler DamageCauserHandler => _shellDamageCauserHandler;
        private AttackId _warningAttackId;

        private float _timer;
        private float _intervalTime = 5f;//间隔时间 发射shell
        private float _damageRangeTime = 2f;//伤害范围显示时间
        private float _damageWarningTime= 0.5f;//shell生成前的预警时间 预警区域内内会触发额外效果
        private Status _status = Status.None;
        
        public void Init(ShellDefinitionMessage shellDefinitionMessage)
        {
            _shellDefinitionMessage = shellDefinitionMessage;

            var attackDefinitionInfoData = AttackDefinitionInfoData.CreatData(_shellDefinitionMessage, _shellDefinitionMessage.AttackDefinitionInfo);
            _shellDamageCauserHandler = new BattleShellDamageCauserHandler(Accessor.Entity, Accessor.Condition.TeamId,attackDefinitionInfoData);
            
            _warningAttackId = AttackId.CreateForWarning((uint)shellDefinitionMessage.Id);

            _status = Status.Wait;
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            if (_status == Status.None)
            {
                return;
            }

            deltaTime *= Speed;
            
            _timer += deltaTime;

            if (_status == Status.Wait)
            {
                if (_timer >= _intervalTime)
                {
                    _timer = 0f;
                    
                    ShowDamageRange(_damageRangeTime);
                    _status = Status.ShowRange;
                }
            }
            else if(_status == Status.ShowRange)
            {
                if (_timer >= _damageRangeTime - _damageWarningTime)
                {
                    SetDamageWarning(true);
                    _status = Status.SetWarning;
                }
            }
            else
            {
                if (_timer >= _damageRangeTime)
                {
                    SetDamageWarning(false);
                    CreateShell();
                    _status = Status.Wait;
                }
            }
        }

        private void SetDamageWarning(bool isOn)
        {
            if (isOn)
            {
                AudioManager.Instance.PlaySound(Constant.Sound.Ding, true, Accessor.Entity.Transform.Position.ToVector3());
                BattleAdmin.Factory.Effect.CreateEffectByEntity(Entity, CommonEffectType.Flash.GetPath(), EffectGroup.OneShot, new GfFloat3(0,2F,0), GfQuaternion.Identity, GfFloat3.One, GfVfxDeleteMode.Delete, GfVfxPriority.High);
            }

            Entity.Request(new BattleRegisterColliderWarningRequest(isOn, _warningAttackId));
        }
        
        private void ShowDamageRange(float time)
        {
            var collisions = _shellDamageCauserHandler.AttackDefinition.Collisions;
            var offset = collisions.Length > 0 ? collisions[0].Offset : GfFloat2.One;
            var extents = collisions.Length > 0 ? collisions[0].Extents : GfFloat2.One;
            
            var position = Accessor.Entity.Transform.Position + offset;
            var direction = Accessor.Entity.Transform.Forward;

            Accessor.Entity.Request(new CreateDamageRangeShowRequest(_shellDefinitionMessage.Id, time, position,
                extents, GfQuaternion.LookRotation(direction)));
        }

        private void CreateShell()
        {
            var position = Accessor.Entity.Transform.Position;
            var direction = Accessor.Entity.Transform.Forward;
            
            var shellEntity = BattleAdmin.Factory.Shell.CreateShell((uint)_shellDefinitionMessage.Id, _shellDefinitionMessage, _shellDamageCauserHandler, position, direction);
            BattleAdmin.Factory.Shell.CreateShellEffect(Accessor.Entity.ThisHandle, shellEntity, _shellDefinitionMessage.EffectId);
        }
    }
}