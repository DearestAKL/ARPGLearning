using Akari.GfCore;

namespace GameMain.Runtime
{
    //基础信息 id，叠加层数，叠加类型，叠加上限，
    //拥有者，来源，结束条件或者是层数减少条件
    //记录多个Effect
    public class Buffer
    {
        public int Id { get; }
        public int Overlay { get; set; }
        public int OverlayLimit { get; set; }
        public BufferOverlayType OverlayType { get; }

        public IBattleCharacterAccessorComponent Accessor { get; }
        public IBattleCharacterAccessorComponent Source { get;}
        
        private IBufferEndCondition[] _endConditions;
        
        private IBufferEffect[] _bufferEffects;

        private bool _isActive;

        public Buffer(IBattleCharacterAccessorComponent accessor,
             IBattleCharacterAccessorComponent source,
             int id, BufferOverlayType overlayType,int overlayLimit)
        {
             Accessor = accessor;
             Source = source;
             Id = id;
             OverlayType = overlayType;
             OverlayLimit = overlayLimit;
             
             Overlay = 1;
        }
        
        
        public void Init(IBufferEffect[] bufferEffects,IBufferEndCondition[] endConditions)
        {
            _bufferEffects = bufferEffects;
            _endConditions = endConditions;
            _isActive = true;
            //Begin
            TriggerEffects(BufferEffectTriggerType.OnBegin);
        }

        public void Tick(float deltaTime)
        {
            if (!_isActive)
            {
                return;
            }

            TickConditions(deltaTime);

            TickEffects(deltaTime);

            if (CheckEndConditions())
            {
                //End
                TriggerEffects(BufferEffectTriggerType.OnEnd);
                
                //TODO:区分上面两种情况
                if ((OverlayType & BufferOverlayType.Cost) != 0 && Overlay > 1)
                {
                    ChangeOverlay(-1);
                    ResetEndConditions();
                    return;
                }
                
                _isActive = false;
                Remove();
            }
        }
        
        public void ChangeOverlay(int changeValue)
        {
            var newOverlay = GfMathf.Min(Overlay + changeValue, OverlayLimit);
            if (Overlay != newOverlay)
            {
                foreach (var bufferEffect in _bufferEffects)
                {
                    bufferEffect.ChangeOverlay();
                } 
            }
        }

        public void Clear()
        {
            
        }
        
        public void Refresh()
        {
            //重置结束条件
            ResetEndConditions();
        }

        private void Remove()
        {
            foreach (var bufferEffect in _bufferEffects)
            {
                bufferEffect.Remove();
            }

            Accessor.Buffer.RemoveBuffer(this);
        }

        private void TriggerEffects(BufferEffectTriggerType triggerType)
        {
            foreach (var bufferEffect in _bufferEffects)
            {
                bufferEffect.Trigger(triggerType);
            } 
        }

        private void TickEffects(float deltaTime)
        {
            foreach (var bufferEffect in _bufferEffects)
            {
                bufferEffect.Tick(deltaTime); 
            }
        }
        
        private void TickConditions(float deltaTime)
        {
            foreach (var condition in _endConditions)
            {
                condition.Tick(deltaTime); 
            }
        }
        
        private void ResetEndConditions()
        {
            foreach (var endCondition in _endConditions)
            {
                endCondition.Reset();
            }
        }
        
        private bool CheckEndConditions()
        {
            if (_endConditions.Length == 0)
            {
                //特殊处理 没有配置结束条件默认就是不结束
                return false;
            }
            
            foreach (var endCondition in _endConditions)
            {
                if (!endCondition.CheckCondition())
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}