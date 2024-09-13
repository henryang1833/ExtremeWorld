using System;
using UnityEngine;
using UnityEngine.UI;
using Models;
using Common.Data;
using Managers;
public class UIQuestInfo : MonoBehaviour
{
    public Text title;
    public Text[] targets;
    public Text description;
    public Transform[] rewardSlots; //自己改的，添加物品的槽
    public GameObject IconItemPrefab;
    public Text rewardMoney;
    public Text rewardExp;
    private UIIconItem[] uiIconItems = new UIIconItem[3];

    //todo  面板清空，什么也不展示,自己写的
    public void SetEmpty()
    {
        foreach(Transform child in transform)
            child.gameObject.SetActive(false);
    }

    public void SetQuestInfo(Quest quest, bool isQuestDialog)
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(true);
        
        this.title.text = string.Format("[{0}]{1}", quest.Define.Type, quest.Define.Name);
        this.targets[0].text = GetFormatTargetText(quest.Define.Target1, quest.Define.Target1Num, quest.Define.Target1ID);
        this.targets[1].text = GetFormatTargetText(quest.Define.Target2, quest.Define.Target2Num, quest.Define.Target2ID);
        this.targets[2].text = GetFormatTargetText(quest.Define.Target3, quest.Define.Target3Num, quest.Define.Target3ID);
        if (isQuestDialog)
        {
            if (quest.Info == null) //未领取的任务
                this.description.text = quest.Define.Dialog;
            else if (quest.Info.Status == SkillBridge.Message.QuestStatus.Completed) //已完成的任务
                this.description.text = quest.Define.DialogFinish;
        }
        else
            this.description.text = quest.Define.Overview;

        this.rewardMoney.text = quest.Define.RewardGold.ToString();
        this.rewardExp.text = quest.Define.RewardExp.ToString();


        ItemDefine itemDefine1;
        if (DataManager.Instance.Items.TryGetValue(quest.Define.RewardItem1, out itemDefine1))
        {
            rewardSlots[0].gameObject.SetActive(true);
            if (uiIconItems[0] == null)
            {
                GameObject go = Instantiate(IconItemPrefab, rewardSlots[0]);
                uiIconItems[0] = go.GetComponent<UIIconItem>();
            }
            uiIconItems[0].SetMainIcon(itemDefine1.Icon, string.Format("x{0}", quest.Define.RewardItem1Count));
        }
        else
            rewardSlots[0].gameObject.SetActive(false);

        ItemDefine itemDefine2;
        if (DataManager.Instance.Items.TryGetValue(quest.Define.RewardItem2, out itemDefine2))
        {
            rewardSlots[1].gameObject.SetActive(true);
            if (uiIconItems[1] == null)
            {
                GameObject go = Instantiate(IconItemPrefab, rewardSlots[1]);
                uiIconItems[1] = go.GetComponent<UIIconItem>();
            }
            uiIconItems[1].SetMainIcon(itemDefine2.Icon, string.Format("x{0}", quest.Define.RewardItem2Count));
        }
        else
            rewardSlots[1].gameObject.SetActive(false);

        ItemDefine itemDefine3;
        if (DataManager.Instance.Items.TryGetValue(quest.Define.RewardItem3, out itemDefine3))
        {
            rewardSlots[2].gameObject.SetActive(true);
            if (uiIconItems[2] == null)
            {
                GameObject go = Instantiate(IconItemPrefab, rewardSlots[2]);
                uiIconItems[2] = go.GetComponent<UIIconItem>();
            }
            uiIconItems[2].SetMainIcon(itemDefine3.Icon, string.Format("x{0}", quest.Define.RewardItem3Count));
        }
        else
            rewardSlots[2].gameObject.SetActive(false);

        foreach (var filter in this.GetComponentsInChildren<ContentSizeFitter>())
            filter.SetLayoutVertical(); //刷新布局
    }

    //自己写的
    //根据任务目标产生显示的字符串
    private string GetFormatTargetText(QuestTarget target,int num,int id)
    {
        string res;
        if (target == QuestTarget.Kill)     
            res = string.Format("杀死{0}只{1}", num, DataManager.Instance.Characters[id].Name);  
        else if (target == QuestTarget.Item)   
            res = string.Format("取得{0}{1}", num, DataManager.Instance.Items[id].Name);     
        else
            res = "";
        return res;
    }

    public void OnClickAbandon()
    {

    }
}
