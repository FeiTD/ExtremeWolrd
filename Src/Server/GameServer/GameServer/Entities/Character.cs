using Common;
using GameServer.Core;
using GameServer.Managers;
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
        public ItemManager ItemManager;
        public StatusManager StatusManager;
        public long Gold 
        { 
            get
            {
                return this.Data.Gold;
            }
            set
            {
                if (this.Data.Gold == value)
                    return;
                this.StatusManager.AddGoldChanged((int)(value - this.Data.Gold));
                this.Data.Gold = value;
            }
        }
        public Character(Vector3Int pos, Vector3Int dir) : base(pos, dir)
        {
        }

        public Character(CharacterType type, TCharacter cha) :
            base(new Core.Vector3Int(cha.MapPosX, cha.MapPosY, cha.MapPosZ), new Core.Vector3Int(100, 0, 0))
        {
            this.Data = cha;
            this.Id = cha.ID;
            this.Info = new NCharacterInfo();
            this.Info.Type = type;
            this.Info.Id = cha.ID;
            this.Info.EntityId = this.entityId;
            this.Info.Name = cha.Name;
            this.Info.Level = 1;//cha.Level;
            this.Info.Gold = cha.Gold;
            this.Info.ConfigId = cha.TID;
            this.Info.Class = (CharacterClass)cha.Class;
            this.Info.mapId = cha.MapID;
            this.Info.Entity = this.EntityData;
            this.Define = DataManager.Instance.Characters[this.Info.ConfigId];

            ItemManager = new ItemManager(this);
            ItemManager.GetItemInfos(this.Info.Items);

            this.Info.Bag = new NBagInfo();
            this.Info.Bag.Unlocked = cha.Bag.Unlocked;
            this.Info.Bag.Items = cha.Bag.Items;

            this.StatusManager = new StatusManager(this);

            this.Info.Equips = cha.Equips;
        }

        public void PostProcess(NetMessageResponse message)
        {
            Log.InfoFormat("PostProcess > Character: characterID:{0}:{1}", this.Id, this.Info.Name);
        }
    }
}
