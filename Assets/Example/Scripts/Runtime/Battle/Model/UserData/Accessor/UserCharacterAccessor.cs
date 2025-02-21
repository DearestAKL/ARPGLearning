using System.Collections.Generic;
using Akari.GfCore;

namespace GameMain.Runtime
{
    public class UserCharacterAccessor
    {
        private readonly Dictionary<int, UserCharacter> _dictionary;
        private readonly List<UserCharacter> _list;
        
        public UserCharacterAccessor(UserData userData)
        {
            _list = userData.Characters ?? new List<UserCharacter>();
            _dictionary = new Dictionary<int, UserCharacter>();
            foreach (var item in _list)
            {
                _dictionary.Add(item.Id, item);
            }

            if (userData.Characters == null || userData.Characters.Count == 0)
            {
                InitCharacter();
            }
        }
        
        public UserCharacter Get(int id)
        {
            if (!_dictionary.TryGetValue(id, out var data))
            {
                GfLog.Warn($"ID: {id} {GetType()}用户数据中没有");
            }

            return data;
        }
        
        public List<UserCharacter> GetList()
        {
            return _list;
        }

        public void AddCharacter(int id)
        {
            if (!_dictionary.ContainsKey(id))
            {
                var data = new UserCharacter(id);
                _list.Add(data);
                _dictionary.Add(id, data);   
            }
            else
            {
                GfLog.Warn($"ID: {id} {GetType()} 已经拥有了");
            }
        }

        public void AddExp(int id, int exp)
        {
            UserCharacter userCharacter = Get(id);
            //判断是否已经满级
            if (LubanManager.Instance.Tables.TbCharacterLevelExp.Get(userCharacter.Level).UpExp <= 0)
            {
                //满级了
                return;
            }
            
            var newExp = userCharacter.Exp + exp;
            userCharacter.Exp = newExp;
            
            var newLevel = CharacterDataHelper.GetLevelUp(userCharacter.Level, newExp);
            if (newLevel > userCharacter.Level)
            {
                //升级了
                userCharacter.Level = newLevel;
                EventManager.Instance.UIEvent.OnPlayerCharacterLevelChangeEvent.Invoke(id, newLevel);
            }
            
            //先更新等级 再更新经验 因为等级会影响当前经验上限
            EventManager.Instance.UIEvent.OnPlayerCharacterExpChangeEvent.Invoke(id, newExp);
        }

        public void Ascension(int id)
        {
            UserCharacter userCharacter = Get(id);
            if (userCharacter.AscensionLevel >= 6)
            {
                //满阶了
                return;
            }
            userCharacter.AscensionLevel += 1;
        }
        
        private void InitCharacter()
        {
            AddCharacter(1001);
            AddCharacter(1002);
            AddCharacter(1003);
            AddCharacter(1004);
        }
    }
    
    public class UserCharacter
    {
        public int Id;
        
        public int Exp;//总经验 在界面中显示需要减去当前等级经验
        public int Level;
        public int AscensionLevel;
        
        public string Weapon;
        
        public string Chest;
        public string Legs;
        public string Feet;

        public UserCharacter(int id)
        {
            Id = id;
            Level = 1;
        }
    }
}