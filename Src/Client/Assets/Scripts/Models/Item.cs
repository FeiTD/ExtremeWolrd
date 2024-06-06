using Assets.Scripts.Manager;
using Common.Data;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Models
{
    public class Item
    {
        public int Id;
        public int Count;
        private NItemInfo info;
        public ItemDefine Define;
        public Item(NItemInfo item)
        {
            Id = item.Id;
            Count = item.Count;
            info = item;
            Define = DataManager.Instance.Items[item.Id];
        }
    }
}
