using Assets.Scripts.UI;
using Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Type = Common.Data.Type;

namespace Assets.Scripts.Manager
{
    public class NpcManager:Singleton<NpcManager>
    {
        public delegate bool NpcActionHandler(NpcDefine npc); 
        Dictionary<NpcFunction, NpcActionHandler> NpcEvents = new Dictionary<NpcFunction, NpcActionHandler>();
        public void RegisterNpcFunction(NpcFunction func,NpcActionHandler action)
        {
            if (!NpcEvents.ContainsKey(func))
            {
                NpcEvents[func] = action;
            }
            else
            {
                NpcEvents[func] += action;
            }
        }

        public bool Interactive(int ID)
        {
            DataManager.Instance.Load();
            if (DataManager.Instance.Npcs.ContainsKey(ID))
            {
                var npc = DataManager.Instance.Npcs[ID];
                return Interactive(npc);
            }
            return false;
        }

        private bool Interactive(NpcDefine npc)
        {
            if(npc.Type == Type.Task)
            {
                return DoTaskInteractive(npc);
            }
            else if(npc.Type == Type.Functional)
            {
                return DoFunctionInteractive(npc);
            }
            return false;
        }

        private bool DoFunctionInteractive(NpcDefine npc)
        {
            if (!NpcEvents.ContainsKey(npc.Function))
            {
                return false;
            }
            return NpcEvents[npc.Function](npc);
        }

        private bool DoTaskInteractive(NpcDefine npc)
        {
            var Status = QuestManager.Instance.GetNpcQuestStatus(npc.ID);
            if (Status == NpcQuestStatus.None)
                return false;
            return QuestManager.Instance.OpenNpcQuest(npc.ID);
        }

        public NpcDefine GetNpcDefine(int ID)
        {
            DataManager.Instance.Load();
            if (DataManager.Instance.Npcs.ContainsKey(ID))
            {
                return DataManager.Instance.Npcs[ID];
            }
            return null;
        }
    }
}
