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
    }
}
