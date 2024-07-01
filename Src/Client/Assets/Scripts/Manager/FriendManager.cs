using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Manager
{
    public class FriendManager:Singleton<FriendManager>
    {
        public List<NFriendInfo> friends = new List<NFriendInfo>();
        public void Init(List<NFriendInfo> friends)
        {
            friends.Clear();
            this.friends = friends;
        }
    }
}
