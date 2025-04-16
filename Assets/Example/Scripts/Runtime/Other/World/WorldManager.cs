using System.Collections.Generic;
using Akari.GfCore;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameMain.Runtime
{
    public class WorldManager : GfSingleton<WorldManager>
    {
        private Dictionary<string, World> _dict = new Dictionary<string, World>();
        private List<string> _removes = new List<string>();
        private World _curWorld;
        public World CurWorld => _curWorld;

        public void Update(float deltaTime)
        {
            //超时自动卸载
            foreach (var item in _dict)
            {
                var world = item.Value;
                if (_curWorld == world)
                {
                    continue;
                }
                
                if (!world.gameObject.activeSelf)
                {
                    if (!item.Value.CheckTime(deltaTime))
                    {
                        //remove
                        _removes.Add(item.Key);
                    }
                }
            }

            foreach (var remove in _removes)
            {
                _dict.Remove(remove);
            }
        }

        public void SetWorldActive(bool active)
        {
            _curWorld.gameObject.SetActive(active);
        }

        public async UniTask ChangeWorld(int worldId, int transitionPointId = 0)
        {
            if (_curWorld != null)
            {
                _curWorld.Exit();
            }

            _curWorld = await CreateWorld($"World_{worldId}");
            _curWorld.Enter(transitionPointId);
        }

        private async UniTask<World> CreateWorld(string name)
        {
            var path = AssetPathHelper.GetWorldPath(name);
            
            if (_dict.TryGetValue(path,out World world))
            {
                return world;
            }

            world = await AssetManager.Instance.Instantiate<World>(path);
            Object.DontDestroyOnLoad(world);
            _dict.Add(path, world);

            return world;
        }
    }
}