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

        public Item(NItemInfo item)
        {
            Id = item.Id;
            Count = item.Count;
        }
    }
}
