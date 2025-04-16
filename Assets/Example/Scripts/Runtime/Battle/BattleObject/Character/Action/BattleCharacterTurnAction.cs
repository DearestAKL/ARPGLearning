// using Akari.GfCore;
//
// namespace GameMain.Runtime
// {
//     public sealed class BattleCharacterTurnActionData : ABattleCharacterActionData
//     {
//         public static BattleCharacterTurnActionData Create()
//         {
//             return Create(() => new BattleCharacterTurnActionData());
//         }
//
//         public override int ActionType => BattleCharacterTurnAction.ActionType;
//
//         public override bool CanTransitionFrom(ABattleCharacterAction currentAction)
//         {
//             return currentAction.Accessor.Condition.Frame.CanMove.Current;
//         }
//     }
//     
//     public class BattleCharacterTurnAction : ABattleCharacterAction
//     {
//         private BattleCharacterTurnActionData _actionData;
//         public override ABattleCharacterActionData ActionData  => _actionData;
//         
//         public const int ActionType = (int)BattleCharacterActionType.Turn;
//         
//         private static readonly string TurnLeft90AnimationName = "TurnL90";
//         private static readonly string TurnLeft180AnimationName = "TurnL180";
//         
//         private static readonly string TurnRight90AnimationName = "TurnR90";
//         private static readonly string TurnRight180AnimationName = "TurnR180";
//
//         public override void OnEnter(AGfFsmState prevAction, bool reenter)
//         {
//             base.OnEnter(prevAction, reenter);
//
//             _actionData = GetActionData<BattleCharacterTurnActionData>();
//         }
//
//         public override void OnStart()
//         {
//             base.OnStart();
//             
//             AnimationClipIndex = GetAnimationClipIndex();
//             Animation.Play(AnimationClipIndex);
//         }
//
//         public override void OnUpdate(float deltaTime)
//         {
//             base.OnUpdate(deltaTime);
//             
//             //检测是否转向到合理方位 如果已经达到则进行移动或者重新进入Idle
//             var from = Accessor.Entity.Transform.Forward.ToXZFloat2();
//             var to = Accessor.Condition.MoveDirection;
//             // 计算旋转角度
//             float angle = GfFloat2.CalcAngle(from, to); // 以Y轴为基准
//             if (GfMathf.Approximately(angle, 0F) || !Animation.IsThatPlaying(AnimationClipIndex))
//             {
//                 EndActionCheck();
//             }
//         }
//
//         private int GetAnimationClipIndex()
//         {
//             var from = Accessor.Entity.Transform.Forward.ToXZFloat2();
//             var to = Accessor.Condition.MoveDirection;
//
//             // 计算旋转角度
//             float angle = GfFloat2.CalcAngle(from, to); // 以Y轴为基准
//
//             string animationStateName = "";
//             // 判断角度范围
//             if (angle > 0)
//             {
//                 //右转
//                 if (angle > 90)
//                 {
//                     animationStateName = TurnRight180AnimationName; 
//                 }
//                 else
//                 {
//                     animationStateName = TurnRight90AnimationName; 
//                 }
//             }
//             else
//             {
//                 //左转
//                 if (angle < -90)
//                 {
//                     animationStateName = TurnLeft180AnimationName; 
//                 }
//                 else
//                 {
//                     animationStateName = TurnLeft90AnimationName; 
//                 }
//             }
//             
//             return Animation.GetClipIndex(animationStateName);
//         }
//     }
// }