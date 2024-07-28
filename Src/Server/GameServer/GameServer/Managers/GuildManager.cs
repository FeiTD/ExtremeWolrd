using Common;
using GameServer.Entities;
using GameServer.Models;
using GameServer.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    public class GuildManager:Singleton<GuildManager>
    {
        public Dictionary<int,Guild> Guilds = new Dictionary<int,Guild>();

        public GuildManager()
        {
            InitGuild();
        }

        internal bool CheckNameExisted(string guildName)
        {
            foreach(var kv in Guilds)
            {
                if (guildName == kv.Value.Name)
                    return true;
            }
            return false;
        }

        internal void CreatGuild(string guildName, string guildNotice, Character character)
        {
            var dbguild =  DBService.Instance.Entities.TGuild.Create();
            dbguild.LeaderID = character.Id;
            dbguild.Name = guildName;
            dbguild.Notice = guildNotice;
            dbguild.CreatTime = DateTime.Now;
            dbguild.LeaderName = character.Name;
            DBService.Instance.Entities.TGuild.Add(dbguild);

            Guild guild = new Guild(dbguild);


            guild.AddMember(character.Id,character.Name,character.Data.Class,(int)character.Data.Level,GuildTitle.President);
            guild.GuildInfo(character);
            character.Guild = guild;
            character.Data.GuildId = dbguild.Id;
            Guilds.Add(character.Id, guild);
            DBService.Instance.Save();
        }

        void InitGuild()
        {
            foreach (var item in DBService.Instance.Entities.TGuild)
            {
                Guilds.Add(item.Id, new Guild(item));
            }
        }

        internal Guild GetGuild(int charid)
        {
            foreach(var kv in Guilds)
            {
                foreach(var item in kv.Value.Data.Members)
                {
                    if (item.Id == charid)
                        return kv.Value;
                }
            }
            return null;
        }

        internal List<NGuildInfo> GetGuildsInfo(Character character)
        {
            List<NGuildInfo> result = new List<NGuildInfo>();
            foreach(var kv in Guilds)
            {
                result.Add(kv.Value.GuildInfo(character));
            }
            return result;
        }
    }
}
