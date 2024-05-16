using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    public class MapService : Singleton<MapService>
    {
        public MapService()
        {
           
        }

        public void Init()
        {
            MapManager.Instance.Init();
        }

    }
}
