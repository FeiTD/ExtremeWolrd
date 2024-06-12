using Assets.Scripts.UI;
using Common;
using Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Manager
{
    public class TestManager:Singleton<TestManager>
    {
        public void Init()
        {
            NpcManager.Instance.RegisterNpcFunction(NpcFunction.InvokeShop, OnNpcInvokeShop);
            NpcManager.Instance.RegisterNpcFunction(NpcFunction.InvokeInsrance, OnNpcInvokeInsrance);
        }

        private bool OnNpcInvokeInsrance(NpcDefine npc)
        {
            var test = UIManager.Instance.Show<UITips>();
            test.Describtion.text = "sssssss";
            return true;
        }

        private bool OnNpcInvokeShop(NpcDefine npc)
        {
            //var test = UIManager.Instance.Show<UITips>();
            //test.Describtion.text = "aaaaaa";
            return true;
        }
    }
}
