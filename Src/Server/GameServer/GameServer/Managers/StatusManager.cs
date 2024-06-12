using Common;
using GameServer.Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    public class StatusManager
    {
        Character Owner;
        private List<NStatus> Status { get; set; }
        public bool HasStatus => this.Status.Count > 0;
        public StatusManager(Character character)
        {
            Owner = character;
            Status = new List<NStatus>();
        }


        public void AddStatus(StatusType type,int id,int value,StatusAction action)
        {
            this.Status.Add(new NStatus()
            {
                Type = type,
                Value = value,
                Id = id,
                Action = action
            });
        }

        public void AddItemChanged(int id,int count,StatusAction action)
        {
            this.AddStatus(StatusType.Item, id, count, action);
        }

        public void AddGoldChanged(int goldDelta)
        {
            if(goldDelta > 0)
            {
                this.AddStatus(StatusType.Money, 0, goldDelta, StatusAction.Add);
            }
            if(goldDelta < 0)
            {
                this.AddStatus(StatusType.Money,0,-goldDelta, StatusAction.Delete);
            }
        }
        public void ApplyResponse(NetMessageResponse message)
        {
            if (message.statusNotify == null)
                message.statusNotify = new StatusNotify();
            foreach(var status in Status)
            {
                message.statusNotify.Status.Add(status);
            }
            Status.Clear();
        }
    }
}
