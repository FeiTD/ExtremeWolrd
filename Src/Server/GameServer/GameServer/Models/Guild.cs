using GameServer.Entities;
using GameServer.Managers;
using GameServer.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    public class Guild
    {
        public TGuild Data;
        public int Id {
            get
            {
                return Data.Id;
            }
        }

        public string Name { 
            get
            {
                return Data.Name;
            }
        }

        public Character Leader { get; set; }

        //public List<Character> Members = new List<Character>();

        public double timestamp;

        public Guild(TGuild guild)
        {
            Data = guild;
        }

        internal NGuildInfo GuildInfo(Character character)
        {
            NGuildInfo info = new NGuildInfo()
            {
                Id = Id,
                GuildName = Name,
                Notice = Data.Notice,
                LeaderId = Data.LeaderID,
                leaderName = Data.LeaderName,
                //creatTime = (long)TimeSpan.FromDays,
                memberCount = Data.Members.Count,
            };
            
            if (character != null)
            {
                info.Members.AddRange(GetMemberInfos());
                if (character.Id == Data.LeaderID)
                    info.Applies.AddRange(GetApplyInfos());
            }
            return info;
        }

        private List<NGuildApplyInfo> GetApplyInfos()
        {
            List<NGuildApplyInfo> applies = new List<NGuildApplyInfo>();
            foreach(var item in Data.Applies)
            {
                if (item.Result != (int)ApplyResult.None)
                    continue;
                NGuildApplyInfo Info = new NGuildApplyInfo();
                Info.Class = item.Class;
                Info.Result = (ApplyResult)item.Result;
                Info.Name = item.Name;
                Info.characterId = item.CharacterId;
                Info.Level= item.Level;
                applies.Add(Info);
            }
            return applies;
        }

        private List<NGuildMemberInfo> GetMemberInfos()
        {
            List<NGuildMemberInfo> members = new List<NGuildMemberInfo>();
            foreach(var item in Data.Members)
            {
                var member = new NGuildMemberInfo()
                {
                    Id = item.Id,
                    characterId = item.CharacterId,
                    Title = (GuildTitle)Convert.ToInt32( item.Title),
                    joinTime = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds(),
                    lastTime = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds(),
                };

                var chara = CharacterManager.Instance.GetCharacter(item.CharacterId);
                if(chara != null)
                {
                    member.Info = chara.Info;
                    member.Status = 1;
                    item.Level = (int)chara.Data.Level;
                    item.Name = chara.Data.Name;
                    item.LastTime = DateTime.Now;
                    if(item.CharacterId == Data.LeaderID)
                    {
                        Leader = chara;
                    }
                }
                else
                {
                    member.Info = GetMemberInfo(item);
                    member.Status = 0;
                }
                members.Add(member);
            }
            return members;
        }

        private NCharacterInfo GetMemberInfo(TGuildMember item)
        {
            var m = new NCharacterInfo()
            {
                Id = item.Id,
                Name = item.Name,
                Level = item.Level,
                Class = (CharacterClass)item.Class,
            };
            
            return m;
        }

        internal void PostProcess(Character character, NetMessageResponse message)
        {
            if(message.Guild == null)
            {
                message.Guild = new GuildResponse();
                message.Guild.Result = Result.Success;
                message.Guild.Guildinfo = GuildInfo(character);
            }
        }

        internal bool JoinApply(NGuildApplyInfo apply)
        {
            var old = Data.Applies.FirstOrDefault(v => v.CharacterId == apply.characterId);
            if(old != null)
            {
                return false;
            }

            var dbapply = DBService.Instance.Entities.TGuilApply.Create();
            dbapply.ApplyTime = DateTime.Now;
            dbapply.CharacterId = apply.characterId;
            dbapply.GuildId = apply.guiId;
            dbapply.Name = apply.Name;
            dbapply.Level = apply.Level;
            dbapply.Class = apply.Class;

            DBService.Instance.Entities.TGuilApply.Add(dbapply);

            Data.Applies.Add(dbapply);

            DBService.Instance.Save();
            return true;
        }

        internal bool JoinAppove(NGuildApplyInfo apply)
        {
            var old = Data.Applies.FirstOrDefault(v => v.CharacterId == apply.characterId && v.Result == 0);
            if(old == null)
            {
                return false;
            }

            old.Result = (int)apply.Result;

            if(apply.Result == ApplyResult.Accept)
            {
                this.AddMember(apply.characterId,apply.Name,apply.Class,apply.Level,GuildTitle.None);
            }

            DBService.Instance.Save();

            return true;
        }

        public void AddMember(int characterId, string name, int @class, int level, GuildTitle none)
        {
            TGuildMember member = new TGuildMember()
            {
                CharacterId = characterId,
                Name = name,
                Class = @class,
                Level = level,
                Title = (int)none,
                JoinTime = DateTime.Now,
                LastTime = DateTime.Now,
            };
            Data.Members.Add(member);
            var character = CharacterManager.Instance.GetCharacter(characterId);
            if (character != null)
            {
                character.Data.GuildId = Id;
            }
            else
            {
                TCharacter dbchar = DBService.Instance.Entities.Characters.SingleOrDefault(c => c.ID == characterId);
                dbchar.GuildId = Id;
            }
        }

        internal void Leave(Character character)
        {
            Data.Members.Remove(Data.Members.FirstOrDefault(v => v.CharacterId == character.Id));
           
        }

        TGuildMember GetMember(int characterId)
        {
            foreach(var m in Data.Members)
            {
                if(m.CharacterId == characterId)
                    return m;
            }
            return null;
        }
        internal void ExecuteAdmin(GuildAdminCommand command, int targetid, int sourceid)
        {
            var target = GetMember(targetid);
            var source = GetMember(sourceid);
            switch(command)
            {
                case GuildAdminCommand.Promote:
                    target.Title = (int)GuildTitle.VicePresident; 
                    break;
                case GuildAdminCommand.Depost:
                    target.Title = (int)GuildTitle.None;
                    break;
                case GuildAdminCommand.Transfer:
                    target.Title = (int)GuildTitle.President;
                    source.Title = (int)GuildTitle.None;
                    this.Data.LeaderID = targetid;
                    this.Data.LeaderName = target.Name;
                    break;
            }
            DBService.Instance.Save();

        }
    }
}
