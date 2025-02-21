using System.Collections.Generic;
using Akari.GfCore;

namespace GameMain.Runtime
{
    //同步游戏数据实时更新 例如武器升级后 更新该武器等级和经验
    public class UserDataManager : GfSingleton<UserDataManager>
    {
        private const string UserDataFileName = "UserData.json";
        private string UserDataFilePath => UserDataFileName.AddPersistentDataPath();
        private ISerializer _serializer;
        private UserData _userData;

        //按理来说访问器Accessor应该是只支持读取，需要另外一个类来控制数据的更新，Accessor可以取其中的数据，
        //这里为了便捷先合二为一，之后再进行功能分割与优化
        public UserPlayerAccessor Player;
        public UserBattleAccessor Battle;
        public UserCharacterAccessor Character;
        public UserItemAccessor Item;
        public UserWeaponAccessor Weapon;
        public UserArmorAccessor Armor;
        
        protected override void OnCreated()
        {
            _serializer = new JsonSerializer();
            _userData = _serializer.Load<UserData>(UserDataFilePath) ?? new UserData();
            
            Player = new UserPlayerAccessor(_userData);
            Battle = new UserBattleAccessor(_userData);
            Character = new UserCharacterAccessor(_userData);
            Item = new UserItemAccessor(_userData);
            Weapon = new UserWeaponAccessor(_userData);
            Armor = new UserArmorAccessor(_userData);
        }

        protected override void OnDisposed()
        {
            Save();
            base.OnDisposed();
        }

        public void Save()
        {
            _userData.Player = Player.UserPlayer;
            _userData.Roguelike = Battle.UserRoguelike;
            
            _userData.Characters = Character.GetList();
            _userData.Items = Item.GetList();
            _userData.Weapons = Weapon.GetList();
            _userData.Armors = Armor.GetList();
            _serializer.Save(UserDataFilePath, _userData);
        }
    }

    public class UserData
    {
        public UserPlayer Player;
        public UserRoguelike Roguelike;
        public List<UserCharacter> Characters;
        public List<UserItem> Items;
        public List<UserWeapon> Weapons;
        public List<UserArmor> Armors;
    }
}