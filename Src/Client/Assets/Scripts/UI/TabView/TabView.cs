using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TabView : MonoBehaviour
{
    public TabButton[] tabButtons;
    public GameObject[] tabPages;
    public int index = 0;
    public UnityAction<int> OnTabSelect;


    IEnumerator Start()
    {
        for (int i = 0; i < tabButtons.Length; ++i)
        {
            tabButtons[i].tabView = this;
            tabButtons[i].tabIndex = i;
        }
        yield return new WaitForEndOfFrame();

        //todo 下面跟原版代码不同,这里默认选中0
        for (int i = 0; i < tabButtons.Length; ++i)
        {
            tabButtons[i].Select(i == index);
            if (i < tabPages.Length)
                tabPages[i].SetActive(i == index);
        }
        yield return null;
        if (OnTabSelect != null)
            OnTabSelect(index);
    }


    public void SelectTab(int index)
    {
        if (this.index != index)
        {
            for (int i = 0; i < tabButtons.Length; ++i)
            {
                tabButtons[i].Select(i == index);
                if (i < tabPages.Length)//todo 这里跟原版代码不同
                    tabPages[i].SetActive(i == index); 
            }
            this.index = index;//todo 这里跟原版代码不同
            if (OnTabSelect != null)
                OnTabSelect(index);
        }
    }
}
