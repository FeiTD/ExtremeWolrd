using GameServer.Core;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Entities
{
    /// <summary>
    /// Character
    /// 玩家角色类
    /// </summary>
    public class Character : CharacterBase, IPostResponser
    {
        public TCharacter Data;
        private CharacterType player;
        private TCharacter cha;

        public Character(Vector3Int pos, Vector3Int dir) : base(pos, dir)
        {
        }

        public Character(CharacterType type, TCharacter cha) :
            base(new Core.Vector3Int(cha.MapPosX, cha.MapPosY, cha.MapPosZ), new Core.Vector3Int(100, 0, 0))
        { }

        public void PostProcess(NetMessageResponse message)
        {
            throw new NotImplementedException();
        }
    }
}
