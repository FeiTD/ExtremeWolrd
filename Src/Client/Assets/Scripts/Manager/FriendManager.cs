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
            this.friends.Clear();
            foreach(var friend in friends)
            {
                this.friends.Add(friend);
            }
            
        }
    }
}
