using Assets.Scripts.Manager;
using Assets.Scripts.Models;
using Assets.Scripts.UI;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

namespace Assets.Scripts.Services
{
    public class FriendService:Singleton<FriendService>,IDisposable
    {
        public UnityAction OnFriendUpdate;
        public FriendService()
        {
            MessageDistributer.Instance.Subscribe<FriendAddRequest>(this.OnFriendAddReq);
            MessageDistributer.Instance.Subscribe<FriendAddResponse>(this.OnFriendAddRes);
            MessageDistributer.Instance.Subscribe<FriendListResponse>(this.OnFriendList);
            MessageDistributer.Instance.Subscribe<FriendRemoveResponse>(this.OnFriendRemove);
        }

        private void OnFriendRemove(object sender, FriendRemoveResponse message)
        {
            if (message.Result == Result.Success)
                MessageBox.Show("删除成功");
            else
                MessageBox.Show("删除失败");
        }

        private void OnFriendList(object sender, FriendListResponse message)
        {
            FriendManager.Instance.friends = message.Friends;
            if (OnFriendUpdate != null)
                OnFriendUpdate();
        }

        private void OnFriendAddRes(object sender, FriendAddResponse message)
        {
            MessageBox.Show(message.Errormsg);
        }

        private void OnFriendAddReq(object sender, FriendAddRequest message)
        {
            var confirm = MessageBox.Show(string.Format("{0} 请求加你为好友", message.FromName), "好友请求", MessageBoxType.Confirm, "接受", "拒绝");
            confirm.OnYes = () =>
            {
                SendFriendAddResponse(true, message);
            };
            confirm.OnNo = () =>
            {
                SendFriendAddResponse(false, message);
            };
        }

        public void Init()
        {

        }

        public void SendFriendAdd(int id)
        {
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.friendAddReq = new FriendAddRequest();
            message.Request.friendAddReq.ToId = id;
            message.Request.friendAddReq.FromId = Users.Instance.CurrentCharacter.Id;
            message.Request.friendAddReq.FromName = Users.Instance.CurrentCharacter.Name;
            NetClient.Instance.SendMessage(message);
        }

        public void SendFriendRemove(int id)
        {
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.friendRemove = new FriendRemoveRequest();
            message.Request.friendRemove.Id = Users.Instance.CurrentCharacter.Id;
            message.Request.friendRemove.friendId = id;
            NetClient.Instance.SendMessage(message);
        }

        public void SendFriendAddResponse(bool accept, FriendAddRequest request)
        {
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.friendAddRes = new FriendAddResponse();
            message.Request.friendAddRes.Errormsg = accept ? "对方接受了你的好友请求" : "对方拒绝了你的好友请求";
            message.Request.friendAddRes.Result = accept ? Result.Success : Result.Failed;
            message.Request.friendAddRes.Request = request;
            NetClient.Instance.SendMessage(message);
        }
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<FriendAddRequest>(this.OnFriendAddReq);
            MessageDistributer.Instance.Unsubscribe<FriendAddResponse>(this.OnFriendAddRes);
            MessageDistributer.Instance.Unsubscribe<FriendListResponse>(this.OnFriendList);
            MessageDistributer.Instance.Unsubscribe<FriendRemoveResponse>(this.OnFriendRemove);
        }
    }
}
