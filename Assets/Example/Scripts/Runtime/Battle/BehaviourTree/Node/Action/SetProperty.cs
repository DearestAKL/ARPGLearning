using Akari.GfCore;
using NPBehave;

namespace GameMain.Runtime
{
    public class SetIntProperty : Task
    {
        private string _key;
        private int _value;
        
        public SetIntProperty(string key,int value) : base("SetIntProperty")
        {
            _key = key;
            _value = value;
        }
        
        protected override void DoStart()
        {
            Blackboard.SetInt(_key, _value);

            Stopped(true);
            
            GfLog.Debug("SetIntProperty");
        }
    }
    
    public class SetBoolProperty : Task
    {
        private string _key;
        private bool _value;
        
        public SetBoolProperty(string key,bool value) : base("SetBoolProperty")
        {
            _key = key;
            _value = value;
        }
        
        protected override void DoStart()
        {
            Blackboard.SetBool(_key, _value);

            Stopped(true);
            
            GfLog.Debug("SetBoolProperty");
        }
    }
    
    public class SetFloatProperty : Task
    {
        private string _key;
        private float _value;
        
        public SetFloatProperty(string key,float value) : base("SetFloatProperty")
        {
            _key = key;
            _value = value;
        }
        
        protected override void DoStart()
        {
            Blackboard.SetFloat(_key, _value);

            Stopped(true);
            
            GfLog.Debug("SetFloatProperty");
        }
    }
}