using Assets.Scripts.Models;
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
        }
    }
}
