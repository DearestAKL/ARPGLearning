using GameMain.Runtime;

namespace Ryu.InGame.Unity
{
    public sealed class BattleUnityFactory : IBattleFactory
    {
        public IBattleEffectFactory         Effect         { get; private set; }
        public IBattleCharacterFactory         Character         { get; private set; }
        public IBattleShellFactory Shell { get; private set;}


        public BattleUnityFactory()
        {
            Shell             = new BattleUnityShellFactory();
            Effect            = new BattleUnityEffectFactory();
            Character         = new BattleUnityCharacterFactory();
        }

        public void Dispose()
        {
            if (Shell != null)
            {
                Shell.Dispose();
                Shell = null;
            }

            if (Effect != null)
            {
                Effect.Dispose();
                Effect = null;
            }
            
            if (Character != null)
            {
                Character.Dispose();
                Character = null;
            }
        }
    }
}
