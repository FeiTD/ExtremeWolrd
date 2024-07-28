using Assets.Scripts.Manager;
using Assets.Scripts.UI;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine.Events;

namespace Assets.Scripts.Services
{
    public class GuildService:Singleton<GuildService>,IDisposable
    {
        public UnityAction<List<NGuildInfo>> OnGuildListResult;

        public UnityAction<bool> OnGuildCreat;

        public UnityAction OnGuildUpdate;
        public GuildService() 
        {
            MessageDistributer.Instance.Subscribe<GuildCreateResponse>(this.OnGuildCreateResponse);
            MessageDistributer.Instance.Subscribe<GuildListResponse>(this.OnGuildListResponse);
            MessageDistributer.Instance.Subscribe<GuildJoinResponse>(this.OnGuildJoinResponse);
            MessageDistributer.Instance.Subscribe<GuildJoinRequest>(this.OnGuildJoinRequest);
            MessageDistributer.Instance.Subscribe<GuildResponse>(this.OnGuildResponse);
            MessageDistributer.Instance.Subscribe<GuildLeaveResponse>(this.OnGuildLeaveResponse);
            MessageDistributer.Instance.Subscribe<GuildAdminResponse>(this.OnGuildAdminResponse);
        }


        public void Init()
        {

        }

        private void OnGuildLeaveResponse(object sender, GuildLeaveResponse message)
        {
            if (message.Result == Result.Success)
                GuildManager.Instance.Init(null);
            MessageBox.Show(message.Errormsg);
        }

        private void OnGuildResponse(object sender, GuildResponse message)
        {
            GuildManager.Instance.Init(message.Guildinfo);
            if (OnGuildUpdate != null)
                OnGuildUpdate();
        }

        private void OnGuildJoinRequest(object sender, GuildJoinRequest message)
        {
            var mes = MessageBox.Show(string.Format("{0}申请加入工会", message.Apply.Name), "", MessageBoxType.Confirm, "接受", "拒绝");
            mes.OnYes = () =>
            {
                SendGuildJoinResponse(true, message);
            };
            mes.OnNo = () =>
            {
                SendGuildJoinResponse(false, message);
            };
        }

        private void OnGuildJoinResponse(object sender, GuildJoinResponse message)
        {
            MessageBox.Show(message.Errormsg);
        }

        private void OnGuildListResponse(object sender, GuildListResponse message)
        {
            if (OnGuildListResult != null)
                OnGuildListResult(message.Guilds);
        }

        private void OnGuildCreateResponse(object sender, GuildCreateResponse message)
        {
            if (OnGuildListResult != null) OnGuildCreat(message.Result == Result.Success);
            if(message.Result == Result.Success)
            {
                GuildManager.Instance.Init(message.Guildinfo);
                MessageBox.Show("工会创建成功！");
            }
            else
                MessageBox.Show("工会创建失败！");
        }


        private void OnGuildAdminResponse(object sender, GuildAdminResponse message)
        {
            throw new NotImplementedException();
        }

        public void SendGuildCreat(string name,string notice)
        {
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.Guildcreat = new GuildCreateRequest();
            message.Request.Guildcreat.GuildName = name;
            message.Request.Guildcreat.GuildNotice = notice;
            NetClient.Instance.SendMessage(message);
        }

        public void SendGuildJoinRequest(int guildId)
        {
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.gulidJoinReq = new GuildJoinRequest();
            message.Request.gulidJoinReq.Apply = new NGuildApplyInfo();
            message.Request.gulidJoinReq.Apply.guiId = guildId;
            NetClient.Instance.SendMessage(message);
        }

        public void SendGuildJoinResponse(bool accept,GuildJoinRequest request)
        {
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildJoinRes = new GuildJoinResponse();
            message.Request.guildJoinRes.Result = accept ? Result.Success : Result.Failed;
            message.Request.guildJoinRes.Apply = request.Apply;
            message.Request.guildJoinRes.Apply.Result = accept ? ApplyResult.Accept : ApplyResult.Reject;
            NetClient.Instance.SendMessage(message);
        }

        public void SendGuildLeaveRequest()
        {
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildLeave = new GuildLeaveRequest();
            NetClient.Instance.SendMessage(message);
        }


        internal void SendGuildJoinApply(bool accept, NGuildApplyInfo info)
        {
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildJoinRes = new GuildJoinResponse();
            message.Request.guildJoinRes.Result = Result.Success;
            message.Request.guildJoinRes.Apply = info;
            message.Request.guildJoinRes.Apply.Result = accept ? ApplyResult.Accept : ApplyResult.Reject;
            NetClient.Instance.SendMessage(message);
        }


        public void SendAdminCommand(GuildAdminCommand command,int characterId)
        {
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildAdmin = new GuildAdminRequest();
            message.Request.guildAdmin.Command = command;
            message.Request.guildAdmin.Target = characterId;
            NetClient.Instance.SendMessage(message);
        }

        public void SendGuildListReq()
        {
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildList = new GuildListRequest();
            NetClient.Instance.SendMessage(message);
        }

        public void SendGuildRequest()
        {
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.Guild = new GuildRequest();
            NetClient.Instance.SendMessage(message);
        }
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<GuildCreateResponse>(this.OnGuildCreateResponse);
            MessageDistributer.Instance.Unsubscribe<GuildListResponse>(this.OnGuildListResponse);
            MessageDistributer.Instance.Unsubscribe<GuildJoinResponse>(this.OnGuildJoinResponse);
            MessageDistributer.Instance.Unsubscribe<GuildJoinRequest>(this.OnGuildJoinRequest);
            MessageDistributer.Instance.Unsubscribe<GuildResponse>(this.OnGuildResponse);
            MessageDistributer.Instance.Unsubscribe<GuildLeaveResponse>(this.OnGuildLeaveResponse);
        }

    }
}
