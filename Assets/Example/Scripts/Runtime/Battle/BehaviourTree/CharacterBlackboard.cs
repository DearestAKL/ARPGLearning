using System;
using System.Collections.Generic;
using Akari.GfCore;
using NPBehave;

namespace GameMain.Runtime
{
    public class CharacterBlackboard : Blackboard
    {
        public BattleCharacterAccessorComponent Accessor { get; }
        public GfEntity Entity { get; }

        public CharacterBlackboard(BattleCharacterAccessorComponent accessor,Clock clock) : base(clock)
        {
            Accessor       = accessor ?? throw new ArgumentNullException(nameof(accessor));
            Entity         = Accessor.Entity;
        }
    }

    public enum CharacterBlackboardKey
    {
        PlayerDistance,
        HasTarget,
        CanAttack,
    }
    
    public static class CharacterBlackboardKeyExtend
    {
        private static readonly Dictionary<int, string> CharacterBlackboardKeyDict = new Dictionary<int, string>()
        {
            {(int)CharacterBlackboardKey.PlayerDistance,"PlayerDistance"},
            {(int)CharacterBlackboardKey.HasTarget,"HasTarget"},
            {(int)CharacterBlackboardKey.CanAttack,"CanAttack"},
        };

        public static string GetString(this CharacterBlackboardKey key)
        {
            return CharacterBlackboardKeyDict[(int)key];
        }
    }
}