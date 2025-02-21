using System.Collections.Generic;
using cfg;
using Luban;
using UnityEngine;
using Akari.GfCore;
using Cysharp.Threading.Tasks;
using UnityEditor;

namespace GameMain.Runtime
{
    public class LubanManager : GfSingleton<LubanManager>
    {
        private Tables _tables;
        public Tables Tables => _tables;

#if UNITY_EDITOR
        [MenuItem("Akari/UpdateEditorTables")]
        public static void UpdateEditorTables()
        {
            _editorTables =  new cfg.Tables(Load);
        }
        
        private static Tables _editorTables;
        public static Tables GetEditorTables() 
        {
            if (_editorTables == null)
            {
                _editorTables =  new cfg.Tables(Load);
            }

            return _editorTables;
        }
        
        private static ByteBuf Load(string file)
        {
            var path = AssetPathHelper.GetLubanDataPath(file);
            var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
            return new ByteBuf(textAsset.bytes);
        }
#endif
        
        protected override void OnCreated()
        {
        }
        
        protected override void OnDisposed()
        {
        }
        
        public async UniTask Init()
        {
            var files = Tables.GetFiles();
            var byteBufs = new Dictionary<string, ByteBuf>();
            
            for (int i = 0; i < files.Length; i++)
            {
                var file = files[i];
                var path = AssetPathHelper.GetLubanDataPath(file);
                var textAsset = await AssetManager.Instance.LoadAsset<TextAsset>(path);
                byteBufs.Add(file, new ByteBuf(textAsset.bytes));
            }

            _tables = new Tables(byteBufs);
        }
    }
}