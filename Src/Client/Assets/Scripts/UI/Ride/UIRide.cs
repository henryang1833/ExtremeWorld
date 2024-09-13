using Managers;
using UnityEngine;
using UnityEngine.UI;

public class UIRide : UIWindow
{
    public Text descript;
    public GameObject itemPrefab;
    public ListView listMain;
    private UIRideItem selectItem;

    void Start()
    {
        this.listMain.onItemSelected += this.OnItemSelected;
        RefreshUI();
    }

    public void OnItemSelected(ListView.ListViewItem item)
    {
        this.selectItem = item as UIRideItem;
        this.descript.text = this.selectItem.item.Define.Description;
    }

    void RefreshUI()
    {
        ClearItems();
        InitAllRideItems();
        if (!this.listMain.SelectFirstItem())//默认选中一个进行显示
            this.descript.text = "";
    }

    void ClearItems()
    {
        this.listMain.RemoveAll();
    }

    private void InitAllRideItems()
    {
        foreach (var kv in ItemManager.Instance.Items)
        {
            if (kv.Value.Define.Type == SkillBridge.Message.ItemType.Ride && (kv.Value.Define.LimitClass == Models.User.Instance.CurrentCharacter.Class
                || kv.Value.Define.LimitClass == SkillBridge.Message.CharacterClass.None))
            {
                GameObject go = Instantiate(itemPrefab, listMain.transform);
                UIRideItem ui = go.GetComponent<UIRideItem>();
                ui.SetRideItem(kv.Value, listMain);
                this.listMain.AddItem(ui);
            }
        }
    }

    public void DoRide()
    {
        if(this.selectItem == null)
        {
            MessageBox.Show("请选择要召唤的坐骑", "提示");
            return;
        }
        Models.User.Instance.Ride(this.selectItem.item.Id);
    }
}
