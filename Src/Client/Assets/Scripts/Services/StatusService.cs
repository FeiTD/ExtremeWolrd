using Assets.Scripts.Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Services
{
    public class StatusService:Singleton<StatusService>
    {
        public delegate bool StatusNotifyHandler(NStatus status);
        Dictionary<StatusType,StatusNotifyHandler> eventMap = new Dictionary<StatusType, StatusNotifyHandler>();
        HashSet<StatusNotifyHandler> handles = new HashSet<StatusNotifyHandler>();
        public void Init()
        {

        }

        public void RegisterStatusNotify(StatusType function,StatusNotifyHandler handler)
        {
            if (handles.Contains(handler))
                return;
            if(!eventMap.ContainsKey(function))
                eventMap[function] = handler;
            else
                eventMap[function] += handler;
            handles.Add(handler);
        }

        public StatusService()
        {
            MessageDistributer.Instance.Subscribe<StatusNotify>(this.OnStatusNotify);
        }

        private void OnStatusNotify(object sender, StatusNotify message)
        {
            foreach(NStatus status in message.Status)
            {
                Notify(status);
            }
        }

        private void Notify(NStatus status)
        {
            if(status.Type == StatusType.Money)
            {
                if (status.Action == StatusAction.Add)
                    Users.Instance.AddGold(status.Value);
                else if(status.Action == StatusAction.Delete)
                    Users.Instance.AddGold(-status.Value);
            }

            StatusNotifyHandler handler;
            if(eventMap.TryGetValue(status.Type, out handler))
            {
                handler(status);
            }
        }
    }
}
