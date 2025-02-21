using System;
using System.Collections.Generic;
using Akari.GfCore;

namespace GameMain.Runtime
{
    public abstract class AUIFilterFlagMapper
    {
        public const  ulong AllFlags = ~0ul;
        private const int   ByteBits = 8;

        protected static ulong ToFlags(uint[] values, Dictionary<uint, ulong> map)
        {
            if (values == null || values.Length == 0)
            {
                return AllFlags;
            }

            var flags = 0ul;
            foreach (var value in values)
            {
                flags |= map[value];
            }

            return flags;
        }

        protected static Dictionary<uint, ulong> CreateEnumMap<T>(bool ignoreDefault)
            where T : unmanaged, Enum
        {
            var map = new Dictionary<uint, ulong>();

            var shift = 0;
            foreach (T enumValue in Enum.GetValues(typeof(T)))
            {
                var value = (uint)enumValue.EnumToInt64();
                if (ignoreDefault && value == default)
                {
                    continue;
                }
                
                GfAssert.ASSERT(shift < sizeof(ulong) * ByteBits, $"flag is too large (shift: {shift})");

                ulong flag = 1ul << shift;
                map.Add(value, flag);
                ++shift;
            }

            return map;
        }

        protected static Dictionary<uint, ulong> CreateRangeMap(uint min, uint max)
        {
            var map = new Dictionary<uint, ulong>();

            var shift = 0;
            for (var value = min; value <= max; ++value)
            {
                GfAssert.ASSERT(shift < sizeof(ulong) * ByteBits, $"flag is too large (shift: {shift})");

                ulong flag = 1ul << shift;
                map.Add(value, flag);
                ++shift;
            }

            return map;
        }
    }
}