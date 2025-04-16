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
    public class BattleDamageRangeComponent : AGfGameComponent<BattleDamageRangeComponent>
    {
        private readonly List<DamageRangeData> _warningData = new List<DamageRangeData>();
        private readonly List<DamageRangeData> _removeList = new List<DamageRangeData>();
        
        public override void OnAwake()
        {
            Entity.On<GfActiveChangedRequest>(OnGfActiveChangedRequest);
            Entity.On<CreateDamageRangeShowRequest>(OnCreatDamageWarningRequest);
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
            
            deltaTime *= Speed;
            
            _removeList.Clear();
            foreach (var warningData in _warningData)
            {
                warningData.OnUpdate(deltaTime);
                if (warningData.IsCompleted)
                {
                    Entity.Request(new DamageRangeShowCompletedRequest(warningData.Id));
                    _removeList.Add(warningData);
                }
            }

            foreach (var remove in _removeList)
            {
                _warningData.Remove(remove);
            }
        }
        
        private void OnCreatDamageWarningRequest(in CreateDamageRangeShowRequest request)
        {
            DamageRangeData damageRangeData = new DamageRangeData(request.Id,request.ShowTime);
            _warningData.Add(damageRangeData);
            
            //表现object
            CreateWarningInstance(damageRangeData, request.Position, request.Extents, request.Rotation);
        }

        private async void CreateWarningInstance(DamageRangeData damageRangeData, GfFloat3 position, GfFloat2 extents,GfQuaternion rotation)
        {
            //表现object
            if (Math.Abs(extents.Y) < 0.01f) 
            {
                //Circle
                var warningCircle = await AssetManager.Instance.InstantiateFormPool<DamageRangeCircle>(AssetPathHelper.GetDamageRangePath("DamageRangeCircle"));
                warningCircle.SetData(damageRangeData, extents.X);
                warningCircle.transform.position = new Vector3(position.X, position.Y, position.Z);
            }
            else
            {
                var warningRectangle= await AssetManager.Instance.InstantiateFormPool<DamageRangeRectangle>(AssetPathHelper.GetDamageRangePath("DamageRangeRectangle"));
                warningRectangle.SetData(damageRangeData, extents.X, extents.Y);
                
                warningRectangle.transform.rotation = rotation.ToQuaternion();

                var positionOffset = rotation * extents;
                warningRectangle.transform.position = new Vector3(position.X + positionOffset.X, position.Y, position.Z + positionOffset.Y);
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
    
    public class DamageRangeData
    {
        private float _showTime;
        private float _elapsedTime;
        public float Rate { get; private set; }
        public bool IsCompleted{ get; set; }

        public int Id;

        public DamageRangeData(int id,float showTime)
        {
            Id = id;
            _showTime = showTime;

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
            if (_elapsedTime > _showTime)
            {
                Rate = 1F;
                IsCompleted = true;
            }
            else
            {
                Rate = _elapsedTime / _showTime;
            }
        }
    }
    
    public readonly struct CreateDamageRangeShowRequest : IGfRequest
    {
        public          GfRunTimeTypeId RttId => GfRunTimeTypeOf<CreateDamageRangeShowRequest>.Id;
        public readonly int Id;
        public readonly float ShowTime;
        public readonly GfFloat3 Position;
        public readonly GfFloat2 Extents;
        public readonly GfQuaternion Rotation;

        public CreateDamageRangeShowRequest(int id,float showTime, GfFloat3 position, GfFloat2 extents,GfQuaternion rotation)
        {
            Id = id;
            ShowTime = showTime;
            Position = position;
            Extents = extents;
            Rotation = rotation;
        }
    }

    public readonly struct DamageRangeShowCompletedRequest : IGfRequest
    {
        public          GfRunTimeTypeId RttId => GfRunTimeTypeOf<CreateDamageRangeShowRequest>.Id;
        public readonly int Id;

        public DamageRangeShowCompletedRequest(int id)
        {
            Id = id;
        }
    }
}