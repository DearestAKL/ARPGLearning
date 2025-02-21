using Akari;
using cfg;

namespace GameMain.Runtime
{
    public class UICharacterItemData : LoopScrollItem
    {
        public CharacterConfig CharacterConfig { get; }

        public UICharacterItemData(CharacterConfig characterConfig)
        {
            CharacterConfig = characterConfig;
        }
    }
}