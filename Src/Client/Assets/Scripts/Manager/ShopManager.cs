using Assets.Scripts.Services;
using Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Manager
{
    public class ShopManager:Singleton<ShopManager>
    {
        public void Init()
        {
            NpcManager.Instance.RegisterNpcFunction(NpcFunction.InvokeShop,OnOpenShop);
        }
        private bool OnOpenShop(NpcDefine npc)
        {
            this.ShowShop(npc.Param);
            return true;
        }

        public void ShowShop(int shopId)
        {
            ShopDefine shop;
            if(DataManager.Instance.Shops.TryGetValue(shopId, out shop))
            {
                UIShop uIShop = UIManager.Instance.Show<UIShop>();
                if(uIShop != null)
                {
                    uIShop.SetShop(shop);
                }
            }
        }

        public bool BuyItem(int shopId,int shopItemId)
        {
            ItemService.Instance.SendBuyItem(shopId, shopItemId);
            return true;  
        }
    }
}
