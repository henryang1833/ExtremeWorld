﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillBridge.Message;
using System;

namespace Entities
{
    public class Entity
    {
        public int entityId;

        public Vector3Int position;
        public Vector3Int direction;
        public int speed;

        private NEntity entityData;
        public NEntity EntityData
        {
            get
            {
                UpdataEntityData();
                return entityData;
            }

            set
            {
                entityData = value;
                this.SetEntityData(value);
            }
        }

        private void UpdataEntityData()
        {
            entityData.Position.FromVector3Int(this.position);
            entityData.Direction.FromVector3Int(this.direction);
            entityData.Speed = this.speed;
        }

        public Entity(NEntity entity)
        {
            this.entityId = entity.Id;
            this.entityData = entity;
            this.SetEntityData(entity);
        }

        public virtual void OnUpdate(float delta)
        {
            if (this.speed != 0)
            {
                Vector3 dir = this.direction;
                this.position += Vector3Int.RoundToInt(dir * speed * delta / 100f); //不清楚为啥/100
            }
        }

        public void SetEntityData(NEntity entity)
        {
            this.position = this.position.FromNVector3(entity.Position);
            this.direction = this.direction.FromNVector3(entity.Direction);
            this.speed = entity.Speed;
        }
    }
}

