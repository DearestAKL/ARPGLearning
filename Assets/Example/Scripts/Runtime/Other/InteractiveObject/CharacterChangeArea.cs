using Akari.GfCore;
using UnityEngine;

namespace GameMain.Runtime
{
    /// <summary>
    /// 角色切换区域
    /// 切换完成，改变表现，关闭交互
    /// </summary>
    public class CharacterChangeArea : AClickInteractiveObject
    {
        public int characterId;
        public GameObject goWeapon;
        public GameObject goVfx;
        public float rotationSpeed = 50f;
        public override string InteractionTips => "更换";
        
        private bool _hasSubscribed = false;
        
        private void Awake()
        {
            Create();
        }

        public void Update()
        {
            if (goWeapon != null)
            {
                goWeapon.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            }

            if (!_hasSubscribed)
            {
                Create();
            }
        }

        protected override async void OnInteracting(CharacterInteractive characterInteractive)
        {
            //切换控制角色
            var lastPlayer = BattleAdmin.Player;
            var lastCharacterId = lastPlayer.Condition.CharacterId;
            await UIHelper.StartLoading();

            GfFloat3 position = lastPlayer.Entity.Transform.Position;
            GfQuaternion rotation = lastPlayer.Entity.Transform.Rotation;

            var entity = await BattleAdmin.Factory.Character.CreateUserCharacter(
                new GameCharacterModel(RuntimeDataHelper.CreateCharacterData(characterId)), 
                position,
                rotation);
            
            lastPlayer.Entity.Delete();
            BattleAdmin.PlayerEntity = entity;
            
            characterInteractive.RemoveInteractiveObject(this);
            ResetStateData();
            
            //UIBattle更新
            //摄像机目标更新
            //其它角色切换区域同步更新
            var curCharacterId = BattleAdmin.Player.Condition.CharacterId;
            EventManager.Instance.BattleEvent.OnChangeCharacterEvent.Invoke(lastCharacterId, curCharacterId);
            
            UIHelper.EndLoading();
        }
        
        private void OnChangeCharacterEvent(int lastCharacterId, int curCharacterId)
        {
            if (characterId == lastCharacterId)
            {
                //可切换状态
                goVfx.SetActive(true);
                SetCanInteract(true);
            }
            else if (characterId == curCharacterId)
            {
                //不可切换状态
                goVfx.SetActive(false);
                SetCanInteract(false);
            }
        }

        private void Create()
        {
            if (!BattleAdmin.HasInstance)
            {
                return;
            }
            
            _hasSubscribed = true;
            EventManager.Instance.BattleEvent.OnChangeCharacterEvent.GfSubscribe(OnChangeCharacterEvent);
            OnChangeCharacterEvent(0, BattleAdmin.Player.Condition.CharacterId);
        }
    }
}