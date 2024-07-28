using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using SkillBridge.Message;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class Users : Singleton<Users>
    {
        SkillBridge.Message.NUserInfo userInfo;

        public MapDefine CurrentMapData { get; set; }


        public SkillBridge.Message.NUserInfo Info
        {
            get { return userInfo; }
        }

        public void SetupUserInfo(SkillBridge.Message.NUserInfo info)
        {
            this.userInfo = info;
        }

        public void AddGold(int value)
        {
            CurrentCharacter.Gold += value;
        }

        internal void InitTeam()
        {
            TeamInfo = new NTeamInfo();
            TeamInfo.TeamId = CurrentCharacter.Id;
            TeamInfo.Leader = CurrentCharacter.Id;
            TeamInfo.Members.Add(CurrentCharacter);
        }

        internal void GetTeam(NTeamInfo team)
        {
            TeamInfo = new NTeamInfo();
            TeamInfo.TeamId = team.TeamId;
            TeamInfo.Leader = team.Leader;
            foreach(var i in team.Members)
            {
                TeamInfo.Members.Add(i);
            }
        }

        public SkillBridge.Message.NCharacterInfo CurrentCharacter { get; set; }
        public PlayerInputController CurrentCharacterObject { get; set; }

        public NTeamInfo TeamInfo { get; set; }

    }
       
}
