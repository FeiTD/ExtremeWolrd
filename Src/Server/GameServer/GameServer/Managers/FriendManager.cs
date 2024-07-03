using Common;
using GameServer.Entities;
using GameServer.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    public class FriendManager
    {
        public Character Character;
        public List<NFriendInfo> friends = new List<NFriendInfo>();
        private bool friendChanged;

        public FriendManager(Character character)
        {
            Character = character;
            InitFriends();
        }

        public NFriendInfo GetFriendInfo(int id)
        {
            return friends.Find( p => p.Id == id );
        }

        public void GetFriendInfos(List<NFriendInfo> friendInfos)
        {
            foreach (var friend in friends)
            {
                friendInfos.Add(friend);
            }
        }

        internal void AddFriend(Character friend)
        {
            TChracterFriend tf = new TChracterFriend()
            {
                FriendID = friend.Id,
                FriendName = friend.Data.Name,
                Class = friend.Data.Class,
                Level = friend.Data.Level,
            };
            this.Character.Data.Friends.Add(tf);
            friendChanged = true;
        }

        internal bool RemoveFriendById(int id)
        {
            var removeItem = Character.Data.Friends.FirstOrDefault(x => x.Id == id);
            if(removeItem != null)
            {
                DBService.Instance.Entities.TChracterFriends.Remove(removeItem);
            }
            friendChanged = true;
            return true;
        }

        internal bool RemoveFriendByFriendId(int friendId)
        {
            var removeItem = Character.Data.Friends.FirstOrDefault(x => x.FriendID == friendId);
            if (removeItem != null)
            {
                DBService.Instance.Entities.TChracterFriends.Remove(removeItem);
            }
            friendChanged = true;
            return true;
        }

        internal void PostProcess(NetMessageResponse message)
        {
            if (friendChanged)
            {
                InitFriends();
                if(message.friendList == null)
                {
                    message.friendList = new FriendListResponse();
                    message.friendList.Friends.AddRange(this.friends);
                }
                friendChanged = false;
            }
        }

        private void InitFriends()
        {
            friends.Clear();
            foreach(var friend in this.Character.Data.Friends)
            {
                friends.Add(AddFriendInfo(friend));
            }
        }

        private NFriendInfo AddFriendInfo(TChracterFriend friend)
        {
            NFriendInfo info = new NFriendInfo();
            info.Id = friend.FriendID;
            info.Status = 1;
            info.friendInfo = new NCharacterInfo()
            {
                Id = friend.FriendID,
                Class = (CharacterClass)friend.Class,
                Level = (int)friend.Level,
                Name = friend.FriendName,
            };
            return info;
        }

        internal void UpdateFriendInfo(NCharacterInfo info, int status)
        {
            foreach(var f in friends)
            {
                if(f.friendInfo.Id ==  info.Id)
                {
                    f.Status = status;
                    break;
                }
            }
            friendChanged = true;
        }
    }
}
