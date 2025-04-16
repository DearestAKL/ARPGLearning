using System.Collections.Generic;

namespace NPBehave
{
    public struct Notification<T>
    {
        public readonly string Key;
        public readonly Blackboard.Type Type;
        public readonly T Value;
        public Notification(string key, Blackboard.Type type, T value)
        {
            this.Key = key;
            this.Type = type;
            this.Value = value;
        }
    }
    
    public class BlackboardPart<T> 
    {
        private Clock clock => blackboard.clock;
        private readonly Blackboard blackboard;

        private bool isNotifiyng = false;
        
        private readonly Dictionary<string, T> data = new Dictionary<string, T>();
        private readonly Dictionary<string, List<System.Action<Blackboard.Type, T>>> observers = new Dictionary<string, List<System.Action<Blackboard.Type, T>>>();
        private readonly Dictionary<string, List<System.Action<Blackboard.Type, T>>> addObservers = new Dictionary<string, List<System.Action<Blackboard.Type, T>>>();
        private readonly Dictionary<string, List<System.Action<Blackboard.Type, T>>> removeObservers = new Dictionary<string, List<System.Action<Blackboard.Type, T>>>();
        private List<Notification<T>> notifications = new List<Notification<T>>();
        private List<Notification<T>> notificationsDispatch = new List<Notification<T>>();
        
        private BlackboardPart<T> parent;
        public HashSet<BlackboardPart<T>> children = new HashSet<BlackboardPart<T>>();

        public BlackboardPart(BlackboardPart<T> parent, Blackboard blackboard)
        {
            this.parent = parent;
        }

        public BlackboardPart(Blackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public void Enable()
        {
            if (parent != null)
            {
                parent.children.Add(this);
            }
        }

        public void Disable()
        {
            if (parent != null)
            {
                parent.children.Remove(this);
            }
            
            if (clock != null)
            {
                clock.RemoveTimer(this.NotifiyObservers);
            }
        }

        public void Set(string key, T value )
        {
            if (parent != null && parent.IsSet(key))
            {
                parent.Set(key, value);
            }
            else
            {
                if (!data.ContainsKey(key))
                {
                    data[key] = value;
                    this.notifications.Add(new Notification<T>(key, Blackboard.Type.ADD, value));
                    this.clock.AddTimer(0f, 0, NotifiyObservers);
                }
                else
                {
                    if ((!this.data[key].Equals(value)))
                    {
                        this.data[key] = value;
                        this.notifications.Add(new Notification<T>(key, Blackboard.Type.CHANGE, value));
                        this.clock.AddTimer(0f, 0, NotifiyObservers);
                    }
                }
            }
        }
        
        public void Unset(string key)
        {
            if (this.data.ContainsKey(key))
            {
                this.data.Remove(key);
                this.notifications.Add(new Notification<T>(key, Blackboard.Type.REMOVE, default));
                this.clock.AddTimer(0f, 0, NotifiyObservers);
            }
        }

        public T Get(string key)
        {
            if (this.data.ContainsKey(key))
            {
                return data[key];
            }
            else if (parent != null)
            {
                return parent.Get(key);
            }
            else
            {
                return default;
            }
        }
        
        public T this[string key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                Set(key, value);
            }
        }
        
        public bool IsSet(string key)
        {
            return this.data.ContainsKey(key) || (parent != null && parent.IsSet(key));
        }
        
        public void AddObserver(string key, System.Action<Blackboard.Type, T> observer)
        {
            List<System.Action<Blackboard.Type, T>> observers = GetObserverList(this.observers, key);
            if (!isNotifiyng)
            {
                if (!observers.Contains(observer))
                {
                    observers.Add(observer);
                }
            }
            else
            {
                if (!observers.Contains(observer))
                {
                    List<System.Action<Blackboard.Type, T>> addObservers = GetObserverList(this.addObservers, key);
                    if (!addObservers.Contains(observer))
                    {
                        addObservers.Add(observer);
                    }
                }
                List<System.Action<Blackboard.Type, T>> removeObservers = GetObserverList(this.removeObservers, key);
                if (removeObservers.Contains(observer))
                {
                    removeObservers.Remove(observer);
                }
            }
        }
        public void RemoveObserver(string key, System.Action<Blackboard.Type, T> observer)
        {
            List<System.Action<Blackboard.Type, T>> observers = GetObserverList(this.observers, key);
            if (!isNotifiyng)
            {
                if (observers.Contains(observer))
                {
                    observers.Remove(observer);
                }
            }
            else
            {
                List<System.Action<Blackboard.Type, T>> removeObservers = GetObserverList(this.removeObservers, key);
                if (!removeObservers.Contains(observer))
                {
                    if (observers.Contains(observer))
                    {
                        removeObservers.Add(observer);
                    }
                }
                List<System.Action<Blackboard.Type, T>> addObservers = GetObserverList(this.addObservers, key);
                if (addObservers.Contains(observer))
                {
                    addObservers.Remove(observer);
                }
            }
        }

        private void NotifiyObservers()
        {
            if (notifications.Count == 0)
            {
                return;
            }

            notificationsDispatch.Clear();
            notificationsDispatch.AddRange(notifications);
            foreach (BlackboardPart<T> child in children)
            {
                child.notifications.AddRange(notifications);
                child.clock.AddTimer(0f, 0, child.NotifiyObservers);
            }
            notifications.Clear();

            isNotifiyng = true;
            foreach (Notification<T> notification in notificationsDispatch)
            {
                if (!this.observers.ContainsKey(notification.Key))
                {
                    // Debug.Log("1 do not notify for key:" + notification.key + " value: " + notification.value);
                    continue;
                }

                List<System.Action<Blackboard.Type, T>> observers = GetObserverList(this.observers, notification.Key);
                foreach (System.Action<Blackboard.Type, T> observer in observers)
                {
                    if (this.removeObservers.ContainsKey(notification.Key) && this.removeObservers[notification.Key].Contains(observer))
                    {
                        continue;
                    }
                    observer(notification.Type, notification.Value);
                }
            }

            foreach (string key in this.addObservers.Keys)
            {
                GetObserverList(this.observers, key).AddRange(this.addObservers[key]);
            }
            foreach (string key in this.removeObservers.Keys)
            {
                foreach (System.Action<Blackboard.Type, T> action in removeObservers[key])
                {
                    GetObserverList(this.observers, key).Remove(action);
                }
            }
            this.addObservers.Clear();
            this.removeObservers.Clear();

            isNotifiyng = false;
        }
        
        private List<System.Action<Blackboard.Type, T>> GetObserverList<T>(Dictionary<string, List<System.Action<Blackboard.Type, T>>> target, string key)
        {
            List<System.Action<Blackboard.Type, T>> observers;
            if (target.ContainsKey(key))
            {
                observers = target[key];
            }
            else
            {
                observers = new List<System.Action<Blackboard.Type, T>>();
                target[key] = observers;
            }
            return observers;
        }
        
#if UNITY_EDITOR
        public List<string> Keys
        {
            get
            {
                List<string> keys = new List<string>();
                if (this.parent != null)
                {
                    List<string> parentKeys = this.parent.Keys;
                    keys.AddRange(parentKeys);
                }
                keys.AddRange(data.Keys);
                return keys;
            }
        }

        public int NumObservers
        {
            get
            {
                int count = 0;
                if (this.parent != null)
                {
                    count += parent.NumObservers;
                }
                foreach (string key in observers.Keys)
                {
                    count += observers[key].Count;
                }
                return count;
            }
        }
#endif
    }
}