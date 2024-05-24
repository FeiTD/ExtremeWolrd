using Assets.Scripts.Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Manager
{
    public interface IEntityNotify
    {
        void OnEntityRemove();
        void OnEntityChanged(Entity entity1);
        void OnEntityEvent(EntityEvent @event);
    }
    public class EntityManager:Singleton<EntityManager>
    {
        Dictionary <int,Entity> entities = new Dictionary <int,Entity> ();

        Dictionary<int,IEntityNotify> notifiers = new Dictionary <int,IEntityNotify>();

        public void RegisterEntityChangedNotify(int entityId,IEntityNotify notify)
        {
            notifiers[entityId] = notify;
        }

        public void AddEntity(Entity entity)
        {
            entities[entity.entityId] = entity;
        }

        public void RemoveEntity(NEntity entity)
        {
            entities.Remove(entity.Id);
            if (notifiers.ContainsKey(entity.Id))
            {
                notifiers[entity.Id].OnEntityRemove();
                notifiers.Remove(entity.Id);
            }
        }

        internal void OnEntitySync(NEntitySync entity)
        {
            Entity entity1 = null;
            entities.TryGetValue(entity.Id, out entity1);
            if(entity1 != null)
            {
                if(entity.Entity != null)
                {
                    entity1.EntityData = entity.Entity;
                }
                if (notifiers.ContainsKey(entity.Id))
                {
                    notifiers[entity1.entityId].OnEntityChanged(entity1);
                    notifiers[entity1.entityId].OnEntityEvent(entity.Event);
                }
            }
        }
    }
}
