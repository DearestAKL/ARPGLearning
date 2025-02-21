using GameMain.Runtime;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using UnityEditor;

namespace GameMain.Editor
{
    public class ShellDefinitionFieldDrawer : AttackDefinitionGroupFieldDrawer
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
                    case "shellType":
                        return OnDrawType(scope, parent, descriptor, ShellTypeDisplayStrings, "Shell类型");
                    case "bulletType":
                        return OnDrawType(scope, parent, descriptor, BulletTypeDisplayStrings, "Bullet类型");
                    default:
                        return base.DrawInt(parent, descriptor);
                }
            }
        }
        
        public override bool DrawString(IMessage parent, FieldDescriptor descriptor)
        {
            using (var scope = new EditorGUI.ChangeCheckScope())
            {
                switch (descriptor.Name)
                {
                    case "effectId":
                        return OnDrawEffectId(scope, parent, descriptor);
                    default:
                        return base.DrawString(parent, descriptor);
                }
            }
        }
        
        private bool OnDrawEffectId(EditorGUI.ChangeCheckScope scope,IMessage parent, FieldDescriptor descriptor)
        {
            var label = descriptor.Name;
            var value = (string)descriptor.Accessor.GetValue(parent);

            //var assetPath = AssetPathHelper.GetBattleEffectPath(value);
            var assetPath = value;
            
            UnityEngine.GameObject loadedGameObject = AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(assetPath);
            var obj = EditorGUILayout.ObjectField("特效",loadedGameObject, typeof(UnityEngine.GameObject),false);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("路径", value);
            EditorGUI.EndDisabledGroup();
            if (!scope.changed)
            {
                return false;
            }
            //descriptor.Accessor.SetValue(parent, AssetPathHelper.ReplaceBattleEffectPath(AssetDatabase.GetAssetPath(obj)));
            descriptor.Accessor.SetValue(parent, AssetDatabase.GetAssetPath(obj));
            return true;
        }
        
        protected override string GetOneOfNameDisplay(string descriptorName,string fieldName)
        {
            if (descriptorName == "shellConfig")
            {
                switch (fieldName)
                {
                    case "bullet":
                        return ShellTypeDisplayStrings[(int)ShellType.Bullet];
                    case "area":
                        return ShellTypeDisplayStrings[(int)ShellType.Area];
                }
            }
            
            return base.GetOneOfNameDisplay(descriptorName,fieldName);
        }
        
        protected override string GetOneOfLabelDisplay(string label)
        {
            switch (label)
            {
                case "shellConfig":
                    return "shell参数";
            }

            return base.GetOneOfLabelDisplay(label);
        }
    }
}