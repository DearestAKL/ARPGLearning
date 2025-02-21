using System.Collections.Generic;
using System.Linq;
using Akari.GfCore;
using Akari.GfUnityEditor.ProtobufExtensions;
using GameMain.Runtime;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using UnityEditor;

namespace GameMain.Editor
{
    public class PassiveSkillDefinitionFieldDrawer : GameMainExFieldDrawer
    {
        public override bool DrawInt(IMessage parent, FieldDescriptor descriptor)
        {
            using (var scope = new EditorGUI.ChangeCheckScope())
            {
                if (descriptor.Name.EndsWith("Index"))
                {
                    //TODO: 根据descriptor.Name 可以就行更精细的判断
                    return OnNumericalValue(scope, parent, descriptor,descriptor.Name);
                }
                
                switch (descriptor.Name)
                {
                    case "eventType":
                        return OnDrawType(scope, parent, descriptor,PassiveSkillEventTypeDisplayStrings,"触发类型");
                    case "selectTargetType":
                        return OnDrawType(scope, parent, descriptor,SelectTargetTypeDisplayStrings,"目标类型");
                    case "filterType":
                        return OnDrawType(scope, parent, descriptor,SelectTargetFilterTypeDisplayStrings,"触发条件类型");
                    case "proConditionType":
                        return OnDrawType(scope, parent, descriptor,PassiveSkillProConditionTypeDisplayStrings,"条件类型");
                    case "attributeType":
                        return OnDrawType(scope, parent, descriptor,AttributeTypeDisplayStrings,"属性类型");
                    default:
                        return base.DrawInt(parent, descriptor);
                }
            }
        }
        
        public override bool DrawRepeated(IMessage parent, FieldDescriptor descriptor)
        {
            if (!Foldouts.ContainsKey(descriptor.Name))
            {
                Foldouts.Add(descriptor.Name, true);
            }

            Foldouts[descriptor.Name] = EditorGUILayout.Foldout(Foldouts[descriptor.Name], descriptor.Name);
            if (!Foldouts[descriptor.Name])
            {
                return false;
            }

            if (!RepeatedFieldDrawers.ContainsKey(descriptor.Name))
            {
                RepeatedFieldDrawers.Add(descriptor.Name,
                    PassiveSkillDefinitionRepeatFieldDrawer.GetRepeatFieldDrawer(parent, descriptor));
            }

            return RepeatedFieldDrawers[descriptor.Name].Draw();
        }

        protected override string GetOneOfNameDisplay(string descriptorName,string fieldName)
        {
            if (descriptorName == "proCondition")
            {
                switch (fieldName)
                {
                    case "timeInterval":
                        return PassiveSkillProConditionTypeDisplayStrings[(int)PassiveSkillProConditionType.TimeInterval];
                    case "attribute":
                        return PassiveSkillProConditionTypeDisplayStrings[(int)PassiveSkillProConditionType.Attribute];
                    case "hasBuffer":
                        return PassiveSkillProConditionTypeDisplayStrings[(int)PassiveSkillProConditionType.HasBuffer];
                }
            }
            else if (descriptorName == "filter")
            {
                switch (fieldName)
                {
                    case "attribute":
                        return SelectTargetFilterTypeDisplayStrings[(int)SelectTargetFilterType.Attribute];
                }
            }
            
            return base.GetOneOfNameDisplay(descriptorName,fieldName);
        }
        
        protected override string GetOneOfLabelDisplay(string label)
        {
            switch (label)
            {
                case "proCondition":
                    return "条件参数";
                case "filter":
                    return "筛选参数";
            }

            return base.GetOneOfLabelDisplay(label);
        }
    }
}