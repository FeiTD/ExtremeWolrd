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
    public class TeamService:Singleton<TeamService>,IDisposable
    {
        public UnityAction OnRefrenshTeam;
        public TeamService() 
        {
            MessageDistributer.Instance.Subscribe<TeamInfoResponse>(this.OnTeamInfoResponse);
            MessageDistributer.Instance.Subscribe<TeamInviteRequest>(this.OnTeamInviteRequest);
            MessageDistributer.Instance.Subscribe<TeamInviteResponse>(this.OnTeamInviteResponse);
            MessageDistributer.Instance.Subscribe<TeamLeaveResponse>(this.OnTeamLeaveResponse);
        }
        public void Init()
        {

        }

        private void OnTeamLeaveResponse(object sender, TeamLeaveResponse response)
        {
            MessageBox.Show(response.Errormsg);
        }

        private void OnTeamInviteResponse(object sender, TeamInviteResponse response)
        {
            MessageBox.Show(response.Errormsg);
        }

        private void OnTeamInviteRequest(object sender, TeamInviteRequest request)
        {
            var message = MessageBox.Show(string.Format("{0}向你发起了组队申请,是否同意？", request.FromName),"",MessageBoxType.Confirm,"同意","拒绝");
            message.OnNo = () =>
            {
                SendTeamInviteRes(false, request);
            };
            message.OnYes = () =>
            {
                SendTeamInviteRes(true, request);
            };
        }

        private void OnTeamInfoResponse(object sender, TeamInfoResponse response)
        {
            Users.Instance.GetTeam(response.Team);
            if(OnRefrenshTeam != null)
            {
                OnRefrenshTeam();
            }
        }

        public void SendTeamInviteReq(int fromid,string fromname,int toid,string toname)
        {
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.teamInviteReq = new TeamInviteRequest();
            message.Request.teamInviteReq.FromId = fromid;
            message.Request.teamInviteReq.ToId = toid;
            message.Request.teamInviteReq.FromName = fromname;
            message.Request.teamInviteReq.ToName = toname;
            NetClient.Instance.SendMessage(message);
        }

        public void SendTeamInviteRes(bool accept,TeamInviteRequest request)
        {
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.teamInviteRes = new TeamInviteResponse();
            message.Response.teamInviteRes.Result = accept ? Result.Success : Result.Failed;
            message.Response.teamInviteRes.Errormsg = accept ? "对方接受了组队请求" : "对方拒绝了组队请求";
            message.Response.teamInviteRes.Request = request;
            NetClient.Instance.SendMessage(message);
        }

        public void SendTeamLeaveReq(int teamId,int characterId)
        {
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.teamLeave = new TeamLeaveRequest();
            message.Request.teamLeave.TeamId = teamId;
            message.Request.teamLeave.CharacterId = characterId;
            NetClient.Instance.SendMessage(message);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<TeamInfoResponse>(this.OnTeamInfoResponse);
            MessageDistributer.Instance.Unsubscribe<TeamInviteRequest>(this.OnTeamInviteRequest);
            MessageDistributer.Instance.Unsubscribe<TeamInviteResponse>(this.OnTeamInviteResponse);
            MessageDistributer.Instance.Unsubscribe<TeamLeaveResponse>(this.OnTeamLeaveResponse);
        }
    }
}
