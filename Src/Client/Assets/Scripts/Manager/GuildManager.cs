using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Manager
{
    internal class GuildManager:Singleton<GuildManager> 
    {
        public List<NGuildInfo> AllGuilds;

        public NGuildInfo Guild;

        public GuildManager()
        {
            
        }

        internal void CreatGuild(string text1, string text2, NCharacterInfo currentCharacter)
        {
            throw new NotImplementedException();
        }

        internal void Init(NGuildInfo guild)
        {
            if(guild == null)
            {
                return;
            }
            Guild = new NGuildInfo();
            Guild.Id = guild.Id;
            Guild.LeaderId = guild.LeaderId;
            Guild.creatTime = guild.creatTime;
            Guild.GuildName = guild.GuildName;
            Guild.Notice = guild.Notice;
            Guild.leaderName = guild.leaderName;
            Guild.memberCount = guild.memberCount;
            foreach (var g in guild.Applies)
            {
                Guild.Applies.Add(g);
            }
            foreach(var m in guild.Members)
            {
                Guild.Members.Add(m);
            }
        }
    }
}
