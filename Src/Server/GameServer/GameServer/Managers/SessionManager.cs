using Common;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    public class SessionManager:Singleton<SessionManager>
    {
        public Dictionary<int,NetConnection<NetSession>> Sessions = new Dictionary<int, NetConnection<NetSession>>();

        internal NetConnection<NetSession> GetSession(int id)
        {
            NetConnection<NetSession> connection = null;
            Sessions.TryGetValue(id, out connection); 
            return connection;
        }

        public void AddSession(int id, NetConnection<NetSession> session)
        {
            Sessions[id] = session;
        }

        public void RemoveSession(int id)
        {
            Sessions.Remove(id);
        }
    }
}
