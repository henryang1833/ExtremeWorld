using Managers;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestSystem : UIWindow{

    public Text title;

    public GameObject itemPrefab;

    public TabView Tabs;
    public ListView listMain;
    public ListView listBranch;

    public UIQuestInfo questInfo;

    private bool showAvailableList = false;

    private void Start()
    {
        this.listMain.onItemSelected += this.OnQuestSelected;
        this.listBranch.onItemSelected += this.OnQuestSelected;
        this.Tabs.OnTabSelect += OnSelectTab;
        QuestManager.Instance.onQuestStatusChanged += OnQuestStatusChanged;
    }

    private void OnQuestStatusChanged(Quest quest)
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        ClearAllQuestList();
        InitAllQuestItems();
        if (!this.listMain.SelectFirstItem() && !this.listBranch.SelectFirstItem())//默认选中一个进行显示
            this.questInfo.SetEmpty();                
    }

    private void InitAllQuestItems()
    {
        foreach(var kv in Managers.QuestManager.Instance.allQuests)
        {
            if (showAvailableList) //可接任务列表
            {
                if (kv.Value.Info != null) //已接取过
                    continue;
            }
            else //正在进行中任务列表
            {
                if (kv.Value.Info == null) //未接取
                    continue;
                if (kv.Value.Info.Status == SkillBridge.Message.QuestStatus.Finished) //已提交的
                    continue;
            }

            GameObject go = Instantiate(itemPrefab, kv.Value.Define.Type == Common.Data.QuestType.Main ? this.listMain.transform : this.listBranch.transform);
            UIQuestItem ui = go.GetComponent<UIQuestItem>();
            ui.SetQuestInfo(kv.Value);

            if (kv.Value.Define.Type == Common.Data.QuestType.Main)
                this.listMain.AddItem(ui as ListView.ListViewItem);
            else
                this.listBranch.AddItem(ui as ListView.ListViewItem);
        }
    }

    private void ClearAllQuestList()
    {
        this.listMain.RemoveAll();
        this.listBranch.RemoveAll();
    }

    private void OnSelectTab(int idx)
    {
        showAvailableList = idx == 1;
        RefreshUI();
    }

    public void OnQuestSelected(ListView.ListViewItem item)
    {
        //自己添加的///////////////////////
        if (item.owner == this.listMain)
            this.listBranch.DisableSelected();
        else
            this.listMain.DisableSelected();
        ///////////////////////////////////

        UIQuestItem questItem = item as UIQuestItem;
        this.questInfo.SetQuestInfo(questItem.quest,false);
    }

    private void OnDestroy()
    {
        this.listMain.onItemSelected -= this.OnQuestSelected;
        this.listBranch.onItemSelected -= this.OnQuestSelected;
        this.Tabs.OnTabSelect -= OnSelectTab;
        QuestManager.Instance.onQuestStatusChanged -= OnQuestStatusChanged;
    }
}
