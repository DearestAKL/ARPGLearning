using System;
using System.Collections.Generic;
using Akari.GfCore;
using Akari.GfGame;
using Akari.GfUnity;
using UnityEngine;

namespace GameMain.Runtime
{
    /// <summary>
    /// 伤害预警管理系统
    /// </summary>
    public class BattleDamageWarningComponent : AGfGameComponent<BattleDamageWarningComponent>
    {
        private readonly List<WarningData> _warningData = new List<WarningData>();
        private readonly List<WarningData> _removeList = new List<WarningData>();
        
        public override void OnAwake()
        {
            Entity.On<GfActiveChangedRequest>(OnGfActiveChangedRequest);
            Entity.On<CreatDamageWarningRequest>(OnCreatDamageWarningRequest);
        }

        public override void OnDelete()
        {
            Clear();
            base.OnDelete();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_warningData.IsEmpty())
            {
                return;
            }
            
            _removeList.Clear();
            foreach (var warningData in _warningData)
            {
                warningData.OnUpdate(deltaTime);
                if (warningData.IsCompleted)
                {
                    _removeList.Add(warningData);
                }
            }

            foreach (var remove in _removeList)
            {
                _warningData.Remove(remove);
            }
        }
        
        private void OnCreatDamageWarningRequest(in CreatDamageWarningRequest request)
        {
            WarningData warningData = new WarningData(request.WarningTime);
            _warningData.Add(warningData);
            
            //表现object
            CreateWarningInstance(warningData, request.Position, request.Extents, request.Rotation);
        }

        private async void CreateWarningInstance(WarningData warningData, GfFloat2 position, GfFloat2 extents,GfQuaternion rotation)
        {
            //表现object
            if (Math.Abs(extents.Y) < 0.01f) 
            {
                //Circle
                var warningCircle = await AssetManager.Instance.Instantiate<WarningCircle>(AssetPathHelper.GetDamageWarningPath("WarningCircle"));
                warningCircle.SetData(warningData, extents.X);
                warningCircle.transform.position = new Vector3(position.X, 0, position.Y);
            }
            else
            {
                var warningRectangle= await AssetManager.Instance.Instantiate<WarningRectangle>(AssetPathHelper.GetDamageWarningPath("WarningRectangle"));
                warningRectangle.SetData(warningData, extents.X, extents.Y);
                
                warningRectangle.transform.rotation = rotation.ToQuaternion();

                var positionOffset = rotation * extents;
                warningRectangle.transform.position = new Vector3(position.X + positionOffset.X, 0, position.Y + positionOffset.Y);
            }
        }

        private void OnGfActiveChangedRequest(in GfActiveChangedRequest request)
        {
            if (!request.Enable)
            {
                Clear();
            }
        }

        private void Clear()
        {
            foreach (var warningData in _warningData)
            {
                warningData.IsCompleted = true;
            }
            _warningData.Clear();
        }
    }
    
    public class WarningData
    {
        private float _warningTime;
        private float _elapsedTime;
        public float Rate { get; private set; }
        public bool IsCompleted{ get; set; }

        public WarningData(float warningTime)
        {
            _warningTime = warningTime;

            _elapsedTime = 0;
            IsCompleted = false;
        }

        public void OnUpdate(float deltaTime)
        {
            if (IsCompleted)
            {
                return;
            }
            
            _elapsedTime += deltaTime;
            if (_elapsedTime > _warningTime)
            {
                Rate = 1F;
                IsCompleted = true;
            }
            else
            {
                Rate = _elapsedTime / _warningTime;
            }
        }
    }
    
    public readonly struct CreatDamageWarningRequest : IGfRequest
    {
        public          GfRunTimeTypeId RttId => GfRunTimeTypeOf<CreatDamageWarningRequest>.Id;
        public readonly float WarningTime;
        public readonly GfFloat2 Position;
        public readonly GfFloat2 Extents;
        public readonly GfQuaternion Rotation;

        public CreatDamageWarningRequest(float warningTime, GfFloat2 position, GfFloat2 extents,GfQuaternion rotation)
        {
            WarningTime = warningTime;
            Position = position;
            Extents = extents;
            Rotation = rotation;
        }
    }
}