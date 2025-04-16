namespace GameMain.Runtime
{
    public static class GfPbTypeId
    {
        public const int AnimationEvent = 0;
        public const int BehaviourTree = 512;
        
        public const int SetProperty = 100;
    }
    
    public static class RyPbTypes
    {
        // AnimationEventParameter
        public const int AnimationEventParameterNull = GfPbTypeId.AnimationEvent + 1;
        public const int AnimationEventParameterUintId = GfPbTypeId.AnimationEvent + 2;
        public const int AnimationEventParameterString = GfPbTypeId.AnimationEvent + 3;
        public const int AnimationEventParameterShell = GfPbTypeId.AnimationEvent + 5;
        public const int AnimationEventParameterEffectPlay = GfPbTypeId.AnimationEvent + 6;
        public const int AnimationEventParameterIntValue = GfPbTypeId.AnimationEvent + 7;
        
        
        public const int BtPropertyInt = GfPbTypeId.BehaviourTree + 1;
        public const int BtPropertyString = GfPbTypeId.BehaviourTree + 2;
        public const int BtPropertyFloat = GfPbTypeId.BehaviourTree + 3;
        public const int BtPropertyBool = GfPbTypeId.BehaviourTree + 4;
        
        public const int BtNodeSetPropertyInt        = BtPropertyInt + GfPbTypeId.SetProperty;
        public const int BtNodeSetPropertyString     = BtPropertyString + GfPbTypeId.SetProperty;
        public const int BtNodeSetPropertyFloat     = BtPropertyFloat + GfPbTypeId.SetProperty;
        public const int BtNodeSetPropertyBool     = BtPropertyBool + GfPbTypeId.SetProperty;

        public const int BtNodeStart           = GfPbTypeId.BehaviourTree + 200;
        public const int BtNodeSelector        = GfPbTypeId.BehaviourTree + 201;
        public const int BtNodeSequencer       = GfPbTypeId.BehaviourTree + 202;
        public const int BtNodeParallel        = GfPbTypeId.BehaviourTree + 203;

        public const int BtNodeWaitAction = GfPbTypeId.BehaviourTree + 301;
        public const int BtNodeDebugAction = GfPbTypeId.BehaviourTree + 302;
        
        public const int BtNodeCharacterPlayAnimationStateAction = GfPbTypeId.BehaviourTree + 305;
        public const int BtNodeCharacterPatrolAction = GfPbTypeId.BehaviourTree + 307;
        public const int BtNodeCharacterFollowAction = GfPbTypeId.BehaviourTree + 308;
        public const int BtNodeCharacterChaseAction = GfPbTypeId.BehaviourTree + 309;
        public const int BtNodeCharacterConfrontAction = GfPbTypeId.BehaviourTree + 310;
        
        public const int BtNodeCharacterService = GfPbTypeId.BehaviourTree + 402;
        public const int BtNodeBoolCondition = GfPbTypeId.BehaviourTree + 403;
        public const int BtNodeFloatCondition = GfPbTypeId.BehaviourTree + 404;
        
    }
}