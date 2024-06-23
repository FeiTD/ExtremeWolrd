using Assets.Scripts.Manager;
using Common.Data;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Models
{
    /// <summary>
    /// 任务
    /// </summary>
    public class Quest
    {
        public QuestDefine Define;
        public NQuestInfo Info;
        public Quest()
        {
                
        }

        public Quest(NQuestInfo info)
        {
            Info = info;
            Define = DataManager.Instance.Quests[info.QuestId];
        }

        public Quest(QuestDefine define)
        {
            Define = define;
            Info = null;
        }
    }
}
