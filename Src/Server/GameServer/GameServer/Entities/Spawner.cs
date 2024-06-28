using Common.Data;
using GameServer.Managers;
using GameServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Entities
{
    public class Spawner
    {
        private SpawnRuleDefine define;
        private Map map;

        SpawnPointDefine SpawnPoint;
        bool spawned;
        private float unspawnTime;

        public Spawner(SpawnRuleDefine define, Map map)
        {
            this.define = define;
            this.map = map;

            if (DataManager.Instance.SpawnPoints.ContainsKey(map.ID))
            {
                if (DataManager.Instance.SpawnPoints[map.ID].ContainsKey(define.SpawnPoint))
                {
                    SpawnPoint = DataManager.Instance.SpawnPoints[map.ID][define.SpawnPoint];
                }
            }
        }

        internal void Update()
        {
            if(CanSpawn())
            {
                Spawn();
            }
        }

        private void Spawn()
        {
            spawned = true;
            map.MonsterManager.Creat(define.SpawnMonID, define.SpawnLevel, SpawnPoint.Position, SpawnPoint.Direction);
        }

        private bool CanSpawn()
        {
            if (unspawnTime + define.SpawnPeriod > Time.time)
                return false;

            return true;
        }
    }
}
