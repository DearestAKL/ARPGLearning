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
    public class AttackDefinitionGroupFieldDrawer : GameMainExFieldDrawer
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
                    case "reactionLevelType":
                        return OnDrawType(scope, parent, descriptor, ReactionLevelTypeDisplayStrings, "反应等级");
                    case "reducePoiseValue":
                        return DrawIntDisplayValue(scope, parent, descriptor,"削韧值");
                    case "horizontalPower":
                        return DrawIntDisplayValue(scope, parent, descriptor,"水平力");
                    case "verticalPower":
                        return DrawIntDisplayValue(scope, parent, descriptor,"垂直力");
                    case "sourceType":
                        return OnDrawType(scope, parent, descriptor, SourceAttributeTypeDisplayStrings, "属性来源类型(只支持攻防血)");
                    case "categoryType":
                        return OnDrawType(scope, parent, descriptor, AttackCategoryTypeDisplayStrings, "攻击类型");
                    case "hitCategoryType":
                        return OnDrawType(scope, parent, descriptor, BattleHitCategoryTypeDisplayStrings, "攻击目标类型");
                    case "shakeDirection":
                        return OnDrawType(scope, parent, descriptor, ShakeDirectionTypeDisplayStrings, "震动方向");
                    case "shakePower":
                        return OnDrawType(scope, parent, descriptor, ShakePowerTypeDisplayStrings, "震动强度");
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
                RepeatedFieldDrawers.Add(descriptor.Name, AttackDefinitionGroupRepeatFieldDrawer.GetRepeatFieldDrawer(parent, descriptor));
            }

            return RepeatedFieldDrawers[descriptor.Name].Draw();
        }
    }
}