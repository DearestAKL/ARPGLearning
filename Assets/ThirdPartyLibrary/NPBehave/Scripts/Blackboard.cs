using System.Collections.Generic;

namespace NPBehave
{
    public class Blackboard
    {
        public enum Type
        {
            ADD,
            REMOVE,
            CHANGE
        }


        private BlackboardPart<int> intPart;
        private BlackboardPart<bool> boolPart;
        private BlackboardPart<float> floatPart;
        private BlackboardPart<object> objectPart;
        
        public Clock clock;
        
        public Blackboard(Blackboard parent, NPBehave.Clock clock)
        {
            this.clock = clock;

            intPart = new BlackboardPart<int>(parent.intPart,this);
            boolPart = new BlackboardPart<bool>(parent.boolPart,this);
            floatPart = new BlackboardPart<float>(parent.floatPart,this);
            objectPart = new BlackboardPart<object>(parent.objectPart,this);
        }
        public Blackboard(NPBehave.Clock clock)
        {
            this.clock = clock;
            
            intPart = new BlackboardPart<int>(this);
            boolPart = new BlackboardPart<bool>(this);
            floatPart = new BlackboardPart<float>(this);
            objectPart = new BlackboardPart<object>(this);
        }

        public void Enable()
        {
            intPart.Enable();
            boolPart.Enable();
            floatPart.Enable();
            objectPart.Enable();
        }

        public void Disable()
        {
            intPart.Disable();
            boolPart.Disable();
            floatPart.Disable();
            objectPart.Disable();
        }

        public object Get(string key)
        {
            if (objectPart.IsSet(key))
            {
                return GetObject(key);
            }
            else if (intPart.IsSet(key))
            {
                return GetInt(key);
            }
            else if (boolPart.IsSet(key))
            {
                return GetBool(key);
            }
            else if (floatPart.IsSet(key))
            {
                return GetFloat(key);
            }
            else
            {
                return default;
            }
        }


        public void SetInt(string key, int value)
        {
            intPart.Set(key,value);
        }

        public void UnsetInt(string key)
        {
            intPart.Unset(key);
        }

        public int GetInt(string key)
        {
            if (IsSetInt(key))
            {
                return intPart.Get(key);
            }
            else
            {
                return default;
            }
        }
        
        public void SetBool(string key, bool value)
        {
            boolPart.Set(key,value);
        }

        public void UnsetBool(string key)
        {
            boolPart.Unset(key);
        }
        
        public bool GetBool(string key)
        {
            if (IsSetBool(key))
            {
                return boolPart.Get(key);
            }
            else
            {
                return default;
            }
        }

        public void SetFloat(string key, float value)
        {
            floatPart.Set(key,value);
        }
        
        public void UnsetFloat(string key)
        {
            floatPart.Unset(key);
        }

        public float GetFloat(string key)
        {
            if (IsSetFloat(key))
            {
                return floatPart.Get(key);
            }
            else
            {
                return default;
            }
        }

        public void SetObject(string key, float value)
        {
            objectPart.Set(key,value);
        }
        
        public void UnsetObject(string key)
        {
            objectPart.Unset(key);
        }
        
        public object GetObject(string key)
        {
            if (IsSetObject(key))
            {
                return objectPart.Get(key);
            }
            else
            {
                return default;
            }
        }

        public bool IsSetInt(string key)
        {
            return intPart.IsSet(key);
        }
        
        public bool IsSetBool(string key)
        {
            return boolPart.IsSet(key);
        }
        
        public bool IsSetFloat(string key)
        {
            return floatPart.IsSet(key);
        }
        
        public bool IsSetObject(string key)
        {
            return objectPart.IsSet(key);
        }

        public void AddObserverInt(string key, System.Action<Type, int> observer)
        {
            intPart.AddObserver(key, observer);
        }

        public void RemoveObserverInt(string key, System.Action<Type, int> observer)
        {
            intPart.RemoveObserver(key,observer);
        }
        
        public void AddObserverBool(string key, System.Action<Type, bool> observer)
        {
            boolPart.AddObserver(key, observer);
        }
        public void RemoveObserverBool(string key, System.Action<Type, bool> observer)
        {
            boolPart.RemoveObserver(key,observer);
        }
        
        public void AddObserverFloat(string key, System.Action<Type, float> observer)
        {
            floatPart.AddObserver(key, observer);
        }
        public void RemoveObserverFloat(string key, System.Action<Type, float> observer)
        {
            floatPart.RemoveObserver(key,observer);
        }
        
        public void AddObserverObject(string key, System.Action<Type, object> observer)
        {
            objectPart.AddObserver(key, observer);
        }
        public void RemoveObserverObject(string key, System.Action<Type, object> observer)
        {
            objectPart.RemoveObserver(key,observer);
        }
        

#if UNITY_EDITOR
        public List<string> Keys
        {
            get
            {
                List<string> keys = new List<string>();
                keys.AddRange(intPart.Keys);
                keys.AddRange(boolPart.Keys);
                keys.AddRange(floatPart.Keys);
                keys.AddRange(objectPart.Keys);
                return keys;
            }
        }
        
        public int NumObservers
        {
            get
            {
                int count = 0;
                count += intPart.NumObservers;
                count += boolPart.NumObservers;
                count += floatPart.NumObservers;
                count += objectPart.NumObservers;
                return count;
            }
        }
#endif
    }
}
