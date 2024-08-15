using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
class UIShopItem : MonoBehaviour,ISelectHandler
{
    UIShop shop;
    public int ShopItemID { get; set; }
    ShopItemDefine ShopItem;
    ItemDefine item;

    public Text title;
    public Text limitClass;
    public Text count;
    public Text price;
    public Image icon;

    public Image background;
    public Color normalColor;
    public Color selecteColor;
    public bool selected;
    public bool Selected
    {
        get { return selected; }
        set
        {
            selected = value;
            this.background.color = selected ? selecteColor : normalColor;
        }
    }

    internal void SetShopItem(int id, ShopItemDefine shopItem, UIShop owner)
    {
        this.shop = owner;
        this.ShopItemID = id;
        this.ShopItem = shopItem;
        this.item = DataManager.Instance.Items[this.ShopItem.ItemID];

        if (this.title != null)
            this.title.text = this.item.Name;
        if (this.count != null)
            this.count.text = "x" + ShopItem.Count.ToString();
        if (this.price != null)
            this.price.text = ShopItem.Price.ToString();
        if (this.limitClass != null)
            this.limitClass.text = this.item.LimitClass.ToString();
        this.icon.overrideSprite = Resloader.Load<Sprite>(item.Icon);
    }

    public void OnSelect(BaseEventData eventData)
    {
        this.Selected = true;
        this.shop.SelectShopItem(this);
    }
}

