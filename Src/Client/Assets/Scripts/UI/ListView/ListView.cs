using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ListView : MonoBehaviour
{
    public UnityAction<ListViewItem> onItemSelected;
    private ListViewItem selectedItem = null;
    private List<ListViewItem> items = new List<ListViewItem>();
    public ListViewItem SelectedItem
    {
        get { return selectedItem; }
        private set
        {
            if (selectedItem != null && selectedItem != value)
                selectedItem.Selected = false;

            selectedItem = value;
            if (onItemSelected != null)
                onItemSelected.Invoke(value);
        }
    }

    public void AddItem(ListViewItem item)
    {
        item.owner = this;
        this.items.Add(item);
    }

    public void RemoveAll()
    {
        foreach (var it in items)
            Destroy(it.gameObject);
        items.Clear();
    }

    //自己写的
    //取消列表的选中状态，也就是不选中任何一个
    public void DisableSelected()
    {
        if (selectedItem != null)
            selectedItem.Selected = false;
        selectedItem = null;
    }

    //自己写的,默认选中第一个选项
    public bool SelectFirstItem()
    {
        if (items.Count == 0)
            return false;
        items[0].OnPointerClick(null);
        return true;
    }

    public class ListViewItem : MonoBehaviour, IPointerClickHandler
    {
        private bool selected;
        public ListView owner;
        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                onSelected(selected);
            }
        }

        //todo: 好像没用到
        //更换选中状态UI显示
        public virtual void onSelected(bool selected)
        {
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!this.selected)
                this.Selected = true;
            if (owner != null && owner.SelectedItem != this)
                owner.SelectedItem = this;
        }
    }
}
