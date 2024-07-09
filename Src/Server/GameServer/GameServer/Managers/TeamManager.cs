using Common;
using GameServer.Entities;
using GameServer.Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    public class TeamManager
    {
        public Character owner;

        public Team team;

        bool teamchanged;

        public void GetTeamInfo(ref NTeamInfo info)
        {
            info = new NTeamInfo()
            {
                TeamId = team.TeamId,
                Leader = team.Leader,
            };
            foreach(var m in team.Members)
            {
                info.Members.Add(new NCharacterInfo()
                {
                    Id = m.Info.Id,
                    Class = m.Info.Class,
                    Level = m.Info.Level,
                    Name = m.Info.Name,
                });
            }
        }

        public TeamManager(Character Character)
        {
            owner = Character;
            InitTeam();
        }

        public void InitTeam()
        {
            team = new Team();
            team.TeamId = owner.Id;
            team.Leader = owner.Id;
            team.Members.Add(owner);
        }
        public void AddTeam(int leader,int teamid, Character member)
        {
            team.Leader = leader;
            team.TeamId = teamid;
            team.Members.Add(member);
            this.GetTeamInfo(ref owner.TeamInfo);
            teamchanged = true;
        }

        internal void PostProcess(NetMessageResponse message)
        {
            if(teamchanged)
            {
                if(message.teamInfo == null)
                {
                    message.teamInfo = new TeamInfoResponse();
                    message.teamInfo.Team = new NTeamInfo();
                    message.teamInfo.Team.TeamId = team.TeamId;
                    message.teamInfo.Team.Leader = team.Leader;
                    foreach (var member in this.team.Members)
                    {
                        message.teamInfo.Team.Members.Add(new NCharacterInfo()
                        {
                            Id = member.Id,
                            Name = member.Name,
                            Class = member.Info.Class,
                            Level = member.Info.Level,
                        });
                    }
                }
                teamchanged = false;
            }
        }

        internal void RemoveMember(int id)
        {
            teamchanged = true;
            if (team.Members.Count == 1)
            {
                return;
            }
            Character member = null;
            if(id == owner.Id)
            {
                team.Members.Clear();
                InitTeam();
                return;
            }
            foreach (var m in team.Members)
            {
                if (m.Id == id)
                {
                    member = m;
                }
            }
            team.Members.Remove(member);
            this.GetTeamInfo(ref owner.TeamInfo);
        }

        internal void UpdateTeamInfo()
        {
            foreach(var member in team.Members)
            {
                NetConnection<NetSession> memb = null;
                memb = SessionManager.Instance.GetSession(member.Id);
                if(memb != null)
                {
                    memb.Session.Character.TeamManager.RemoveMember(owner.Id);
                }
            }
            teamchanged = true;
        }
    }
}
