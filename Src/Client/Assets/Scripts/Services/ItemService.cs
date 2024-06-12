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
        }


        private void OnItemBuy(object sender, ItemBuyResponse message)
        {
            MessageBox.Show("购买结果：" + message.Result.ToString());
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<ItemBuyResponse>(this.OnItemBuy);
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
    }
}
