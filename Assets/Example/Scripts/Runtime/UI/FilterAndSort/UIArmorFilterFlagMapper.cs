using System.Collections.Generic;
using System.Runtime.CompilerServices;
using cfg;

namespace GameMain.Runtime
{
    public class UIArmorFilterFlagMapper : AUIFilterFlagMapper
    {
        private Dictionary<uint, ulong> _typeMap;
        private Dictionary<uint, ulong> _qualityMap;
        
        public void Initialize()
        {
            _typeMap = CreateEnumMap<ArmorType>(false);
            _qualityMap = CreateRangeMap(1, 5);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ToArmorTypeFlag(ArmorType type)
        {
            return _typeMap[(uint)type];
        }

        public ulong ToArmorTypeFlags(uint[] values)
        {
            return ToFlags(values, _typeMap);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ToArmorQualityFlag(int quality)
        {
            return _qualityMap[(uint)quality];
        }

        public ulong ToArmorQualityFlags(uint[] values)
        {
            return ToFlags(values, _qualityMap);
        }
    }
}