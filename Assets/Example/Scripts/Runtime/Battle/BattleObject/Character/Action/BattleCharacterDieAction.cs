using Akari.GfCore;
using Akari.GfGame;
using Cysharp.Threading.Tasks;

namespace GameMain.Runtime
{
    public sealed class BattleCharacterActionDieData : ABattleCharacterActionData
    {
        public GfFloat2 DamageVector;

        public static BattleCharacterActionDieData Create(GfFloat2 damageVector)
        {
            var data = Create(() => new BattleCharacterActionDieData());
            data.DamageVector = damageVector;

            return data;
        }

        public override int ActionType => BattleCharacterDieAction.ActionType;

        public override bool CanTransitionFrom(ABattleCharacterAction currentAction)
        {
            return true;
        }
    }


    public class BattleCharacterDieAction : ABattleCharacterAction
    {
        private BattleCharacterActionDieData _actionData;
        public override ABattleCharacterActionData ActionData => _actionData;
        public const int ActionType = (int)BattleCharacterActionType.Die;

        private static readonly string DieAnimationName = "Die";


        public override void OnEnter(AGfFsmState prevAction, bool reenter)
        {
            base.OnEnter(prevAction, reenter);
            _actionData = GetActionData<BattleCharacterActionDieData>();
            
            Rotate(_actionData.DamageVector * -1);
        }
        
        public override void OnStart()
        {
            base.OnStart();
            AnimationClipIndex = GetAnimationClipIndex();
            Animation.Play(AnimationClipIndex);
        }

        public override async void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            if (!Animation.IsThatPlaying(AnimationClipIndex))
            {
                Entity.SetActive(false);
                
                await BattleAdmin.Factory.Effect.CreateEffectByEntity(Entity,
                    CommonEffectType.Die.GetPath(), EffectGroup.OneShot, GfFloat3.Zero,
                    GfQuaternion.Identity, GfFloat3.One, GfVfxDeleteMode.Delete, GfVfxPriority.High);

                EventManager.Instance.BattleEvent.OnCharacterDieEvent.Invoke(Entity);
                Entity.Delete();
            }
        }

        private int GetAnimationClipIndex()
        {
            return Animation.GetClipIndex(DieAnimationName);
        }
    }
}