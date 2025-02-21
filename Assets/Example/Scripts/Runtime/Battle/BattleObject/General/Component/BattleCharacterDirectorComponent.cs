using Akari.GfCore;
using Akari.GfGame;
using NPBehave;

namespace GameMain.Runtime
{
    public class BattleCharacterDirectorComponent : AGfGameComponent<BattleCharacterDirectorComponent>
    {
        private GfComponentCache<BattleCharacterAccessorComponent> _accessorCache;

        private BattleCharacterAccessorComponent Accessor => Entity.GetComponent(ref _accessorCache);

        private GfBtData _gfBtData;
        
        private Root _behaviorTree;
        private Blackboard _ownBlackboard;
        private Clock _clock;

        private float _curDeltaTime;

        public IBattleCharacterAccessorComponent Target { get; private set; }

        public BattleCharacterDirectorComponent(GfBtData gfBtData)
        {
            _gfBtData = gfBtData;
        }

        public void SetTarget(IBattleCharacterAccessorComponent target)
        {
            Target = target;
            Accessor.Condition.SetTargetAccessor(Target);
        }

        public override void OnStart()
        {
            base.OnStart();
            _clock = new Clock();
            _ownBlackboard = new CharacterBlackboard(Accessor, _clock);
            
            _behaviorTree = CreateBehaviourTree();
            _behaviorTree.Start();

#if UNITY_EDITOR
            var npDebugger = Entity.GetComponent<BattleCharacterViewComponent>().UnityView?.gameObject.GetOrAddComponent<Debugger>();
            npDebugger.BehaviorTree = _behaviorTree;
#endif
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            _curDeltaTime = deltaTime;
            _clock.Update(deltaTime);
        }

        private Root CreateBehaviourTree()
        {
            for (int i = 0; i < _gfBtData.PropertyDatas.Length; i++)
            {
                var propertyData = _gfBtData.PropertyDatas[i];
                _ownBlackboard.Set(propertyData.Key, propertyData);
            }
            return _gfBtData.GetStartNodeData().CreateRoot(_ownBlackboard, _clock);
        }
    }
}