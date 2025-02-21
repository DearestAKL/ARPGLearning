using System;

namespace GameMain.Runtime
{
    public readonly struct AttackId : IEquatable<AttackId>
    {
        public static readonly AttackId Invalid = new AttackId();
        
        public readonly AttackType AttackType;
        
        public readonly uint GeneralId;

        public BattleColliderGroupId ToColliderGroupId()
        {
            return new BattleColliderGroupId(this, false);
        }

        public static AttackId CreateForCharacterAttack(uint attackNumber)
        {
            return new AttackId(AttackType.CharacterAttack, attackNumber);
        }

        public static AttackId CreateForShell(uint shellId)
        {
            return new AttackId(AttackType.Shell, shellId);
        }
        
        public AttackId(AttackType attackType, uint generalId)
        {
            AttackType         = attackType;
            GeneralId          = generalId;
        }

        public bool Equals(AttackId key)
        {
            if (AttackType != key.AttackType)
            {
                return false;
            }

            if (GeneralId != key.GeneralId)
            {
                return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is AttackId info && Equals(info);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)AttackType;
                hashCode  = (hashCode * 397) ^ (int)GeneralId;
                // hashCode  = (hashCode * 397) ^ (int)AttackNumber;
                // hashCode  = (hashCode * 397) ^ SecondaryGeneralId;
                return hashCode;
            }
        }

        public static bool operator ==(in AttackId a, in AttackId b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(in AttackId a, in AttackId b)
        {
            return !a.Equals(b);
        }

        public override string ToString()
        {
            return
                $"{nameof(AttackType)}: {AttackType}, {nameof(GeneralId)}: {GeneralId}";
                //$"{nameof(AttackType)}: {AttackType}, {nameof(GeneralId)}: {GeneralId}, {nameof(SecondaryGeneralId)}: {SecondaryGeneralId}, {nameof(AttackNumber)}: {AttackNumber}";
        }
    }
}