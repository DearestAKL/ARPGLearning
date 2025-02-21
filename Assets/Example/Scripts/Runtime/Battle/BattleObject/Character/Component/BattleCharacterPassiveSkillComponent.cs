using System.Collections.Generic;
using Akari.GfCore;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public sealed class BattleCharacterPassiveSkillComponent : AGfGameComponent<BattleCharacterPassiveSkillComponent>
    {
        private GfComponentCache<BattleCharacterAccessorComponent> _accessorCache;
        private BattleCharacterAccessorComponent Accessor => Entity.GetComponent(ref _accessorCache);
        
        private readonly List<int> _passiveSkillIds = new List<int>();
        private readonly List<IPassiveSkillEvent> _passiveSkillEvents = new List<IPassiveSkillEvent>();
        
        // ------------------------------
        public override void OnUpdate(float deltaTime)
        {
            foreach (var passiveSkillEvent in _passiveSkillEvents)
            {
                passiveSkillEvent.Tick(deltaTime);
            }
        }

        public void AddPassiveSkills(List<int> ids)
        {
            if (ids == null || ids.Count <= 0)
            {
                return;
            }

            for (int i = 0; i < ids.Count; i++)
            {
                AddPassiveSkill(ids[i]);
            }
        }
        
        public async void AddPassiveSkill(int id)
        {
            if (_passiveSkillIds.Contains(id)) { return; }
            _passiveSkillIds.Add(id);
            var message = await PbDefinitionHelper.GetPassiveSkillDefinitionMessage(id);
            AddPassiveSkill(message);
        }

        public void AddPassiveSkill(PassiveSkillDefinitionMessage message)
        {
            foreach (var eventMessage in message.Events)
            {
                var proConditions = CreateProCondition(message,eventMessage);
                var addBuffers = CreatAddBufferData(message,eventMessage);
                PassiveSkillEventType eventType = (PassiveSkillEventType)eventMessage.EventType;
                var  passiveSkillEvent = new PassiveSkillEvent(Accessor, proConditions, addBuffers, eventType, message.Id);
                _passiveSkillEvents.Add(passiveSkillEvent);
                
                passiveSkillEvent.Trigger(PassiveSkillEventType.OnCreate);
            }
        }
        
        public void RemovePassiveSkill(int id)
        {
            if (_passiveSkillIds.Remove(id))
            {
                _passiveSkillEvents.RemoveAll(passiveSkillEvent =>
                {
                    if (passiveSkillEvent.PassiveSkillId == id)
                    {
                        passiveSkillEvent.Dispose();
                        return true;
                    }
                    return false;
                });
            }
        }

        public void TriggerEvents(PassiveSkillEventType eventTyp)
        {
            foreach (var passiveSkillEvent in _passiveSkillEvents)
            {
                passiveSkillEvent.Trigger(eventTyp);
            }
        }

        public void TriggerEvents(int passiveSkillId, PassiveSkillEventType eventTyp)
        {
            foreach (var passiveSkillEvent in _passiveSkillEvents)
            {
                if (passiveSkillEvent.PassiveSkillId == passiveSkillId)
                {
                    passiveSkillEvent.Trigger(eventTyp);
                    return;
                }
            } 
        }

        #region pb message处理
        private IPassiveSkillProCondition[] CreateProCondition(PassiveSkillDefinitionMessage message, PassiveSkillDefinitionEventMessage eventMessage)
        {
            IPassiveSkillProCondition[] proConditions = new IPassiveSkillProCondition[eventMessage.Conditions.Count];
            for (int i = 0; i < eventMessage.Conditions.Count; i++)
            {
                var conditionMessage = eventMessage.Conditions[i];
                IPassiveSkillProCondition condition = null;
                if (conditionMessage.ProConditionType == (int)PassiveSkillProConditionType.TimeInterval)
                {
                    condition = new TimeIntervalPassiveSkillProCondition(conditionMessage.TimeInterval.Interval);
                }
                else if (conditionMessage.ProConditionType == (int)PassiveSkillProConditionType.Attribute)
                {
                    var attribute = conditionMessage.Attribute;
                    var attributeValue = PbDefinitionHelper.GetNumericalMessage(message, attribute.AttributeIndex);
                    condition = new AttributePassiveSkillProCondition(Accessor, (AttributeType)attribute.AttributeType, attributeValue, attribute.IsLessThan);
                }
                else if (conditionMessage.ProConditionType == (int)PassiveSkillProConditionType.HasBuffer)
                {
                    condition = new HasBufferProCondition(Accessor, conditionMessage.HasBuffer.BufferId);
                }

                proConditions[i] = condition;
            }

            return proConditions;
        }

        private PassiveSkillAddBufferData[] CreatAddBufferData(PassiveSkillDefinitionMessage message, PassiveSkillDefinitionEventMessage eventMessage)
        {
            PassiveSkillAddBufferData[] addBuffers = new PassiveSkillAddBufferData[eventMessage.AddBuffers.Count];
            for (int i = 0; i < eventMessage.AddBuffers.Count; i++)
            {
                var addBufferMessage = eventMessage.AddBuffers[i];
                var filters = CreatBufferSelectTargetFilters(message,addBufferMessage);
                addBuffers[i] = new PassiveSkillAddBufferData(addBufferMessage.BuffId,
                    (SelectTargetType)addBufferMessage.SelectTargetType, filters);
            }

            return addBuffers;
        }

        private ISelectTargetFilter[] CreatBufferSelectTargetFilters(PassiveSkillDefinitionMessage message,
            PassiveSkillAddBufferMessage addBufferMessage)
        {
            var filters = new ISelectTargetFilter[addBufferMessage.Filters.Count];
            for (int i = 0; i < addBufferMessage.Filters.Count; i++)
            {
                var filterMessage = addBufferMessage.Filters[i];
                if (filterMessage.FilterType == (int)SelectTargetFilterType.Attribute)
                {
                    var attribute = filterMessage.Attribute;
                    var attributeValue = PbDefinitionHelper.GetNumericalMessage(message, attribute.AttributeIndex);
                    filters[i] = new AttributeFilter((AttributeType)attribute.AttributeType, attributeValue, attribute.IsLessThan);
                }
            }

            return filters;
        }
        #endregion
    }
}