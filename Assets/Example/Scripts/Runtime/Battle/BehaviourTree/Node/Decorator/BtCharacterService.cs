using Akari.GfCore;
using NPBehave;

namespace GameMain.Runtime
{
    public class BtCharacterService : Decorator
    {
        protected CharacterBlackboard                      CharacterBlackBoard { get; set; }
        
        protected GfEntity Entity => CharacterBlackBoard.Entity;
        protected BattleCharacterDirectorComponent Director     => Entity.GetComponent(ref _directorCache);
        private GfComponentCache<BattleCharacterDirectorComponent>  _directorCache;
        
        private float interval = -1.0f;
        private float randomVariation;
        

        public BtCharacterService(float interval, Node decoratee) : base("CharacterService", decoratee)
        {
            this.interval = interval;
            this.randomVariation = interval * 0.05f;
            this.Label = "" + (interval - randomVariation) + "..." + (interval + randomVariation) + "s";
        }

        public override void SetRoot(Root rootNode)
        {
            base.SetRoot(rootNode);
            CharacterBlackBoard = (RootNode.Blackboard as CharacterBlackboard);
        }
        
        protected override void DoStart()
        {
            if (this.interval <= 0f)
            {
                this.Clock.AddUpdateObserver(ServiceMethod);
                ServiceMethod();
            }
            else if (randomVariation <= 0f)
            {
                this.Clock.AddTimer(this.interval, -1, ServiceMethod);
                ServiceMethod();
            }
            else
            {
                InvokeServiceMethodWithRandomVariation();
            }
            Decoratee.Start();
        }

        protected override void DoStop()
        {
            Decoratee.Stop();
        }
        
        protected override void DoChildStopped(Node child, bool result)
        {
            if (this.interval <= 0f)
            {
                this.Clock.RemoveUpdateObserver(ServiceMethod);
            }
            else if (randomVariation <= 0f)
            {
                this.Clock.RemoveTimer(ServiceMethod);
            }
            else
            {
                this.Clock.RemoveTimer(InvokeServiceMethodWithRandomVariation);
            }
            Stopped(result);
        }
        
        private void InvokeServiceMethodWithRandomVariation()
        {
            ServiceMethod();
            this.Clock.AddTimer(interval, randomVariation, 0, InvokeServiceMethodWithRandomVariation);
        }

        private void ServiceMethod()
        {
            var playerDistanceKey = CharacterBlackboardKey.PlayerDistance.GetString();
            if (Director.Target != null) 
            {
                //与目标距离
                var targetDistance = GfFloat3.DistanceXZ(Director.Target.Entity.Transform.Position,CharacterBlackBoard.Accessor.Entity.Transform.Position);
                Blackboard.SetFloat(playerDistanceKey,targetDistance);
                
                if (targetDistance > 20f)
                {
                    Director.SetTarget(null);
                }
            }
            else
            {
                //无限大
                Blackboard.SetFloat(playerDistanceKey,100f);
            }

            var hasTargetKey = CharacterBlackboardKey.HasTarget.GetString();
            Blackboard.SetBool(hasTargetKey,Director.Target != null && Director.Target.IsAlive);
        }
    }
}