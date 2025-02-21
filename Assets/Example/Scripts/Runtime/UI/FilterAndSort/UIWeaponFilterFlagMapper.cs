using System.Collections.Generic;
using System.Runtime.CompilerServices;
using cfg;

namespace GameMain.Runtime
{
    public class UIWeaponFilterFlagMapper : AUIFilterFlagMapper
    {
        private Dictionary<uint, ulong> _typeMap;
        private Dictionary<uint, ulong> _qualityMap;
        
        public void Initialize()
        {
            _typeMap = CreateEnumMap<WeaponType>(false);
            _qualityMap = CreateRangeMap(1, 5);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ToWeaponTypeFlag(WeaponType type)
        {
            return _typeMap[(uint)type];
        }

        public ulong ToWeaponTypeFlags(uint[] values)
        {
            return ToFlags(values, _typeMap);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ToWeaponQualityFlag(int quality)
        {
            return _qualityMap[(uint)quality];
        }

        public ulong ToWeaponQualityFlags(uint[] values)
        {
            return ToFlags(values, _qualityMap);
        }
    }
}