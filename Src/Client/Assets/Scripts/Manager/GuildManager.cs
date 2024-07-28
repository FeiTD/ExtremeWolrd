using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Manager
{
    internal class GuildManager:Singleton<GuildManager>
    {
        public NGuildInfo Guild;
        public bool HasGuild {
            get { return Guild != null; }
        }
        public GuildManager()
        {
            
        }

        internal void Init(NGuildInfo guild)
        {
            Guild = guild;

        }

        internal NGuildMemberInfo GetMemberInfo(NCharacterInfo currentCharacter)
        {
            if (Guild != null)
            {
                foreach(var item in Guild.Members)
                {
                    if(currentCharacter.Id == item.characterId)
                    {
                        return item;
                    }
                }
            }
            return null;
        }
    }
}
