using Akari.GfCore;
using Akari.GfGame;
using UnityEngine;

namespace GameMain.Runtime
{
    public enum BattleMainCameraActionType : int
    {
        Invalid = 0, 
        Basic = 1,
    }

    public struct BattleMainCameraPoseInfo
    {
        public Vector3 Position { get; set; }
        //public Quaternion Rotation { get; }

        public BattleMainCameraPoseInfo(Vector3 position)//, Quaternion rotation)
        {
            Position = position;
            //Rotation = rotation;
        }
    }

    public abstract class ABattleMainCameraAction : AGfActionState<BattleMainCameraActionContext>
    {
        private class Transition : IGfFsmStateTransition
        {
            public int JudgeOrder => 0;
            protected ABattleMainCameraAction Action { get; private set; }

            public Transition(ABattleMainCameraAction action)
            {
                Action = action;
            }

            public virtual bool CanTransition(GfFsmStateTransitionRequest request)
            {
                var actionData = request.NextUserData as ABattleMainCameraActionData;
                if (actionData == null)
                {
                    return false;
                }

                if (Action.Accessor.ActionComponent.GetNowActionId() == BattleMainCameraBasicAction.ActionType)
                {
                    return false;
                }

                return true;
            }

            public void Dispose()
            {
                Action = null;
            }
        }

        private GfComponentCache<BattleMainCameraAccessorComponent> _accessorCache;

        private int _transitionFrame;
        private int _remainingFrame;

        protected BattleMainCameraAccessorComponent Accessor => GetComponent(ref _accessorCache);

        protected ABattleMainCameraAction()
        {
            AddTransition(new Transition(this));
        }


        public override void OnAwake()
        {
        }

        public override void OnEnter(AGfFsmState prevAction, bool reenter)
        {
        }

        public override void OnStart()
        {
        }

        public override void OnExit(AGfFsmState nextAction)
        {
        }

        public override void OnDelete()
        {
        }

        public override void OnBeginUpdate(float deltaTime)
        {
        }

        public override void OnUpdate(float deltaTime)
        {
        }

        public override void OnEndUpdate(float deltaTime)
        {
        }

        // protected abstract BattleMainCameraPoseInfo GetCurrentFrameTargetPoseInfo();

        // protected void SetTransitionTime(int transitionFrame)
        // {
        //     mTransitionFrame = transitionFrame;
        //     mRemainingFrame = mTransitionFrame;
        // }

        // protected void UpdateCameraPose()
        // {
        //     mRemainingFrame = GfMathf.Max(0, mRemainingFrame - 1);
        //
        //     var interpolationRatio = mTransitionFrame > 0f ? 1f - ((float)mRemainingFrame / mTransitionFrame) : 1f;
        //
        //     if (interpolationRatio <= 0)
        //     {
        //         // 補完が0なら、今のカメラ姿勢のままでよい
        //         return;
        //     }
        //
        //     var cameraPoseInfo  = GetCurrentFrameTargetPoseInfo();
        //     var cameraTransform = Accessor.View.transform;
        //
        //     if (1f <= interpolationRatio)
        //     {
        //         // 補完が1なら、完全に目標姿勢にいれば良い
        //         cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraPoseInfo.Position, 0.1F);
        //         //cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, cameraPoseInfo.Rotation,  0.1F);
        //         return;
        //     }
        //
        //     cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraPoseInfo.Position, interpolationRatio);
        //     //cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, cameraPoseInfo.Rotation, interpolationRatio);
        // }

        // protected IBattleCharacterAccessorComponent GetTargetCharacterAccessor()
        // {
        //     var fightingCharacterAccessor = BattleAdmin.Player;
        //     
        //     if (fightingCharacterAccessor != null)
        //     {
        //         if (Accessor.TargetHandle != fightingCharacterAccessor.Entity.ThisHandle)
        //         {
        //             Accessor.SetTargetHandle(fightingCharacterAccessor.Entity.ThisHandle);
        //         }
        //     }
        //
        //     return fightingCharacterAccessor;
        // }
    }

}
