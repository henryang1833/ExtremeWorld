using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using SkillBridge.Message;
using System;

namespace Managers
{
    public interface IEntityNotify
    {
        void OnEntityRemoved();
        void OnEntityChanged(Entity entity);
        void OnEntityEvent(EntityEvent @event);
    }
    public class EntityManager : Singleton<EntityManager>
    {
        Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
        Dictionary<int, IEntityNotify> notifers = new Dictionary<int, IEntityNotify>();

        public void RegisterEntityChangeNotify(int entityId,IEntityNotify notify)
        {
            this.notifers[entityId] = notify;
        }

        public void AddEntity(Entity entity)
        {
            entities[entity.entityId] = entity;
        }

        public void RemoveEntity(NEntity entity)
        {
            this.entities.Remove(entity.Id);
            if (notifers.ContainsKey(entity.Id))
            {
                notifers[entity.Id].OnEntityRemoved();
                notifers.Remove(entity.Id);
            }
        }

        internal void OnEntitySycn(NEntitySync data)
        {
            Entity entity = null;
            entities.TryGetValue(data.Id, out entity);
            if (entity != null)
            {
                if (data.Entity != null)
                    entity.EntityData = data.Entity;
                if (notifers.ContainsKey(data.Id))
                {
                    notifers[entity.entityId].OnEntityChanged(entity);
                    notifers[entity.entityId].OnEntityEvent(data.Event);
                }
            }
        }
    }
}

