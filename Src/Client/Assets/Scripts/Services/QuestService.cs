using Assets.Scripts.Manager;
using Assets.Scripts.Models;
using Assets.Scripts.UI;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Services
{
    public class QuestService:Singleton<QuestService>,IDisposable
    {
        public void Init()
        {
            MessageDistributer.Instance.Subscribe<QuestAcceptResponse>(this.OnAcceptQuest);
            MessageDistributer.Instance.Subscribe<QuestSubmitResponse>(this.OnSubmitQuest);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<QuestAcceptResponse>(this.OnAcceptQuest);
            MessageDistributer.Instance.Unsubscribe<QuestSubmitResponse>(this.OnSubmitQuest);
        }

        public void SenQuestAccept(Quest quest)
        {
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.questAccept = new QuestAcceptRequest();
            message.Request.questAccept.QuestId = quest.Define.ID;
            NetClient.Instance.SendMessage(message);
        }

        public void SendQuestSubmit(Quest quest)
        {
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.questSubmit = new QuestSubmitRequest();
            message.Request.questSubmit.QuestId = quest.Define.ID;
            NetClient.Instance.SendMessage(message);
        }

        private void OnSubmitQuest(object sender, QuestSubmitResponse message)
        {
            if(message.Result == Result.Success)
            {
                QuestManager.Instance.OnQuestSubmited(message.Quest);
            }
            else
            {
                MessageBox.Show("任务完成失败", "错误", MessageBoxType.Error);
            }
        }

        private void OnAcceptQuest(object sender, QuestAcceptResponse message)
        {
            if(message.Result == Result.Success)
            {
                QuestManager.Instance.OnQuestAccepted(message.Quest);
            }
            else
            {
                MessageBox.Show("任务接受失败", "错误", MessageBoxType.Error);
            }
        }


    }
}
