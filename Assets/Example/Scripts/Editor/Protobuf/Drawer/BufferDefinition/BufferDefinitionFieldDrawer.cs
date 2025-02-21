using GameMain.Runtime;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using UnityEditor;

namespace GameMain.Editor
{
    public class BufferDefinitionFieldDrawer : GameMainExFieldDrawer
    {
        public override bool DrawInt(IMessage parent, FieldDescriptor descriptor)
        {
            using (var scope = new EditorGUI.ChangeCheckScope())
            {
                switch (descriptor.Name)
                {
                    case "attributeType":
                        return OnDrawType(scope, parent, descriptor,AttributeTypeDisplayStrings,"属性类型");
                    case "overlayType":
                        return OnDrawType(scope, parent, descriptor,BufferOverlayTypeDisplayStrings,"buffer叠加类型");
                    case "endType":
                        return OnDrawType(scope, parent, descriptor,BufferEndTypeDisplayStrings,"buffer结束条件类型");
                    case "effectType":
                        return OnDrawType(scope, parent, descriptor,BufferTypeDisplayStrings,"BufferEffect类型");
                    case "triggerType":
                        return OnDrawType(scope, parent, descriptor,BufferEffectTriggerTypeDisplayStrings,"BufferEffect触发类型");
                    case "validType":
                        return OnDrawType(scope, parent, descriptor,BufferEffectValidTypeDisplayStrings,"BufferEffect生效类型");
                    case "sourceType":
                        return OnDrawType(scope, parent, descriptor, SourceAttributeTypeDisplayStrings, "属性来源类型(只支持攻防血)");
                    case "fixedValueIndex":
                        return OnNumericalValue(scope, parent, descriptor,"固定数值");
                    case "percentageValueIndex":
                        return OnNumericalValue(scope, parent, descriptor,"来源百分比数值");
                    default:
                        if (descriptor.Name.EndsWith("Index"))
                        {
                            return OnNumericalValue(scope, parent, descriptor,descriptor.Name);
                        }
                        return base.DrawInt(parent, descriptor);
                }
            }
        }

        public override bool DrawBool(IMessage parent, FieldDescriptor descriptor)
        {
            using (var scope = new EditorGUI.ChangeCheckScope())
            {
                switch (descriptor.Name)
                {
                    case "isPercentage":
                        return DrawBoolDisplayValue(scope, parent, descriptor,"是否是百分比");
                    case "isUsedSourceType":
                        return DrawBoolDisplayValue(scope, parent, descriptor,"是否使用来源属性");
                    case "isUsedBufferSource":
                        return DrawBoolDisplayValue(scope, parent, descriptor,"是否使用Buffer施加者的属性");
                    default:
                        return base.DrawBool(parent, descriptor);
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
                    BufferDefinitionRepeatFieldDrawer.GetRepeatFieldDrawer(parent, descriptor));
            }

            return RepeatedFieldDrawers[descriptor.Name].Draw();
        }

        protected override string GetOneOfNameDisplay(string descriptorName,string fieldName)
        {
            if (descriptorName == "bufferEffect")
            {
                switch (fieldName)
                {
                    case "attribute":
                        return BufferTypeDisplayStrings[(int)BufferEffectType.Attribute];
                    case "changeCurHp":
                        return BufferTypeDisplayStrings[(int)BufferEffectType.ChangeCurHp];
                }
            }
            else if (descriptorName == "validCondition")
            {
                switch (fieldName)
                {
                    case "timeInterval":
                        return BufferEffectValidTypeDisplayStrings[(int)BufferEffectValidType.TimeInterval];
                    case "attribute":
                        return BufferEffectValidTypeDisplayStrings[(int)BufferEffectValidType.Attribute];
                }
            }
            
            return base.GetOneOfNameDisplay(descriptorName,fieldName);
        }
        
        protected override string GetOneOfLabelDisplay(string label)
        {
            switch (label)
            {
                case "bufferEffect":
                    return "BufferEffect参数";
                case "validCondition":
                    return "生效条件参数";
            }

            return base.GetOneOfLabelDisplay(label);
        }
    }
}