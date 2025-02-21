using System.Collections.Generic;
using System.Linq;

namespace GameMain.Runtime
{
    /// <summary>
    /// 被动技能
    /// 给目标添加buffer或者新的被动技能
    /// </summary>
    public interface IPassiveSkillEvent
    {
        void Tick(float deltaTime);
        void Dispose();
        void Trigger(PassiveSkillEventType eventTyp);
        PassiveSkillEventType EventType{ get; }
        int PassiveSkillId{ get; }
    }

    public sealed class PassiveSkillEvent : IPassiveSkillEvent
    {
        private IBattleCharacterAccessorComponent Accessor { get; }
        
        public PassiveSkillEventType EventType { get; }
        public int PassiveSkillId{ get; }

        private readonly IPassiveSkillProCondition[] _proConditions;
        private readonly PassiveSkillAddBufferData[] _addBuffers;

        public PassiveSkillEvent(IBattleCharacterAccessorComponent accessor,
            IPassiveSkillProCondition[] proConditions,
            PassiveSkillAddBufferData[] addBuffers,
            PassiveSkillEventType eventType, int passiveSkillId)
        {
            Accessor = accessor;
            _proConditions = proConditions;
            _addBuffers = addBuffers;
            EventType = eventType;
            PassiveSkillId = passiveSkillId;

            if (eventType == PassiveSkillEventType.OnHpChange)
            {
                Accessor.Entity.On<BattleCurHpChangeRequest>(OnHpChange);
            }
        }

        public void Trigger(PassiveSkillEventType eventType)
        {
            if (eventType == EventType)
            {
                if (CheckProConditions())
                {
                    ProConditionsTakeEffect();
                    Apply();
                }
            }
        }
        
        public void Dispose()
        {
            
        }

        public void Tick(float deltaTime)
        {
            foreach (var condition in _proConditions)
            {
                condition.Tick(deltaTime); 
            }
        }
        
        private void Apply()
        {
            foreach (var addBufferData in _addBuffers)
            {
                AddBuffer(addBufferData);
            }
        }

        private void ProConditionsTakeEffect()
        {
            foreach (var condition in _proConditions)
            {
                condition.TakeEffect();
            }
        }

        private bool CheckProConditions()
        {
            foreach (var condition in _proConditions)
            {
                if (!condition.CheckCondition())
                {
                    return false;
                }
            }
            
            return true;
        }

        private void AddBuffer(PassiveSkillAddBufferData addBufferData)
        {
            //选择目标
            var selectTargetType = addBufferData.SelectTargetType;
            List<IBattleCharacterAccessorComponent> targets = new List<IBattleCharacterAccessorComponent>();
            if (selectTargetType == SelectTargetType.Self)
            {
                targets.Add(Accessor);
            }
            else if(selectTargetType == SelectTargetType.Team)
            {
                
            }
            
            var filteredTargets = targets.Where(_=>
            {
                //筛选目标
                foreach (var filter in addBufferData.Filters)
                {
                    if (!filter.PassesFilter(_))
                    {
                        return false;
                    }
                }
                return true;
            });

            foreach (var filteredTarget in filteredTargets)
            {
                //解析被动技能时会 预解析其携带的buffer message
                // var bufferDefinitionMessage = await PbDefinitionHelper.GetBufferDefinitionMessage(addBufferData.BufferId);
                // filteredTarget.Buffer.AddBuffer(bufferDefinitionMessage, Accessor);
                filteredTarget.Buffer.AddBuffer(addBufferData.BufferId, Accessor);
            }
        }
        
        #region Request

        private void OnHpChange(in BattleCurHpChangeRequest request)
        {
            Trigger(PassiveSkillEventType.OnHpChange);
        }
        
        #endregion
    }
}