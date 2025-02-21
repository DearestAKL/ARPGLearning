using System.Collections.Generic;
using Akari.GfCore;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public class BattleCharacterBufferComponent : AGfGameComponent<BattleCharacterBufferComponent>
    {
        private GfComponentCache<BattleCharacterAccessorComponent> _accessorCache;
        private BattleCharacterAccessorComponent Accessor => Entity.GetComponent(ref _accessorCache);
        
        private readonly List<Buffer> _buffers = new List<Buffer>();
        private readonly List<Buffer> _removeBuffers = new List<Buffer>();
        
        public override void OnStart()
        {

        }
        
        public override void OnUpdate(float deltaTime)
        {
            if (_removeBuffers.Count > 0)
            {
                foreach (var removeBuffer in _removeBuffers)
                {
                    _buffers.Remove(removeBuffer);
                }
                _removeBuffers.Clear();
            }

            foreach (var buffer in _buffers)
            {
                buffer.Tick(deltaTime);
            }
        }

        public async void AddBuffer(int bufferId,IBattleCharacterAccessorComponent source)
        {
            var bufferDefinitionMessage = await PbDefinitionHelper.GetBufferDefinitionMessage(bufferId);
            AddBuffer(bufferDefinitionMessage, source);
        }

        private bool AddBuffer(BufferDefinitionMessage message,IBattleCharacterAccessorComponent source)
        {
            //TODO:buff添加逻辑 概率 命中 抵抗 叠加方式
            if (TryGetBuffer(message.Id,out Buffer buffer))
            {
                //已经有对应buffer了
                if ((buffer.OverlayType & BufferOverlayType.Overlay) != 0)
                {
                    buffer.ChangeOverlay(1);
                }
                
                if ((buffer.OverlayType & BufferOverlayType.Refresh) != 0)
                {
                    buffer.Refresh();
                }
            }
            else
            { 
                var newBuffer = CreateBuffer(message, source);
                if (newBuffer != null)
                {
                    _buffers.Add(newBuffer);
                }
            }

            return true;
        }

        public bool ClearBuffer(int id)
        {
            if (TryGetBuffer(id, out var buffer)) 
            {
                buffer.Clear();
                return true;
            }

            return false;
        }

        public void RemoveBuffer(Buffer buffer)
        {
            if (!_removeBuffers.Contains(buffer))
            {
                _removeBuffers.Add(buffer);
            }
        }

        public bool HasBuffer(int id)
        {
            foreach (var buffer in _buffers)
            {
                if (buffer.Id == id)
                {
                    return true;
                }
            }
            return false;
        }

        public bool TryGetBuffer(int id, out Buffer buffer)
        {
            foreach (var item in _buffers)
            {
                if (item.Id == id)
                {
                    buffer = item;
                    return true;
                }
            }

            buffer = null;
            return false;
        }

        #region pb message处理
        private Buffer CreateBuffer(BufferDefinitionMessage message,IBattleCharacterAccessorComponent source)
        {
            var bufferOverlayType = message.OverlayType.IntToEnum<BufferOverlayType>();
            var newBuffer = new Buffer(Accessor, source, message.Id, bufferOverlayType,message.OverlayLimit);

            var endConditions = CreateBufferEndConditions(newBuffer,message);
            var bufferEffects = CreateBufferEffects(newBuffer, message);
            newBuffer.Init(bufferEffects,endConditions);
            return newBuffer;
        }
        
        private IBufferEndCondition[] CreateBufferEndConditions(Buffer buffer,BufferDefinitionMessage message)
        {
            IBufferEndCondition[] conditions = new IBufferEndCondition[message.EndConditions.Count];
            for (int i = 0; i < message.EndConditions.Count; i++)
            {
                var conditionMessage = message.EndConditions[i];
                IBufferEndCondition condition = null;
                if (conditionMessage.EndType == (int)BufferEndType.AlwaysFulfilled)
                {
                    condition = new BufferEndConditionAlwaysFulfilled(buffer);
                }
                else if (conditionMessage.EndType == (int)BufferEndType.TimeOver)
                {
                    condition = new BufferEndConditionTimeOver(buffer,conditionMessage.EndValue);
                }
                else if (conditionMessage.EndType == (int)BufferEndType.Hit)
                {
                    condition = new BufferEndConditionHit(buffer);
                }
                else if (conditionMessage.EndType == (int)BufferEndType.NoTargetBuffer)
                {
                    condition = new BufferEndConditionNoTargetBuffer(buffer, conditionMessage.EndValue);
                }
                
                conditions[i] = condition;
            }
            
            return conditions;
        }

        private IBufferEffect[] CreateBufferEffects(Buffer buffer, BufferDefinitionMessage message)
        {
            IBufferEffect[] bufferEffects = new IBufferEffect[message.Effects.Count];
            for (int i = 0; i < message.Effects.Count; i++)
            {
                var effectMessage = message.Effects[i];
                IBufferEffect bufferEffect = null;
                if (effectMessage.EffectType == (int)BufferEffectType.Attribute)
                {
                    bufferEffect = AttributeBufferEffect.Create(buffer, message, effectMessage);
                }
                else if (effectMessage.EffectType == (int)BufferEffectType.ChangeCurHp)
                {
                    bufferEffect = ChangeCurHpBufferEffect.Create(buffer, message, effectMessage);
                }
                else
                {
                    //错误的类型
                    continue;
                }

                var validConditions = CreatBufferEffectValidConditions(bufferEffect, message, effectMessage);
                bufferEffect.Init(validConditions);
                bufferEffects[i] = bufferEffect;
            }

            return bufferEffects;
        }

        private IBufferEffectValidCondition[] CreatBufferEffectValidConditions(IBufferEffect bufferEffect, BufferDefinitionMessage message,BufferEffectDefinitionMessage effectMessage)
        {
            IBufferEffectValidCondition[] validConditions = new IBufferEffectValidCondition[effectMessage.ValidConditions.Count];
            
            for (int i = 0; i < effectMessage.ValidConditions.Count; i++)
            {
                var validConditionMessage = effectMessage.ValidConditions[i];
                IBufferEffectValidCondition validCondition = null;
                if (validConditionMessage.ValidType == (int)BufferEffectValidType.TimeInterval)
                {
                    validCondition = new TimeIntervalBufferEffectValidCondition(bufferEffect,validConditionMessage.TimeInterval.Interval);
                }
                else if (validConditionMessage.ValidType == (int)BufferEffectValidType.Attribute)
                {
                    var attribute = validConditionMessage.Attribute;
                    var attributeValue = PbDefinitionHelper.GetNumericalMessage(message, attribute.AttributeIndex);
                    validCondition = new AttributeBufferEffectValidCondition(bufferEffect, (AttributeType)attribute.AttributeType, attributeValue, attribute.IsLessThan);
                }
                validConditions[i] = validCondition;
            }

            return validConditions;
        }

        #endregion
    }
}