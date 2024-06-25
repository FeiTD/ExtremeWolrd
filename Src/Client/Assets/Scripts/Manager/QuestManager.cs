using Assets.Scripts.Models;
using Assets.Scripts.Services;
using Assets.Scripts.UI;
using Common.Data;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Manager
{
    public enum NpcQuestStatus
    {
        None,
        Complate,
        Available,
        Incomplate,
    }
    public class QuestManager:Singleton<QuestManager>
    {
        public List<NQuestInfo> questInfos;
        public Dictionary<int,Quest> Quests = new Dictionary<int, Quest>();
        public Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>> npcQuests = new Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>>();
        public Action<Quest> OnQuestStatusChanged;

        public void Init(List<NQuestInfo> quests)
        {
            questInfos = quests;
            Quests.Clear();
            npcQuests.Clear();
            InitQuests();
        }

        private void InitQuests()
        {
            foreach(var info in questInfos)
            {
                Quest quest = new Quest(info);
                this.AddNpcQuest(quest.Define.AcceptNPC, quest);
                this.AddNpcQuest(quest.Define.SubmitNPC, quest);
                this.Quests.Add(info.QuestId, quest);
            }

            foreach(var kv in DataManager.Instance.Quests)
            {
                if (kv.Value.LimitClass != CharacterClass.None && kv.Value.LimitClass != Users.Instance.CurrentCharacter.Class)
                    continue;
                if (kv.Value.LimitLevel > Users.Instance.CurrentCharacter.Level)
                    continue;
                if (Quests.ContainsKey(kv.Key))
                    continue;
                if(kv.Value.PreQuest > 0)
                {
                    Quest preQuest;
                    if (this.Quests.TryGetValue(kv.Value.PreQuest, out preQuest))
                    {
                        if(preQuest.Info == null)
                            continue;
                        if(preQuest.Info.Status != QuestStatus.Finished)
                            continue;
                    }
                    else
                        continue;
                }
                Quest quest = new Quest(kv.Value);
                this.AddNpcQuest(quest.Define.AcceptNPC, quest);
                this.AddNpcQuest(quest.Define.SubmitNPC, quest);
                this.Quests[quest.Define.ID] = quest;
            }
        }

        private void AddNpcQuest(int npcId, Quest quest)
        {
            if (!npcQuests.ContainsKey(npcId))
            {
                this.npcQuests[npcId] = new Dictionary<NpcQuestStatus, List<Quest>>();
            }
            List<Quest> availables;
            List<Quest> complates;
            List<Quest> incompates;
            if (!npcQuests[npcId].TryGetValue(NpcQuestStatus.Complate,out complates))
            {
                complates = new List<Quest> { };
                npcQuests[npcId][NpcQuestStatus.Complate] = complates;
            }

            if (!npcQuests[npcId].TryGetValue(NpcQuestStatus.Incomplate, out incompates))
            {
                incompates = new List<Quest> { };
                npcQuests[npcId][NpcQuestStatus.Incomplate] = incompates;
            }

            if (!npcQuests[npcId].TryGetValue(NpcQuestStatus.Available, out availables))
            {
                availables = new List<Quest> { };
                npcQuests[npcId][NpcQuestStatus.Available] = availables;
            }


            if(quest.Info == null)
            {
                if(npcId == quest.Define.AcceptNPC && !this.npcQuests[npcId][NpcQuestStatus.Available].Contains(quest))
                {
                    npcQuests[npcId][NpcQuestStatus.Available].Add(quest);
                }
            }
            else
            {
                if(quest.Define.SubmitNPC == npcId && quest.Info.Status == QuestStatus.Complated)
                {
                    if (!npcQuests[npcId][NpcQuestStatus.Complate].Contains(quest))
                        npcQuests[npcId][NpcQuestStatus.Complate].Add(quest);
                }
                if (quest.Define.SubmitNPC == npcId && quest.Info.Status == QuestStatus.InProgress)
                {
                    if (!npcQuests[npcId][NpcQuestStatus.Incomplate].Contains(quest))
                        npcQuests[npcId][NpcQuestStatus.Incomplate].Add(quest);
                }
            }
        }

        public NpcQuestStatus GetNpcQuestStatus(int npcId)
        {
            Dictionary<NpcQuestStatus, List<Quest>> status = new Dictionary<NpcQuestStatus, List<Quest>>();
            if(npcQuests.TryGetValue(npcId,out status))
            {
                if (status[NpcQuestStatus.Complate].Count > 0)
                {
                    return NpcQuestStatus.Complate;
                }
                if (status[NpcQuestStatus.Available].Count > 0)
                {
                    return NpcQuestStatus.Available;
                }
                if (status[NpcQuestStatus.Incomplate].Count > 0)
                {
                    return NpcQuestStatus.Incomplate;
                }
            }
            return NpcQuestStatus.None;
        }

        public bool OpenNpcQuest(int npcId)
        {
            Dictionary<NpcQuestStatus, List<Quest>> status = new Dictionary<NpcQuestStatus, List<Quest>>();
            if (npcQuests.TryGetValue(npcId, out status))
            {
                if (status[NpcQuestStatus.Complate].Count > 0 )
                    return ShowQuestDialog(status[NpcQuestStatus.Complate].First());
                if (status[NpcQuestStatus.Available].Count > 0)
                    return ShowQuestDialog(status[NpcQuestStatus.Available].First());
                if (status[NpcQuestStatus.Incomplate].Count > 0)
                    return ShowQuestDialog(status[NpcQuestStatus.Incomplate].First());
            }
            return false;
        }
          
        private bool ShowQuestDialog(Quest quest)
        {
            if(quest.Info == null || quest.Info.Status == QuestStatus.Complated)
            {
                UIQuestInfo info = UIManager.Instance.Show<UIQuestInfo>();
                info.SetQuest(quest);
                info.OnClose += OnQuestDialogClose;
                return true;
            }
            if(quest.Info != null || quest.Info.Status == QuestStatus.Complated)
            {
                if (!string.IsNullOrEmpty(quest.Define.DialogIncomplete))
                    MessageBox.Show(quest.Define.DialogIncomplete);
            }
            return false;
        }

        void OnQuestDialogClose(UIWindow sender,UIWindow.WindowResult result)
        { 
            UIQuestInfo dlg = (UIQuestInfo)sender;
            if(result == UIWindow.WindowResult.Yes)
            {
                if (dlg.quest.Info == null)
                    QuestService.Instance.SenQuestAccept(dlg.quest);
                else if(dlg.quest.Info.Status == QuestStatus.Complated)
                    QuestService.Instance.SendQuestSubmit(dlg.quest);
            }
            else if(result == UIWindow.WindowResult.No)
            {
                MessageBox.Show(dlg.quest.Define.DialogDeny);
            }

        }

        public void OnQuestAccepted(NQuestInfo quest)
        {
            Quest info = this.RefreshQuestStatus(quest);
            MessageBox.Show(info.Define.DialogAccept);
        }

        private Quest RefreshQuestStatus(NQuestInfo quest)
        {
            npcQuests.Clear();
            Quest result;
            if (Quests.ContainsKey(quest.QuestId))
            {
                Quests[quest.QuestId].Info = quest;
                result = Quests[quest.QuestId];
            }
            else
            {
                result = new Quest(quest);
                Quests[quest.QuestId] = result;
            }

            foreach (var kv in DataManager.Instance.Quests)
            {
                if (kv.Value.LimitClass != CharacterClass.None && kv.Value.LimitClass != Users.Instance.CurrentCharacter.Class)
                    continue;
                if (kv.Value.LimitLevel > Users.Instance.CurrentCharacter.Level)
                    continue;
                if (Quests.ContainsKey(kv.Key))
                    continue;
                if (kv.Value.PreQuest > 0)
                {
                    Quest preQuest;
                    if (this.Quests.TryGetValue(kv.Value.PreQuest, out preQuest))
                    {
                        if (preQuest.Info == null)
                            continue;
                        if (preQuest.Info.Status != QuestStatus.Finished)
                            continue;
                    }
                    else
                        continue;
                }
                Quest info = new Quest(kv.Value);
                this.AddNpcQuest(info.Define.AcceptNPC, info);
                this.AddNpcQuest(info.Define.SubmitNPC, info);
                this.Quests[info.Define.ID] = info;
            }

            if (OnQuestStatusChanged != null)
                OnQuestStatusChanged(result);

            return result;
        }

        private void CheckAvailableQuests()
        {
            throw new NotImplementedException();
        }

        public void OnQuestSubmited(NQuestInfo quest)
        {
            Quest info = this.RefreshQuestStatus(quest);
            MessageBox.Show(info.Define.DialogFinish);
        }
    }
}
