using Assets.Scripts.Models;
using Assets.Scripts.Services;
using Common;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Manager
{
    public class EquipManager : Singleton<EquipManager>
    {
        public delegate void EquipItemHandler();

        public EquipItemHandler OnEquipChanged;

        public Item[] Equips = new Item[(int)EquipSlot.SlotMax];

        byte[] Data;

        public void Init(byte[] data)
        {
            Data = data;
            Analyze(Data);
        }

        unsafe void Analyze(byte[] data)
        {
            fixed(byte* pt = Data)
            {
                for(int i = 0;i < Equips.Length; i++)
                {
                    int ItemId = *(int*)(pt + i * sizeof(int));
                    if (ItemId > 0)
                        Equips[i] = ItemManager.Instance.Items[ItemId];
                    else
                        Equips[i] = null;
                }
            }
        }

        unsafe byte[] GetEquipData()
        {
            fixed(byte* pt = Data)
            {
                for(int i =0;i < (int)EquipSlot.SlotMax; i++)
                {
                    int itemId = *(int*)(pt + i * sizeof(int));
                    if (Equips[i] == null)
                        itemId = 0;
                    else
                        itemId = Equips[i].Id;
                }
            } 
            return Data;
        }

        public Item GetEquip(EquipSlot slot)
        {
            return Equips[(int)slot];
        }

        public bool Contains(int equipId)
        {
            for(int i = 0;i < Equips.Length; i++)
            {
                if (Equips[i] != null && Equips[i].Id == equipId)
                    return true;
            }
            return false;
        }

        public void EquipItem(Item equip)
        {
            ItemService.Instance.SendEquipItem(equip, true);
        }

        public void UnEquipItem(Item equip)
        {
            ItemService.Instance.SendEquipItem(equip, false);
        }

        public void OnEquipItem(Item equip)
        {
            if (Equips[(int)equip.EquipDefine.Slot] != null && Equips[(int)equip.EquipDefine.Slot].Id == equip.Id)
            {
                return;
            }
            Equips[(int)equip.EquipDefine.Slot] = ItemManager.Instance.Items[equip.Id];
            if (OnEquipChanged != null)
                OnEquipChanged();
        }

        public void OnUnEquipItem(EquipSlot slot)
        {
            if (Equips[(int)slot] != null)
            {
                Equips[(int)slot] = null;
            }
            if (OnEquipChanged != null)
                OnEquipChanged();
        }
    }
}
