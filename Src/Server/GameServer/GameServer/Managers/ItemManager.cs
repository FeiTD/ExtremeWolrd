using GameServer.Entities;
using GameServer.Models;
using GameServer.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    public class ItemManager
    {
        Character Owner;
        public Dictionary<int, Item> Items = new Dictionary<int, Item>();
        public ItemManager(Character owner)
        {
            this.Owner = owner;
            foreach(var item in owner.Data.Items) 
            {
                this.Items.Add(item.ItemID,new Item(item));
            }
        }
        public bool UseItem(int itemID,int count = 1)
        {
            Item item = null;
            if(this.Items.TryGetValue(itemID, out item))
            {
                if (item.Count < count)
                    return false;
                item.Remove(count);
                return true;
            }
            return false;
        }

        public bool HasItem(int itemID)
        {
            Item item = null;
            if(Items.TryGetValue(itemID, out item))
                return item.Count > 0;
            return false;
        }

        public Item GetItem(int itemID)
        {
            Item item = null;
            Items.TryGetValue(itemID, out item);
            return item;
        }

        public bool AddItem(int itemID,int count)
        {
            Item item = null;
            if (Items.TryGetValue(itemID,out item))
            {
                item.Add(count);
            }
            else
            {
                TCharacterItem dbItem = new TCharacterItem();
                dbItem.Owner = Owner.Data;
                dbItem.ItemID = itemID;
                dbItem.CharacterID = Owner.Data.ID;
                dbItem.ItemCount = count;
                Owner.Data.Items.Add(dbItem);
                item = new Item(dbItem);
                Items.Add(itemID, item);
            }
            //DBService.Instance.Save();
            this.Owner.StatusManager.AddItemChanged(itemID,count,StatusAction.Add);
            return true;
        }

        public bool RemoveItem(int itemID,int count)
        {
            if(!Items.ContainsKey(itemID)) return false;
            Item item = Items[itemID];
            if (item.Count < count) return false;
            item.Remove(count);
            //DBService.Instance.Save();
            this.Owner.StatusManager.AddItemChanged(itemID, count, StatusAction.Delete);
            return true;
        }

        public void GetItemInfos(List<NItemInfo> list)
        {
            foreach(var item in this.Items) 
            {
                list.Add(new NItemInfo()
                {
                    Id = item.Value.ItemID,
                    Count = item.Value.Count,

                });
            }
        }
    }
}
