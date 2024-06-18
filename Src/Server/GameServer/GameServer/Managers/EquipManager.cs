using Common;
using GameServer.Services;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    public class EquipManager : Singleton<EquipManager>
    {

        public Result EquipItem(NetConnection<NetSession> sender, int slot, int itemId, bool isEquip)
        {
            if (!sender.Session.Character.ItemManager.Items.ContainsKey(itemId))
                return Result.Failed;
            UpdateEquip(sender.Session.Character.Data.Equips, slot, itemId, isEquip);
            DBService.Instance.Save();
            return Result.Success;
        }

        unsafe void UpdateEquip(byte[] equips, int slot, int itemId, bool isEquip)
        {
            fixed(byte* pt = equips)
            {
                int* slotid = (int*)(pt + slot * sizeof(int));
                if (isEquip)
                    *slotid = itemId;
                else
                    *slotid = 0;
            }
        }
    }
}
