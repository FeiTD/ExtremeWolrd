using Assets.Scripts.Manager;
using Assets.Scripts.Models;
using Assets.Scripts.UI;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Services
{
    public class ItemService : Singleton<ItemService>,IDisposable
    {
        public ItemService()
        {
            MessageDistributer.Instance.Subscribe<ItemBuyResponse>(this.OnItemBuy);
            MessageDistributer.Instance.Subscribe<ItemEquipResopnse>(this.OnItemEquip);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<ItemBuyResponse>(this.OnItemBuy);
            MessageDistributer.Instance.Unsubscribe<ItemEquipResopnse>(this.OnItemEquip);
        }

        private void OnItemBuy(object sender, ItemBuyResponse message)
        {
            MessageBox.Show("购买结果：" + message.Result.ToString());
        }


        public void SendBuyItem(int shopId, int shopItemId)
        {
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.itemBuy = new ItemBuyRequest();
            message.Request.itemBuy.shopId = shopId;
            message.Request.itemBuy.shopItemId = shopItemId;
            NetClient.Instance.SendMessage(message);
        }

        bool isEquip = false;
        Item pendingItem = null;
        private void OnItemEquip(object sender, ItemEquipResopnse resopnse)
        {
            if (resopnse.Result == Result.Failed)
                return;
            if(pendingItem != null)
            {
                if (this.isEquip)
                    EquipManager.Instance.OnEquipItem(pendingItem);
                else
                    EquipManager.Instance.OnUnEquipItem(pendingItem.EquipDefine.Slot);
                pendingItem = null;
            }
        }

        public bool SendEquipItem(Item euip,bool isEquip) 
        {
            if (pendingItem != null)
                return false;
            pendingItem = euip;
            this.isEquip = isEquip;
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.itemEquip = new ItemEquipRequest();
            message.Request.itemEquip.Slot = (int)euip.EquipDefine.Slot;
            message.Request.itemEquip.itemId = euip.Id;
            message.Request.itemEquip.isEquip = isEquip;
            NetClient.Instance.SendMessage(message);
            return true;
        }

    }
}
