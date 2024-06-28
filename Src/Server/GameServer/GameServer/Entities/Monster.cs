using Common.Data;
using GameServer.Core;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Entities
{
    public class Monster:CharacterBase
    {
        public Monster(int spawnMonID, int spawnLevel, Vector3Int pos, Vector3Int dir) : base(pos, dir)
        {
            this.Info = new NCharacterInfo();
            Info.Type = CharacterType.Monster;
            Info.ConfigId = spawnMonID;
            
            Info.Level = spawnLevel;
            Info.Entity = this.EntityData;
        }
    }
}
