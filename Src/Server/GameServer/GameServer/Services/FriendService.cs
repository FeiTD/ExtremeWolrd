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
    public class FriendService:Singleton<FriendService>
    {
        public FriendService() 
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendAddRequest>(this.OnFriendAddRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendRemoveRequest>(this.OnFriendRemoveRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendAddResponse>(this.OnFriendAddResponse);
        }

        private void OnFriendAddResponse(NetConnection<NetSession> sender, FriendAddResponse response)
        {
            Character character = sender.Session.Character;
            if(response.Result == Result.Success)
            {
                var requester = SessionManager.Instance.GetSession(response.Request.FromId);
                if(requester == null)
                {
                    sender.Session.Response.friendAddRes.Result = Result.Failed;
                    sender.Session.Response.friendAddRes.Errormsg = "请求者已经下线";
                }
                else
                {
                    character.FriendManager.AddFriend(requester.Session.Character);
                    requester.Session.Character.FriendManager.AddFriend(character);
                    DBService.Instance.Save();
                    requester.Session.Response.friendAddRes = response;
                    requester.Session.Response.friendAddRes.Result = Result.Success;
                    requester.Session.Response.friendAddRes.Errormsg = "对方已经成为你的好友";

                    requester.SendResponse();
                      
                }
            }
            //sender.Session.Response.friendAddRes = response;
            //sender.Session.Response.friendAddRes.Result = Result.Failed;
            //sender.Session.Response.friendAddRes.Errormsg = "对方已经成为你的好友";
            //sender.SendResponse();
        }

        private void OnFriendRemoveRequest(NetConnection<NetSession> sender, FriendRemoveRequest request)
        {
            Character character = sender.Session.Character;
            sender.Session.Response.friendRemove = new FriendRemoveResponse();
            sender.Session.Response.friendRemove.Id = request.friendId;

            if (character.FriendManager.RemoveFriendById(request.friendId))
            {
                sender.Session.Response.friendRemove.Result = Result.Success;
                var friend = SessionManager.Instance.GetSession(request.friendId);
                if(friend != null)
                {
                    friend.Session.Character.FriendManager.RemoveFriendById(character.Id);
                }
                else
                {
                    RemoveFriend(request.friendId, character.Id);
                }
            }
            else
            {
                sender.Session.Response.friendRemove.Result = Result.Failed;
                //sender.Session.Response.friendRemove.Errormsg = ""
            }

            DBService.Instance.Save();
            sender.SendResponse();
        }

        private void RemoveFriend(int chid, int id)
        {
            var removeitem = DBService.Instance.Entities.TChracterFriends.FirstOrDefault(v => v.TCharacterID == chid && v.Id == id);
            if(removeitem != null)
            {
                DBService.Instance.Entities.TChracterFriends.Remove(removeitem);
            }
        }

        private void OnFriendAddRequest(NetConnection<NetSession> sender, FriendAddRequest request)
        {
            Character character = sender.Session.Character;
            if(request.ToId > 0)
            {
                foreach(var cha in CharacterManager.Instance.Characters)
                {
                    if(request.ToId == cha.Value.Id)
                    {
                        request.ToName = cha.Value.Name;
                        break;
                    }
                }
            }
            NetConnection<NetSession> friend = null;
            if(character.FriendManager.GetFriendInfo(request.ToId) != null)
            {
                sender.Session.Response.friendAddRes = new FriendAddResponse();
                sender.Session.Response.friendAddRes.Result = Result.Failed;
                sender.Session.Response.friendAddRes.Errormsg = "对方已经是好友了";
                sender.SendResponse();
                return;
            }
            friend = SessionManager.Instance.GetSession(request.ToId);
            if(friend == null)
            {
                sender.Session.Response.friendAddRes = new FriendAddResponse();
                sender.Session.Response.friendAddRes.Result = Result.Failed;
                sender.Session.Response.friendAddRes.Errormsg = "好友不存在或者不在线";
                sender.SendResponse();
                return;
            }
            friend.Session.Response.friendAddReq = request;
            friend.SendResponse();
        }

        public void Init()
        {

        }
    }
}
