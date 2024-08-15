using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;
namespace Common.Data
{
    public class EquipDefine
    {
        public int ID { get; set; }
        public EquipSlot Slot { get; set; }
        public string Category { get; set; }
        public float STR { get; set; } //力量
        public float INT { get; set; } //智力
        public float DEX { get; set; } //敏捷
        public float HP { get; set; }
        public float MP { get; set; }
        public float AD { get; set; }
        public float AP { get; set; }
        public float DEF { get; set; } //物防
        public float MDEF { get; set; } //法防
        public float SPD { get; set; } //攻速
        public float CRI { get; set; } //暴击
    }
}
