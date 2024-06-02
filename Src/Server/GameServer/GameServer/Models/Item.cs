﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    public class Item
    {
        TCharacterItem dbItem;

        public int ItemID;

        public int Count;
        public Item(TCharacterItem item)
        {
            dbItem = item;

            ItemID = dbItem.ItemID;
            Count = dbItem.ItemCount;
        }

        public void Add(int count)
        {
            Count += count;
            dbItem.ItemCount = Count;
        }

        public void Remove(int count) 
        { 
            Count -= count;
            dbItem.ItemCount = Count;
        }
    }
}
