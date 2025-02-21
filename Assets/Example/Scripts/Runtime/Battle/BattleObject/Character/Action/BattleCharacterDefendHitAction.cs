// using Akari.GfCore;
//
// namespace GameMain.Runtime
// {
//     public sealed class BattleCharacterDefendHitActionData : ABattleCharacterActionData
//     {
//         public GfFloat2 DamageVector;
//         public float HorizontalVelocity;
//         public static BattleCharacterDefendHitActionData Create(GfFloat2 damageVector,float horizontalVelocity)
//         {
//             var data = Create(() => new BattleCharacterDefendHitActionData());
//             data.DamageVector = damageVector;
//             data.HorizontalVelocity = horizontalVelocity;
//
//             return data;
//         }
//
//         public override int ActionType => BattleCharacterDefendHitAction.ActionType;
//
//         public override bool CanTransitionFrom(ABattleCharacterAction currentAction)
//         {
//             return true;
//         }
//     }
//     
//     public class BattleCharacterDefendHitAction : ABattleCharacterAction
//     {
//         private BattleCharacterDefendHitActionData _actionData;
//         public override ABattleCharacterActionData ActionData => _actionData;
//         public const int ActionType = (int)BattleCharacterActionType.DefendHit;
//
//         private static readonly string DefendHitAnimationName = "DefendHit";
//
//         private float _elapsedTime;
//         private bool _hasHorizontalInitVelocity;
//         
//         public override void OnEnter(AGfFsmState prevAction, bool reenter)
//         {
//             base.OnEnter(prevAction, reenter);
//             _actionData = GetActionData<BattleCharacterDefendHitActionData>();
//             
//             
//             _hasHorizontalInitVelocity = _actionData.HorizontalVelocity > 0f;
//             _elapsedTime = 0;
//         }
//         
//         public override void OnStart()
//         {
//             base.OnStart();
//             AnimationClipIndex = GetAnimationClipIndex();
//             Animation.Play(AnimationClipIndex);
//         }
//
//         public override void OnUpdate(float deltaTime)
//         {
//             base.OnUpdate(deltaTime);
//
//             if (_hasHorizontalInitVelocity)
//             {
//                 //在地面
//                 //如果水平速度数值 > 0，则保持击退效果，水平速度计算如下
//                 //水平速度 = (水平初速度的1/3次方 - 3*时间)的三次方
//                 var horizontalVelocity = GfMathf.Pow(GfMathf.Pow(_actionData.HorizontalVelocity, 1f / 3f) - 3 * _elapsedTime, 3);
//                 
//                 if (horizontalVelocity <= 0f)
//                 {
//                     _hasHorizontalInitVelocity = false;
//                 }
//                 else
//                 {
//                     HorizontalMove(deltaTime, _actionData.DamageVector, horizontalVelocity);
//                 }
//             }
//
//             _elapsedTime += deltaTime;
//             
//             EndActionCheck();
//         }
//
//         private int GetAnimationClipIndex()
//         {
//             return Animation.GetClipIndex(DefendHitAnimationName);
//         }
//     }
// }