using Common;
using GameServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    public class EntityManager : Singleton<EntityManager>
    {
        private int idx = 0;
        public List<Entity> AllEntities = new List<Entity>();
        public Dictionary<int,List<Entity>> MapEntities = new Dictionary<int,List<Entity>>();

        public void AddEntity(int mapID, Entity entity)
        {
            AllEntities.Add(entity);
            entity.EntityData.Id = ++idx;
            List<Entity> entities = null;
            if(!MapEntities.TryGetValue(mapID, out entities))
            {
                entities = new List<Entity>();
                MapEntities[mapID] = entities;
            }
            entities.Add(entity);
        }

        internal void RemoveEntity(int mapID, Entity entity)
        {
            AllEntities.Remove(entity);
            MapEntities[mapID].Remove(entity);
        }
    }
}
