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

        internal bool Interactive(NPCDefine npc)
        {
            if (npc.Type == NPCType.Task)
            {
                return DoTaskInateractive(npc);
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
            MessageBox.Show("点击了NPC：" + npc.Name, "NPC对话");
            return true;
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

