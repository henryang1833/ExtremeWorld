using UnityEngine;
using UnityEngine.UI;

public class UIRideItem : ListView.ListViewItem
{
    public Image icon;
    public Text title;
    public Text levle;

    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;

    public override void onSelected(bool selected)
    {
        this.background.overrideSprite = selected ? selectedBg : normalBg;
    }

    public Item item;

    public void SetRideItem(Item item,ListView owner)
    {
        this.item = item;
        this.owner = owner;
        if (this.title != null)this.title.text = this.item.Define.Name;
        if (this.levle != null) this.levle.text = item.Define.Level.ToString();
        if (this.icon != null) this.icon.overrideSprite = Resloader.Load<Sprite>(this.item.Define.Icon);
    }
}

