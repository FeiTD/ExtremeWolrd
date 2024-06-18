using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    public class EquipService : Singleton<EquipService>
    {
        public EquipService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ItemEquipRequest>(this.OnItemEquip);

        }

        private void OnItemEquip(NetConnection<NetSession> sender, ItemEquipRequest request)
        {
            Character character = sender.Session.Character;
            var reslut = EquipManager.Instance.EquipItem(sender, request.Slot, request.itemId, request.isEquip);
            sender.Session.Response.itemEquip = new ItemEquipResopnse();
            sender.Session.Response.itemEquip.Result = reslut;
            sender.SendResponse();
        }

        public void Init()
        {

        }
    }
}
