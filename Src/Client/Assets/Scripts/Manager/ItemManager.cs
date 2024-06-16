using Assets.Scripts.Models;
using Assets.Scripts.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Manager
{
    public class ItemManager : Singleton<ItemManager>
    {
        public Dictionary<int, Item> Items = new Dictionary<int, Item>();
        public void Init(List<NItemInfo> items)
        {
           Items.Clear();
            foreach(var info in items)
            {
                Item item = new Item(info);
                Items.Add(item.Id, item);
            }
            StatusService.Instance.RegisterStatusNotify(StatusType.Item, OnItemNotify);
        }

        private bool OnItemNotify(NStatus status)
        {
            if (status.Action == StatusAction.Add)
                this.AddItem(status.Id, status.Value);
            if(status.Action == StatusAction.Delete)
                this.RemoveItem(status.Id,status.Value);
            return true;
        }

        private void RemoveItem(int id, int value)
        {
            if (Items.ContainsKey(id)){
                if (Items[id].Count >= value)
                {
                    Items[id].Count -= value;
                    BagManager.Instance.RemoveItem(id, value);
                }       
            }
        }

        private void AddItem(int id, int value)
        {
            if (Items.ContainsKey(id))
            {
                Items[id].Count += value;
            }
            else
            {
                Item item = new Item(id, value);
                Items.Add(id, item);
            }
            BagManager.Instance.AddItem(id, value);
            //BagManager.Instance.Reset();
        }
    }
}
