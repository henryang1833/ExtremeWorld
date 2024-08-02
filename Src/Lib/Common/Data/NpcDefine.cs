﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;

namespace Common.Data
{
    public enum NPCType
    {
        None = 0,
        Functional = 1,
        Task,
    }

    public enum NPCFunctional
    {
        None = 0,
        InvokeShop = 1,
        InvokeInsrance = 2,
    }


    public class NPCDefine
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Descript { get; set; }
        public NVector3 Position { get; set; }
        public NPCType Type { get; set; }
        public NPCFunctional Function { get; set; }
        public int Param { get; set;  }

    }
}
