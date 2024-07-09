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
    public class TeamService:Singleton<TeamService>
    {
        public TeamService() 
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamInviteRequest>(this.OnTeamInviteReq);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamInviteResponse>(this.OnTeamInviteRes);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamLeaveRequest>(this.OnTeamLeaveReq);
        }
        public void Init()
        {

        }

        private void OnTeamLeaveReq(NetConnection<NetSession> sender, TeamLeaveRequest request)
        {
            Character character = sender.Session.Character;
            int leader = character.Id;
            var members = character.TeamInfo.Members;
            foreach (var member in members)
            {
                if (leader == character.Id)
                {
                    leader = member.Id;
                }
                var memberssesion = SessionManager.Instance.GetSession(member.Id);
                if(memberssesion != null)
                {
                    memberssesion.Session.Character.TeamManager.RemoveMember(request.CharacterId);
                    memberssesion.Session.Response.teamLeave = new TeamLeaveResponse();
                    memberssesion.Session.Response.teamLeave.characterId = character.Id;
                    memberssesion.Session.Response.teamLeave.Result = Result.Success;
                    memberssesion.SendResponse();
                }

            }
            sender.Session.Response.teamLeave = new TeamLeaveResponse();
            sender.Session.Response.teamLeave.Result = Result.Success;
            sender.SendResponse();
        }

        private void OnTeamInviteRes(NetConnection<NetSession> sender, TeamInviteResponse response)
        {
            NetConnection<NetSession> friend = null;
            friend = SessionManager.Instance.GetSession(response.Request.FromId);
            if(response.Result == Result.Success)
            {
                if (friend == null)
                {
                    sender.Session.Response.teamInviteRes = new TeamInviteResponse();
                    sender.Session.Response.teamInviteRes.Result = Result.Failed;
                    sender.Session.Response.teamInviteRes.Errormsg = "对方已经下线";
                }
                else
                {
                    sender.Session.Character.TeamManager.AddTeam(response.Request.FromId, response.Request.FromId, friend.Session.Character);
                    friend.Session.Character.TeamManager.team.Members.Clear();
                    foreach (var member in sender.Session.Character.TeamManager.team.Members)
                    {
                        friend.Session.Character.TeamManager.AddTeam(response.Request.FromId, response.Request.FromId,member);
                    }
                    friend.Session.Response.teamInviteRes = response;
                    sender.Session.Response.teamInviteRes = new TeamInviteResponse();
                    sender.Session.Response.teamInviteRes.Result = Result.Success;
                    sender.Session.Response.teamInviteRes.Errormsg = "组队成功";
                    friend.SendResponse();
                }
            }
            else
            {
                sender.Session.Response.teamInviteRes = new TeamInviteResponse();
                sender.Session.Response.teamInviteRes.Result = Result.Failed;
                sender.Session.Response.teamInviteRes.Errormsg = "组队失败";
            }
            sender.SendResponse();
        }

        private void OnTeamInviteReq(NetConnection<NetSession> sender, TeamInviteRequest request)
        {
            Character character = sender.Session.Character;
            NetConnection<NetSession> friend = null;
            friend = SessionManager.Instance.GetSession(request.ToId);
            if(friend == null)
            {
                sender.Session.Response.teamInviteRes = new TeamInviteResponse();
                sender.Session.Response.teamInviteRes.Result = Result.Failed;
                sender.Session.Response.teamInviteRes.Errormsg = "对方已经下线";
                sender.SendResponse();
                return;
            }
            else
            {
                friend.Session.Response.teamInviteReq = request;
                friend.SendResponse();
            }
        }
    }
}
