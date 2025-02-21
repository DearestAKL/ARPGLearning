using System.Collections.Generic;
using Akari.GfCore;

namespace GameMain.Runtime
{
    public class UserItemAccessor
    {
        private readonly Dictionary<int, UserItem> _dictionary;
        private readonly List<UserItem> _list;
        public int Num => _list.Count;
        public UserItemAccessor(UserData userData)
        {
            _list = userData.Items ?? new List<UserItem>();
            _dictionary = new Dictionary<int, UserItem>();
            foreach (var item in _list)
            {
                _dictionary.Add(item.Id, item);
            }
        }

        public bool HasItem(int id)
        {
            return _dictionary.ContainsKey(id);
        }

        public UserItem Get(int id)
        {
            if (!_dictionary.TryGetValue(id, out var data))
            {
                GfLog.Warn($"ID: {id} {GetType()}用户数据中没有");
            }
            return data;
        }

        public void AddItem(int id,int num)
        {
            if (_dictionary.TryGetValue(id, out var value))
            {
                value.Amount += num;
            }
            else
            {
                var userItem = new UserItem(id, num); 
                _dictionary.Add(id, userItem);
                _list.Add(userItem);
            }
        }

        public void UseItem(int id,int num)
        {
            if (_dictionary.TryGetValue(id, out var value))
            {
                if (num > value.Amount)
                {
                    //数量不够
                    GfLog.Error($"Item{id}数量不足");
                    return;
                }
                value.Amount -= num;
                
                //item数量为0时，需要移除出去吗
                if (value.Amount <= 0)
                {
                    _dictionary.Remove(id);
                    _list.Remove(value); 
                }
            }
            else
            {
                //没有这个材料
                GfLog.Error($"没有Item{id}");
            }
        }


        public List<UserItem> GetList()
        {
            return _list;
        }
    }
    
    public class UserItem
    {
        public int Id;
        
        public int Amount;

        public UserItem(int id, int amount)
        {
            Id = id;
            Amount = amount;
        }
    }
}