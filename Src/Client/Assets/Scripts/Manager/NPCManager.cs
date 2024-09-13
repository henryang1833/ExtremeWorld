using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Data;
using System;

namespace Managers
{
    public class NPCManager : Singleton<NPCManager>
    {
        public delegate bool NpcActionHandler(NPCDefine npc);

        Dictionary<NPCFunctional, NpcActionHandler> eventMap = new Dictionary<NPCFunctional, NpcActionHandler>();
        public void RegisterNpcEnvent(NPCFunctional function,NpcActionHandler action)
        {
            if (!eventMap.ContainsKey(function))
                eventMap[function] = action;   
            else
                eventMap[function] += action;
        }
       
        public NPCDefine GetNPCDefine(int npcID)
        {
            return DataManager.Instance.NPCs[npcID];
        }

        public bool Interactive(NPCDefine npc)
        {

            if (DoTaskInateractive(npc))
            {
                return true;
            }
            else if (npc.Type == NPCType.Functional)
            {
                return DoFunctionInateractive(npc);
            }
            return false;
        }

        private bool DoFunctionInateractive(NPCDefine npc)
        {
            if(npc.Type != NPCType.Functional)           
                return false;
            
            if (!eventMap.ContainsKey(npc.Function))
                return false;


            foreach (NpcActionHandler f in eventMap[npc.Function].GetInvocationList())
                f.Invoke(npc);
            return true;
            //return eventMap[npc.Function](npc);
        }

        private bool DoTaskInateractive(NPCDefine npc)
        {
            var status = QuestManager.Instance.GetQuestStatusByNpc(npc.ID);
            if (status == NpcQuestStatus.None)
                return false;
            return QuestManager.Instance.OpenNpcQuest(npc.ID);
        }

        internal bool Interactive(int npcId)
        {
            if (DataManager.Instance.NPCs.ContainsKey(npcId))
            {
                var npc = DataManager.Instance.NPCs[npcId];
                return Interactive(npc);
            }
            return false;
        }
    }
}

