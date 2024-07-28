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
    internal class GuildService: Singleton<GuildService>
    {

        public GuildService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildCreateRequest>(this.OnGuildCreateRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildJoinResponse>(this.OnGuildJoinResponse);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildListRequest>(this.OnGuildListRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildRequest>(this.OnGuildRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildJoinRequest>(this.OnGuildJoinRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildLeaveRequest>(this.OnGuildLeaveRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildAdminRequest>(this.OnGuildAdminRequest);
        }

        private void OnGuildAdminRequest(NetConnection<NetSession> sender, GuildAdminRequest message)
        {
            Character character = sender.Session.Character;
            sender.Session.Response.guildAdmin = new GuildAdminResponse();
            if(character.Guild == null)
            {
                sender.Session.Response.guildAdmin.Result = Result.Failed;
                sender.Session.Response.guildAdmin.Errormsg = "请先加入公会";
                sender.SendResponse();
                return;
            }
            character.Guild.ExecuteAdmin(message.Command, message.Target, character.Id);
            var target = SessionManager.Instance.GetSession(message.Target);
            if(target != null)
            {
                target.Session.Response.guildAdmin = new GuildAdminResponse();
                target.Session.Response.guildAdmin.Result = Result.Success;
                target.Session.Response.guildAdmin.Command = message;
                target.SendResponse();
            }
            sender.Session.Response.guildAdmin = new GuildAdminResponse();
            sender.Session.Response.guildAdmin.Command = message;
            sender.SendResponse();
        }

        public void Init()
        {

        }
        private void OnGuildLeaveRequest(NetConnection<NetSession> sender, GuildLeaveRequest message)
        {
            Character character = sender.Session.Character;
            sender.Session.Response.guildLeave = new GuildLeaveResponse();

            character.Guild.Leave(character);

            sender.Session.Response.guildLeave.Result = Result.Success;
            DBService.Instance.Save();
            sender.SendResponse();
        }

        private void OnGuildJoinRequest(NetConnection<NetSession> sender, GuildJoinRequest message)
        {
            Character character = sender.Session.Character;
            var guild = GuildManager.Instance.GetGuild(message.Apply.guiId);
            if (guild == null)
            {
                sender.Session.Response.guildJoinRes = new GuildJoinResponse();
                sender.Session.Response.guildJoinRes.Errormsg = "公会不存在";
                sender.Session.Response.guildJoinRes.Result = Result.Failed;
                sender.SendResponse();
                return;
            }
            message.Apply.characterId = character.Data.ID;
            message.Apply.Name = character.Name;
            message.Apply.Level = (int)character.Data.Level;
            message.Apply.Class = character.Data.Class;

            if (guild.JoinApply(message.Apply))
            {
                var leader = SessionManager.Instance.GetSession(guild.Data.LeaderID);
                if(leader != null)
                {
                    leader.Session.Response.gulidJoinReq = message;
                    leader.SendResponse();
                } 
            }
            else
            {
                sender.Session.Response.guildJoinRes = new GuildJoinResponse();
                sender.Session.Response.guildJoinRes.Result = Result.Failed;
                sender.Session.Response.guildJoinRes.Errormsg = "请勿重复申请";
                sender.SendResponse();
            }
        }

        private void OnGuildRequest(NetConnection<NetSession> sender, GuildRequest message)
        {
            Character character = sender.Session.Character;
            sender.Session.Response.Guild = new GuildResponse();
            sender.Session.Response.Guild.Guildinfo = character.Guild.GuildInfo(character);
            sender.SendResponse();
        }

        private void OnGuildListRequest(NetConnection<NetSession> sender, GuildListRequest message)
        {
            Character character = sender.Session.Character;
            sender.Session.Response.guildList = new GuildListResponse();
            sender.Session.Response.guildList.Guilds.AddRange(GuildManager.Instance.GetGuildsInfo(character));
            sender.Session.Response.guildList.Result = Result.Success;
            sender.SendResponse();
        }

        private void OnGuildJoinResponse(NetConnection<NetSession> sender, GuildJoinResponse message)
        {
            Character character = sender.Session.Character;
            var guild = GuildManager.Instance.GetGuild(message.Apply.guiId);
            if(message.Result == Result.Success)
            {
                if (guild.JoinAppove(message.Apply))
                {
                    var requester = SessionManager.Instance.GetSession(message.Apply.characterId);
                    if (requester != null)
                    {
                        requester.Session.Character.Guild = guild;
                        requester.Session.Response.guildJoinRes = message;
                        requester.Session.Response.guildJoinRes.Result = Result.Success;
                        requester.Session.Response.guildJoinRes.Errormsg = "加入成功";
                        requester.SendResponse();
                    }
                };
            }
        }

        private void OnGuildCreateRequest(NetConnection<NetSession> sender, GuildCreateRequest message)
        {
            Character character = sender.Session.Character;
            sender.Session.Response.Guildcreat = new GuildCreateResponse();
            if(character.Guild != null)
            {
                sender.Session.Response.Guildcreat.Result = Result.Failed;
                sender.Session.Response.Guildcreat.Errormsg = "已经有工会了";
                sender.SendResponse();
                return;
            }
            if (GuildManager.Instance.CheckNameExisted(message.GuildName))
            {
                sender.Session.Response.Guildcreat.Result = Result.Failed;
                sender.Session.Response.Guildcreat.Errormsg = "工会名称已经存在";
                sender.SendResponse();
                return;
            }

            GuildManager.Instance.CreatGuild(message.GuildName, message.GuildNotice, character);
            sender.Session.Response.Guildcreat.Result = Result.Success;
            sender.Session.Response.Guildcreat.Guildinfo = character.Guild.GuildInfo(character);
            sender.SendResponse();
        }

    }
}
