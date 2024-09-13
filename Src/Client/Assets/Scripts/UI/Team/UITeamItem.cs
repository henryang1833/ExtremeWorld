using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillBridge.Message;
public class UITeamItem : ListView.ListViewItem {

    public Text nickName;
    public Image classIcon;
    public Image leaderIcon;
    public Image background;
    public NCharacterInfo Info;
    public override void onSelected(bool selected)
    {
        this.background.enabled = selected ? true : false;
    }

    public int idx;

    private void Start()
    {
        this.background.enabled = false;
    }

    public void SetMemberInfo(int idx, NCharacterInfo info,bool isLeader)
    {
        this.idx = idx;
        this.Info = info;
        if (this.nickName != null) this.nickName.text = this.Info.Level.ToString().PadRight(4) + this.Info.Name;
        if (this.classIcon != null) this.classIcon.overrideSprite = SpriteManager.Instance.classIcons[(int)this.Info.Class - 1];
        if (this.leaderIcon != null) this.leaderIcon.gameObject.SetActive(isLeader);
    }
}
