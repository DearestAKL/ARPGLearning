using System;

namespace GameMain.Runtime
{
    public static class EnumExtensions
    {
        public static int GetEnumCount<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum)).Length;
        }
    }
}