using System;
using Akari.GfCore;
using Cysharp.Threading.Tasks;

namespace GameMain.Runtime
{
    public interface IBattleCharacterFactory : IDisposable
    {
        UniTask<GfEntity> CreateUserCharacter(GameCharacterModel gameCharacterModel,
            GfFloat3 position,
            GfQuaternion rotation);
        
        UniTask<GfEntity> CreateEnemyCharacter(GameCharacterModel gameCharacterModel,
             GfFloat3 position,
             GfQuaternion rotation,
             string enemyKey
        );
        
        UniTask<GfEntity> CreateSummonerCharacter(GameCharacterModel gameCharacterModel,
            GfFloat3 position,
            GfQuaternion rotation,
            string summonerKey
        );
        
        UniTask<GfEntity> CreateNpcCharacter(GameCharacterModel gameCharacterModel,
            GfFloat3 position,
            GfQuaternion rotation,
            string npcKey
        );
    }
}