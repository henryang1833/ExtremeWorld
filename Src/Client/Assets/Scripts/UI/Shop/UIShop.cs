using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common.Data;
using System;
using Managers;

public class UIShop : UIWindow
{
    public Text title;
    public Text page1Title;
    public Text page2Title;
    public Text page3Title;

    public Text money;

    public GameObject shopItem;
    ShopDefine shop;
    public Transform[] itemRoot;

    void Start()
    {
        StartCoroutine(InitItems());
    }

    private IEnumerator InitItems()
    {
        int count = 0;
        int page = 0;

        foreach (var kv in DataManager.Instance.ShopItems[shop.ID])
        {
            if (kv.Value.Status > 0)
            {
                if (shop.ID == 2) //武器商店分职业展示
                {
                    int idx = (int)DataManager.Instance.Items[kv.Value.ItemID].LimitClass - 1;
                    GameObject go = Instantiate(shopItem, itemRoot[idx]);
                    UIShopItem ui = go.GetComponent<UIShopItem>();
                    ui.SetShopItem(kv.Key, kv.Value, this);

                    if (!itemRoot[idx].gameObject.activeSelf)
                        itemRoot[idx].gameObject.SetActive(true);
                    
                }
                else {
                    GameObject go = Instantiate(shopItem, itemRoot[page]);
                    UIShopItem ui = go.GetComponent<UIShopItem>();
                    ui.SetShopItem(kv.Key, kv.Value, this);
                    count++;
                    if (count >= 10)
                    {
                        count = 0;
                        page++;
                        itemRoot[page].gameObject.SetActive(true);
                    }
                }
            }
            yield return null;
        }
    }

    public void SetShop(ShopDefine shop)
    {
        this.shop = shop;
        this.title.text = shop.Name;
        this.money.text = Models.User.Instance.CurrentCharacter.Gold.ToString();

        if (shop.ID == 2)
        {
            page1Title.text = "战士";
            page2Title.text = "法师";
            page3Title.text = "弓箭手";

        }
        else
        {
            page1Title.text = "页1";
            page2Title.text = "页2";
            page3Title.text = "页3";
        }
    }

    private UIShopItem selectedItem;
    internal void SelectShopItem(UIShopItem item)
    {
        if (selectedItem != null)
            selectedItem.Selected = false;
        selectedItem = item;
    }

    public void OnClickBuy()
    {
        if(this.selectedItem == null)
        {
            MessageBox.Show("请选择要购买的道具", "购买提示");
            return;
        }

        if (!ShopManager.Instance.BuyItem(this.shop.ID, this.selectedItem.ShopItemID))
        {

        }
    }
}
