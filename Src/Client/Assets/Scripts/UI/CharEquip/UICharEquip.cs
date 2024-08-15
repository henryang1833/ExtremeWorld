using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;
public class UICharEquip : UIWindow
{
    public Text title;
    public Text money;

    public GameObject itemPrefab;
    public GameObject itemEquipedPrefab;

    public Transform itemListRoot;

    public List<Transform> slots;

    void Start()
    {
        RefreshUI();
        EquipManager.Instance.OnEquipChanged += RefreshUI;
    }

    private void OnDestroy()
    {
        EquipManager.Instance.OnEquipChanged -= RefreshUI;
    }

    void RefreshUI()
    {
        ClearAllEquipList();
        InitAllEquipItems();
        ClearEquipedList();
        InitEquipedItems();
        this.money.text = Models.User.Instance.CurrentCharacter.Gold.ToString();
    }

    private void InitEquipedItems()
    {
        for(int i = 0; i < (int)SkillBridge.Message.EquipSlot.SlotMax; ++i)
        {
            var item = EquipManager.Instance.Equips[i];

            if(item!=null)
            {
                GameObject go = Instantiate(itemEquipedPrefab, slots[i]);
                UIEquipItem ui = go.GetComponent<UIEquipItem>();
                ui.SetEquipItem(i, item, this, true);
            }
        }
    }

    private void ClearEquipedList()
    {
        foreach(var item in slots)
        {
            if (item.childCount > 0)
                Destroy(item.GetChild(0).gameObject);
        }
    }

    private void InitAllEquipItems()
    {
        foreach(var kv in ItemManager.Instance.Items)
        {
            if(kv.Value.Define.Type == SkillBridge.Message.ItemType.Equip &&kv.Value.Define.LimitClass == Models.User.Instance.CurrentCharacter.Class)
            {
                if (EquipManager.Instance.Contains(kv.Key))   //已经装备的就不要显示了
                    continue;
                GameObject go = Instantiate(itemPrefab, itemListRoot);
                UIEquipItem ui = go.GetComponent<UIEquipItem>();
                ui.SetEquipItem(kv.Key, kv.Value, this, false);
            }
        }
    }

    private void ClearAllEquipList()
    {
        foreach (var item in itemListRoot.GetComponentsInChildren<UIEquipItem>())
            Destroy(item.gameObject);
    }

    public void DoEquip(Item item)
    {
        EquipManager.Instance.EquipItem(item);
    }

    public void UnEquip(Item item)
    {
        EquipManager.Instance.UnEquipItem(item); 
    }

    private UIEquipItem selectItem;
    public void OnSelectItem(UIEquipItem item)
    {
        if (selectItem != null)
            selectItem.Selected = false;
        selectItem = item;
    }
}
