using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

namespace Assets.Scripts.Services
{
    public class FriendService:Singleton<FriendService>
    {
        public UnityAction OnFriendUpdate;
        public void Init()
        {

        }

        public void SendFriendAdd(int id)
        {

        }

        public void SendFriendRemove(int id)
        {

        }
    }
}
