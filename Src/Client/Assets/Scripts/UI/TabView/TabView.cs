using System.Collections;
using UnityEngine;

public class TabView : MonoBehaviour
{
    public TabButton[] tabButtons;
    public GameObject[] tabPages;
    public int index = -1;
    
    IEnumerator Start()
    {
        for(int i = 0; i < tabButtons.Length; ++i)
        {
            tabButtons[i].tabView = this;
            tabButtons[i].tabIndex = i;
        }
        yield return new WaitForEndOfFrame();
        SelectTab(0);
    }


    internal void SelectTab(int index)
    {
        if (this.index != index)
        {
            for(int i = 0; i < tabButtons.Length; ++i)
            {
                tabButtons[i].Select(i == index);
                tabPages[i].SetActive(i == index);
            }
            this.index = index;
        }
    }
}
