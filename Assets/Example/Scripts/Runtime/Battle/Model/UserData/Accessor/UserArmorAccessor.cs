using System.Collections.Generic;
using Akari.GfCore;

namespace GameMain.Runtime
{
    public class UserArmorAccessor
    {
        private readonly Dictionary<string, UserArmor> _dictionary;
        private readonly List<UserArmor> _list;
        public int Num => _list.Count;
        public UserArmorAccessor(UserData userData)
        {
            _list = userData.Armors ?? new List<UserArmor>();
            _dictionary = new Dictionary<string, UserArmor>();
            foreach (var item in _list)
            {
                _dictionary.Add(item.Guid, item);
            }
        }
        
        // 用不上
        public UserArmor Get(string guid)
        {
            if (!_dictionary.TryGetValue(guid, out var data))
            {
                GfLog.Warn($"ID: {guid} {GetType()}用户数据中没有");
            }
            return data;
        }
        
        public List<UserArmor> GetList()
        {
            return _list;
        }
        
        public UserArmor AddArmor(int id)
        {
            var guid = System.Guid.NewGuid().ToString();
            
            if (!_dictionary.ContainsKey(guid))
            {
                var data = new UserArmor(guid, id);
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
    
    public class UserArmor
    {
        //暂时不需要，因为装备没有重复存在的必要
        public string Guid;//唯一Id System.Guid.NewGuid().ToString()
        
        public int Id;
        
        public int Exp;
        public int Level;

        public int CharacterId;
        
        public UserArmor(string guid, int id)
        {
            Guid = guid;
            Id = id;
            Level = 1;
        }
    }
}