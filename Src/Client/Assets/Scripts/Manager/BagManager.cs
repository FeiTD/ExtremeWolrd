using Assets.Scripts.Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Manager
{
    public class BagManager:Singleton<BagManager>
    {
        public int Unlocked;
        public BagItem[] Items;

        NBagInfo NBagInfo;

        unsafe public void Init(NBagInfo info)
        {
            this.NBagInfo = info;
            this.Unlocked = info.Unlocked;
            Items = new BagItem[this.Unlocked];
            if(info.Items != null && info.Items.Length >= this.Unlocked) 
            {
                Analyze(info.Items);
            }
            else
            {
                info.Items = new byte[sizeof(BagItem) * this.Unlocked];
                Reset();
            }
        }

        //整理
        public void Reset()
        {
            int i = 0;
            foreach(var kv in ItemManager.Instance.Items)
            {
                if(kv.Value.Count <= kv.Value.Define.StackLimit)
                {
                    this.Items[i].ItemId = (ushort)kv.Key;
                    this.Items[i].Count = (ushort)kv.Value.Count;
                }
                else
                {
                    int count = kv.Value.Count;
                    while(count > kv.Value.Define.StackLimit)
                    {
                        Items[i].ItemId = (ushort)kv.Key;
                        Items[i].Count = (ushort)kv.Value.Count;
                        i++;
                        count -= kv.Value.Define.StackLimit;
                    }
                    Items[i].ItemId = (ushort)kv.Key;
                    Items[i].Count = (ushort)count;
                }
                i++;
            }
        }

        unsafe public void Analyze(byte[] data)
        {
            fixed(byte* bt = data)
            {
                for(int i = 0;i < Unlocked; i++)
                {
                    BagItem* item = (BagItem*)(bt + i*sizeof(BagItem));
                    Items[i] = *item;
                }
            }
        }

        unsafe public NBagInfo GetBagInfo()
        {
            fixed (byte* bt = NBagInfo.Items)
            {
                for (int i = 0; i < Unlocked; i++)
                {
                    BagItem* item = (BagItem*)(bt + i * sizeof(BagItem));
                    *item = Items[i];
                }
            }
            return NBagInfo;
        }
    }
}
