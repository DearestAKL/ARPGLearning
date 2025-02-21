using Akari;
using cfg;

namespace GameMain.Runtime
{
    public abstract class AUIItemData
    {
    }

    public class UIItemData : AUIItemData
    {
        public ItemType ItemType => Config.Type;
        
        private readonly UserItem _userData;
        public ItemConfig Config;

        private readonly int _amount;
        public int Amount => _userData?.Amount ?? _amount;

        public UIItemData(UserItem userData)
        {
            _userData = userData;
            Config = LubanManager.Instance.Tables.TbItem.Get(userData.Id);
        }

        public UIItemData(int id,int amount)
        {
            Config = LubanManager.Instance.Tables.TbItem.Get(id);
            _amount = amount;
        }
    }

    public class UIWeaponData : AUIItemData
    {
        private readonly UserWeapon _userData;
        public WeaponConfig Config;
        
        public int Level => _userData?.Level ?? 1;
        public int AscensionLevel => _userData?.AscensionLevel ?? 1;
        public int CharacterId => _userData?.CharacterId ?? 0;
        
        public UIWeaponData(UserWeapon userData)
        {
            _userData = userData;
            Config = LubanManager.Instance.Tables.TbWeapon.Get(userData.Id);
        }
        
        public UIWeaponData(int id)
        {
            Config = LubanManager.Instance.Tables.TbWeapon.Get(id);
        }
    }
    
    public class UIArmorData : AUIItemData
    {
        private readonly UserArmor _userData;
        public ArmorConfig Config;
        
        public int Level => _userData?.Level ?? 1;
        public int CharacterId => _userData?.CharacterId ?? 0;
        
        public UIArmorData(UserArmor userData)
        {
            _userData = userData;
            Config = LubanManager.Instance.Tables.TbArmor.Get(userData.Id);
        }
        
        public UIArmorData(int id)
        {
            Config = LubanManager.Instance.Tables.TbArmor.Get(id);
        }
    }
}