using Common;
using GameServer.Entities;
using GameServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    public class SpawnManager
    {
        public Map Map;
        private List<Spawner> Rules = new List<Spawner>();
        public void Init(Map map)
        {
            Map = map;
            if(DataManager.Instance.SpawnRules.ContainsKey(map.Define.ID))
            {
                foreach(var define in DataManager.Instance.SpawnRules[map.Define.ID].Values)
                {
                    Rules.Add(new Spawner(define, Map));
                }
            }
        }

        internal void Update()
        {
            if (Rules.Count == 0)
                return;
            for(int i = 0;i < Rules.Count;i++)
            {
                Rules[i].Update();
            }
        }
    }
}
