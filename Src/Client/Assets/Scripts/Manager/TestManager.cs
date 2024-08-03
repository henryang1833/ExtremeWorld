using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using UnityEngine;

namespace Managers
{
    class TestManager : Singleton<TestManager>
    {
        public void Init()
        {
            NPCManager.Instance.RegisterNpcEnvent(Common.Data.NPCFunctional.InvokeShop, OnNpcInvokeShop);
            NPCManager.Instance.RegisterNpcEnvent(Common.Data.NPCFunctional.InvokeInsrance, OnNpcInvokeInsrance);
        }

        private bool OnNpcInvokeShop(NPCDefine npc)
        {
            Debug.LogFormat("TestManager.OnNpcInvokeShop :NPC[{0}:{1} Type:{2} Func:{3}]",npc.ID,npc.Name,npc.Type,npc.Function);
            //UITest test = UIManager.Instance.Show<UITest>();
           
            return true;
        }

        private bool OnNpcInvokeInsrance(NPCDefine npc)
        {
            Debug.LogFormat("TestManager.OnNpcInvokeShop :NPC[{0}:{1} Type:{2} Func:{3}]", npc.ID, npc.Name, npc.Type, npc.Function);
            MessageBox.Show("点击了NPC：" + npc.Name, "NPC对话");
            return true;
        }
    }
}
