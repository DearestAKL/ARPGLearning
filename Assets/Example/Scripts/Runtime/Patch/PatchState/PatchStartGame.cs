using Akari.GfCore;

namespace GameMain.Runtime
{
    public class PatchStartGame : PatchBaseState
    {
        public static int Type => (int)PatchStateType.StartGame;

        public override int StateType => Type;

        public PatchStartGame(PatchConfigurationModel configurationModel) : base(configurationModel)
        {
        }

        public override void OnEnter(AGfFsmState prevState, bool reenter)
        {
            //游戏开始
        }
    }
}