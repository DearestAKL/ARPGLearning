using Akari.GfCore;

namespace GameMain.Runtime
{
    public sealed class GimmickDamageReceiverHandler : IBattleObjectDamageReceiverHandler
    {
        public TeamId TeamId { get; }
        
        public GimmickDamageReceiverHandler()
        {
            TeamId = TeamId.TeamB;
        }

        public void Dispose()
        {

        }
        
        public GfFloat3 GetReceiverPosition()
        {
            throw new System.NotImplementedException();
        }

        public bool CanReceiveKnockUp()
        {
            throw new System.NotImplementedException();
        }

        public float GetDefense()
        {
            throw new System.NotImplementedException();
        }

        public float GetDamageReduction()
        {
            throw new System.NotImplementedException();
        }
    }
}