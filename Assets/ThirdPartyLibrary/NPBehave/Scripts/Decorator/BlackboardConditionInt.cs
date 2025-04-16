using System;

namespace NPBehave
{
    public abstract class ABlackboardCondition : NPBehave.ObservingDecorator
    {
        protected string key;
        protected Operator op;
        
        public string Key
        {
            get
            {
                return key;
            }
        }
        
        public Operator Operator
        {
            get
            {
                return op;
            }
        }

        public abstract object GetObjectValue();

        protected ABlackboardCondition(string key,  Operator op,string name,Stops stopsOnChange, Node decoratee) : base(name, stopsOnChange, decoratee)
        {
            this.key = key;
            this.op = op;
        }
    }

    public class BlackboardConditionInt : ABlackboardCondition
    {
        private int value;

        public override object GetObjectValue() => value;

        public BlackboardConditionInt(string key, Operator op, int value, NPBehave.Stops stopsOnChange,
            NPBehave.Node decoratee) : base(key, op, "BlackboardConditionInt",stopsOnChange, decoratee)
        {
            this.value = value;
        }
        
        protected override void StartObserving()
        {
            this.RootNode.Blackboard.AddObserverInt(key, OnValueChanged);
        }

        protected override void StopObserving()
        {
            this.RootNode.Blackboard.RemoveObserverInt(key, OnValueChanged);
        }

        private void OnValueChanged(Blackboard.Type type, int newValue)
        {
            Evaluate();
        }

        protected override bool IsConditionMet()
        {
            if (op == Operator.ALWAYS_TRUE)
            {
                return true;
            }

            if (!this.RootNode.Blackboard.IsSetInt(key))
            {
                return op == Operator.IS_NOT_SET;
            }

            var o = this.RootNode.Blackboard.GetInt(key);

            switch (this.op)
            {
                case Operator.IS_SET: return true;
                case Operator.IS_EQUAL: return o == value;
                case Operator.IS_NOT_EQUAL: return o != value;

                case Operator.IS_GREATER_OR_EQUAL:
                    return (int)o >= (int)this.value;

                case Operator.IS_GREATER:
                    return (int)o > (int)this.value;
                
                case Operator.IS_SMALLER_OR_EQUAL:
                    return (int)o <= (int)this.value;

                case Operator.IS_SMALLER:
                    return (int)o < (int)this.value;

                default: return false;
            }
        }

        public override string ToString()
        {
            return "(" + this.op + ") " + this.key + " ? " + this.value;
        }
    }
    
    public class BlackboardConditionFloat : ABlackboardCondition
    {
        private float value;

        public override object GetObjectValue() => value;

        public BlackboardConditionFloat(string key, Operator op, float value, NPBehave.Stops stopsOnChange, NPBehave.Node decoratee) : base(key,op, "BlackboardConditionFloat",stopsOnChange, decoratee)
        {
            this.value = value;
        }
        
        protected override void StartObserving()
        {
            this.RootNode.Blackboard.AddObserverFloat(key, OnValueChanged);
        }

        protected override void StopObserving()
        {
            this.RootNode.Blackboard.RemoveObserverFloat(key, OnValueChanged);
        }

        private void OnValueChanged(Blackboard.Type type, float newValue)
        {
            Evaluate();
        }

        protected override bool IsConditionMet()
        {
            if (op == Operator.ALWAYS_TRUE)
            {
                return true;
            }

            if (!this.RootNode.Blackboard.IsSetFloat(key))
            {
                return op == Operator.IS_NOT_SET;
            }

            var o = this.RootNode.Blackboard.GetFloat(key);
            
            switch (this.op)
            {
                case Operator.IS_SET: return true;
                case Operator.IS_EQUAL: return Math.Abs(o - value) < 0.01f;
                case Operator.IS_NOT_EQUAL: return Math.Abs(o - value) > 0.01f;

                case Operator.IS_GREATER_OR_EQUAL:
                    return (float)o >= (float)this.value;

                case Operator.IS_GREATER:
                    return (float)o > (float)this.value;
                
                case Operator.IS_SMALLER_OR_EQUAL:
                    return (float)o <= (float)this.value;

                case Operator.IS_SMALLER:
                    return (float)o < (float)this.value;

                default: return false;
            }
        }

        override public string ToString()
        {
            return "(" + this.op + ") " + this.key + " ? " + this.value;
        }
    }
    
    public class BlackboardConditionBool : ABlackboardCondition
    {
        private bool value;

        public override object GetObjectValue() => value;

        public BlackboardConditionBool(string key, Operator op, bool value, NPBehave.Stops stopsOnChange, NPBehave.Node decoratee) : base(key,op, "BlackboardConditionBool",stopsOnChange, decoratee)
        {
            this.value = value;
        }
        
        protected override void StartObserving()
        {
            this.RootNode.Blackboard.AddObserverBool(key, OnValueChanged);
        }

        protected override void StopObserving()
        {
            this.RootNode.Blackboard.RemoveObserverBool(key, OnValueChanged);
        }

        private void OnValueChanged(Blackboard.Type type, bool newValue)
        {
            Evaluate();
        }

        protected override bool IsConditionMet()
        {
            if (op == Operator.ALWAYS_TRUE)
            {
                return true;
            }

            if (!this.RootNode.Blackboard.IsSetBool(key))
            {
                return op == Operator.IS_NOT_SET;
            }

            var o = this.RootNode.Blackboard.GetBool(key);

            switch (this.op)
            {
                case Operator.IS_SET: return true;
                case Operator.IS_EQUAL: return o == value;
                case Operator.IS_NOT_EQUAL: return o != value;
                default: return false;
            }
        }

        override public string ToString()
        {
            return "(" + this.op + ") " + this.key + " ? " + this.value;
        }
    }
    
    public class BlackboardConditionObject : ABlackboardCondition
    {
        private object value;

        public override object GetObjectValue() => value;

        public BlackboardConditionObject(string key, Operator op, object value, NPBehave.Stops stopsOnChange, NPBehave.Node decoratee) : base(key,op, "BlackboardConditionObject",stopsOnChange, decoratee)
        {
            this.value = value;
        }
        
        protected override void StartObserving()
        {
            this.RootNode.Blackboard.AddObserverObject(key, OnValueChanged);
        }

        protected override void StopObserving()
        {
            this.RootNode.Blackboard.RemoveObserverObject(key, OnValueChanged);
        }

        private void OnValueChanged(Blackboard.Type type, object newValue)
        {
            Evaluate();
        }

        protected override bool IsConditionMet()
        {
            if (op == Operator.ALWAYS_TRUE)
            {
                return true;
            }

            if (!this.RootNode.Blackboard.IsSetObject(key))
            {
                return op == Operator.IS_NOT_SET;
            }

            var o = this.RootNode.Blackboard.GetObject(key);

            switch (this.op)
            {
                case Operator.IS_SET: return true;
                case Operator.IS_EQUAL: return Equals(0,value);
                case Operator.IS_NOT_EQUAL: return !Equals(0,value);
                default: return false;
            }
        }

        public override string ToString()
        {
            return "(" + this.op + ") " + this.key + " ? " + this.value;
        }
    }
}