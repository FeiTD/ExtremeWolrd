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
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapEntitySyncRequest>(this.OnMapEntitySync);
            
        }
        public void Init()
        {
            MapManager.Instance.Init();
        }

        internal void SendEntityUpdate(NetConnection<NetSession> conn, NEntitySync entitySync)
        {
            conn.Session.Response.mapEntitySync = new MapEntitySyncResponse();
            conn.Session.Response.mapEntitySync.entitySyncs.Add(entitySync);
            conn.SendResponse();
        }
        private void OnMapEntitySync(NetConnection<NetSession> sender, MapEntitySyncRequest request)
        {
            Character character = sender.Session.Character;

            MapManager.Instance[character.Info.mapId].UpdateEntity(request.entitySync);
        }
    }
}
