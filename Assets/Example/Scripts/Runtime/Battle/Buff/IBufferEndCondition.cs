using Akari.GfCore;

namespace GameMain.Runtime
{
    public interface IBufferEndCondition
    {
        public Buffer Buffer { get; }
        bool CheckCondition();
        void Tick(float deltaTime);
        void Reset();
    }

    public class ABufferEndCondition : IBufferEndCondition
    {
        public Buffer Buffer { get; }
        public virtual bool CheckCondition() { return false; }
        public virtual void Tick(float deltaTime) { }
        public virtual void Reset() { }
        
        protected ABufferEndCondition(Buffer buffer)
        {
            Buffer = buffer;
        }
    }

    public sealed class BufferEndConditionAlwaysFulfilled : ABufferEndCondition
    {
        public override bool CheckCondition()
        {
            return true;
        }

        public BufferEndConditionAlwaysFulfilled(Buffer buffer) : base(buffer)
        {
        }
    }
    
    public sealed class BufferEndConditionTimeOver : ABufferEndCondition
    {
        private readonly float _durationTime;
        private float _elapsedTime;

        public BufferEndConditionTimeOver(Buffer buffer,float durationTime): base(buffer)
        {
            _durationTime = durationTime;
        }

        public override void Tick(float deltaTime)
        {
            _elapsedTime += deltaTime;
        }

        public override bool CheckCondition()
        {
            return _elapsedTime >= _durationTime;
        }
        
        public override void Reset()
        {
            _elapsedTime = 0;
        }
    }

    public sealed class BufferEndConditionHit : ABufferEndCondition
    {
        private bool _isActive;

        public BufferEndConditionHit(Buffer buffer): base(buffer)
        {
            Buffer.Accessor.Entity.On<BattleDidCauseDamageRequest>(OnHit);
        }

        public override bool CheckCondition()
        {
            return _isActive;
        }

        public override void Reset()
        {
            _isActive = false;
        }
        
        private void OnHit(in BattleDidCauseDamageRequest request)
        {
            if (request.DamageResult.AttackCategoryType == AttackCategoryType.Damage)
            {
                _isActive = true;
            }
        }
    }
    
    public sealed class BufferEndConditionNoTargetBuffer : ABufferEndCondition
    {
        private readonly int _bufferId;

        public BufferEndConditionNoTargetBuffer(Buffer buffer,int bufferId): base(buffer)
        {
            _bufferId = bufferId;
        }

        public override bool CheckCondition()
        {
            return ! Buffer.Accessor.Buffer.HasBuffer(_bufferId);
        }
    }
}