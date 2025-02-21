using System;

namespace GameMain.Runtime
{
    public readonly struct BattleColliderGroupId : IEquatable<BattleColliderGroupId>
    {
        private readonly bool     IsDefend;
        private readonly AttackId AttackId;

        public BattleColliderGroupId(in AttackId attackId, bool isDefend = false)
        {
            IsDefend = isDefend;
            AttackId = attackId;
        }

        public bool Equals(BattleColliderGroupId other)
        {
            return IsDefend == other.IsDefend && AttackId.Equals(other.AttackId);
        }

        public override bool Equals(object obj)
        {
            return obj is BattleColliderGroupId other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (IsDefend.GetHashCode() * 397) ^ AttackId.GetHashCode();
            }
        }
    }
}