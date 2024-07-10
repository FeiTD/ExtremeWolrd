using GameServer.Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    internal class GuildManager
    {
        Character Character;
        public NGuildInfo Guild = new NGuildInfo();

        public GuildManager(Character owner)
        {
            Character = owner;
            InitGuild();
        }
        public void Init(NGuildInfo info)
        {

        }

        public void InitGuild()
        {
            foreach(var item in  Character.Data.Guild.Members)
            {

            }
        }
    }
}
