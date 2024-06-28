using Common;
using GameServer.Entities;
using GameServer.Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    public class MonsterManager
    {
        Map map;
        public Dictionary<int,Monster> Monsters = new Dictionary<int,Monster>();
        public void Init(Map map)
        {
            this.map = map;
        }

        public Monster Creat(int spawnMonID,int spawnLevel,NVector3 position,NVector3 direction)
        {
            Monster monster = new Monster(spawnMonID, spawnLevel, position, direction);
            EntityManager.Instance.AddEntity(this.map.ID, monster);
            monster.Info.Id = monster.entityId;
            monster.Info.mapId = map.ID;
            Monsters[monster.Id] = monster;

            map.MonsterEnter(monster);

            return monster;
        }
    }
}
