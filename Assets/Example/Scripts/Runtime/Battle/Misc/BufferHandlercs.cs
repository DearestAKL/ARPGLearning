using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Akari.GfGame;

namespace GameMain.Runtime
{
    /// <summary>
    /// 在一个Animation播放过程中，Handler前提是只在一个AnimationEventSubTrack中进行更改
    /// </summary>
    public class BufferHandlerSingle<T>
        where T : struct
    {
        private bool _isBufferUpdated;
        private T _buffered;
        private T _defaultValue;
        public T Current;
        public T DEBUG_Buffered => _buffered;    
        public T DEBUG_Default => _defaultValue; 

        public BufferHandlerSingle()
        {
        }
        public BufferHandlerSingle(T defaultValue)
        {
            _defaultValue = defaultValue;
            _buffered = _defaultValue;
            Current = _defaultValue;
        }

        public void SetBuffer(T value, bool isOn)
        {
            if (isOn)
            {
                _buffered = value;
            }
            else
            {
                _buffered = _defaultValue;
            }

            _isBufferUpdated = true;
        }

        public void SetBuffer(T value)
        {
            _buffered = value;
            _isBufferUpdated = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnUpdate()
        {
            if (_isBufferUpdated)
            {
                Current = _buffered;
                _buffered = _defaultValue;
                _isBufferUpdated = false;
            }
        }
    }

    /// <summary>
    /// 在一个Animation播放过程中，Handler支持在多个AnimationEventSubTrack中进行更改
    /// 使用Int总和的类型
    /// </summary>
    public class BufferHandlerIntSum
    {
        private bool _isBufferUpdated;
        public int CurrentTotal { get; private set; }

        private Dictionary<GfAnimationEventSubTrackHandle, int> _dict = new Dictionary<GfAnimationEventSubTrackHandle, int>();

        public void SetBuffer(int value, GfAnimationEventSubTrackHandle handle, bool isOn)
        {
            if (isOn)
            {
                _dict[handle] = value;
            }
            else
            {
                _dict[handle] = default;
            }

            _isBufferUpdated = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnUpdate()
        {
            if (_isBufferUpdated)
            {
                CurrentTotal = 0;

                foreach (var dictValue in _dict.Values)
                {
                    CurrentTotal += dictValue;
                }

                _isBufferUpdated = false;
            }
        }
    }
}

