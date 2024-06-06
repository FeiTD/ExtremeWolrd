using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Models
{
    public struct BagItem
    {
        public ushort ItemId;
        public ushort Count;

        public static BagItem zero = new BagItem() { Count = 0,ItemId = 0};

        public static bool operator ==(BagItem left, BagItem right)
        {
            return left.ItemId == right.ItemId && left.Count == right.Count;
        }

        public static bool operator !=(BagItem left, BagItem right)
        {
            return !(left == right);
        }
    }
}
