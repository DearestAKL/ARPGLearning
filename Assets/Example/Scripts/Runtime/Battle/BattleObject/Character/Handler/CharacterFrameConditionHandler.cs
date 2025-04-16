using System.Runtime.CompilerServices;
using Akari.GfCore;

namespace GameMain.Runtime
{
    /// <summary>
    /// 每个帧具有不同值的类
    /// 主要从AnimationEvent设置
    /// </summary>
    public sealed class CharacterFrameConditionHandler
    {
        public class CountDownFrame
        {
            private int _current = 0;

            public bool IsEnable => _current > 0;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void OnUpdate()
            {
                if (_current > 0)
                {
                    _current--;
                }
            }

            public void SetFrame(int frame)
            {
                _current = GfMathf.Max(_current, frame);
            }

            public void ForceSetFrame(int frame)
            {
                _current = frame;
            }

            public void Reset()
            {
                _current = 0;
            }
        }

        public BufferHandlerSingle<bool> CanMove { get; } = new BufferHandlerSingle<bool>();
        public BufferHandlerSingle<bool> CanAttack { get; } = new BufferHandlerSingle<bool>();
        public BufferHandlerSingle<bool> CanDash { get; } = new BufferHandlerSingle<bool>();

        public BufferHandlerSingle<bool> IsDamageImmunity { get; } = new BufferHandlerSingle<bool>();//无敌
        public BufferHandlerSingle<bool> IsSuperArmor { get; } = new BufferHandlerSingle<bool>();//霸体
        public BufferHandlerSingle<bool> IsDodge { get; } = new BufferHandlerSingle<bool>();//躲闪
        public BufferHandlerSingle<bool> IsJump { get; } = new BufferHandlerSingle<bool>();//跳跃中

        public void OnUpdate() 
        {
            CanMove.OnUpdate();
            CanAttack.OnUpdate();
            CanDash.OnUpdate();
            IsDamageImmunity.OnUpdate();
            IsSuperArmor.OnUpdate();
            IsDodge.OnUpdate();
            IsJump.OnUpdate();
        }
    }
}
