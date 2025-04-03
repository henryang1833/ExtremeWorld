using System.Collections.Generic;
using Common;
using GameServer.Entities;
namespace GameServer.Managers
{
    class EntityManager : Singleton<EntityManager>
    {
        private int idx = 0;
        public List<Entity> AllEntites = new List<Entity>();
        public Dictionary<int, List<Entity>> MapEntities = new Dictionary<int, List<Entity>>();

        public void AddEntity(int mapId, Entity entity)
        {
            AllEntites.Add(entity);

            entity.EntityData.Id = ++this.idx;

            List<Entity> entities = null;
            if(!MapEntities.TryGetValue(mapId,out entities))
            {
                entities = new List<Entity>();
                MapEntities[mapId] = entities;
            }
            entities.Add(entity);
        }

        public void RemoveEntity(int mapId, Entity entity)
        {
            this.AllEntites.Remove(entity);
            this.MapEntities[mapId].Remove(entity);
        }
    }
}
