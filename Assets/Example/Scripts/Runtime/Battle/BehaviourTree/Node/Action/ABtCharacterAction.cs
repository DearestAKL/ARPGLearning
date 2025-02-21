using System;
using Akari.GfCore;
using Akari.GfGame;
using NPBehave;

namespace GameMain.Runtime
{
    public abstract class ABtCharacterAction : Task
    {
        protected CharacterBlackboard                      CharacterBlackBoard { get; private set; }
        protected BattleCharacterAccessorComponent Accessor     => CharacterBlackBoard.Accessor;
        protected GfEntity Entity => CharacterBlackBoard.Entity;
        
        protected BattleCharacterDirectorComponent Director     => Entity.GetComponent(ref _directorCache);
        private GfComponentCache<BattleCharacterDirectorComponent>  _directorCache;
        
        protected GfRequestResult RequestResult { get; private set; }
        
        protected ABtCharacterAction(string name) : base(name)
        {
            RequestResult = new GfRequestResult();
        }

        public override void SetRoot(Root rootNode)
        {
            base.SetRoot(rootNode);
            CharacterBlackBoard = (RootNode.Blackboard as CharacterBlackboard);
        }
        
        protected void SendRequest(ABattleCharacterActionData actionData)
        {
            if (actionData == null)
            {
                throw new ArgumentNullException(nameof(actionData));
            }

            RequestResult.Clear();
            Entity.Request(new GfChangeActionRequest<BattleObjectActionComponent>(actionData.ActionType, actionData, actionData.Priority, RequestResult));
        }
    }
    
    //闲置 巡逻 ====(发现目标)====>
    //追击 对峙 攻击
    
    
    //Activate主动计划 自由态时调用
    //Interrupt变招计划 不只是读指令 由于玩家角色的特定行为，中断原有计划转为执行其他动作的计划
    //Kengeki交锋计划 最大特点 根据自身或玩家角色的防御情况进行后续动作计划 => 特殊boss可能需要
}