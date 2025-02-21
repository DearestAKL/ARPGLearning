using System.Collections.Generic;
using Akari.GfCore;

namespace GameMain.Runtime
{
    public class UserWeaponAccessor
    {
        private readonly Dictionary<string, UserWeapon> _dictionary;
        private readonly List<UserWeapon> _list;
        public int Num => _list.Count;
        public UserWeaponAccessor(UserData userData)
        {
            _list = userData.Weapons ?? new List<UserWeapon>();
            _dictionary = new Dictionary<string, UserWeapon>();
            foreach (var item in _list)
            {
                _dictionary.Add(item.Guid, item);
            }
        }
        
        // 用不上
        public UserWeapon Get(string guid)
        {
            if (!_dictionary.TryGetValue(guid, out var data))
            {
                GfLog.Warn($"ID: {guid} {GetType()}用户数据中没有");
            }
            return data;
        }
        
        public List<UserWeapon> GetList()
        {
            return _list;
        }

        public UserWeapon AddWeapon(int id)
        {
            var guid = System.Guid.NewGuid().ToString();
            
            if (!_dictionary.ContainsKey(guid))
            {
                var data = new UserWeapon(guid, id);
                _list.Add(data);
                _dictionary.Add(guid, data);   
                return data;
            }
            else
            {
                GfLog.Error($"GUID 重复");
                return null;
            }
        }
    }
    
    public class UserWeapon
    {
        //暂时不需要，因为装备没有重复存在的必要
        public string Guid;//唯一Id System.Guid.NewGuid().ToString()
        
        public int Id;
        
        public int Exp;
        public int Level;
        public int AscensionLevel;

        public int CharacterId;

        public UserWeapon(string guid, int id)
        {
            Guid = guid;
            Id = id;
            Level = 1;
            AscensionLevel = 1;
        }
    }
}