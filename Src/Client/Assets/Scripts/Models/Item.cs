using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillBridge.Message;
using Common.Data;
public class Item  {
    public int Id;
    public int Count;
    public ItemDefine define;
    public Item(NItemInfo item)
    {
        this.Id = item.Id;
        this.Count = item.Count;
        this.define = DataManager.Instance.Items[item.Id];
    }   

    public override string ToString()
    {
        return string.Format("Id:{0},Count:{1}", this.Id, this.Count);
    }
}
