using UnityEngine;

class InputBox
{
    static Object cacheObject = null;
    public static UIInputBox Show(string message,string title = "",string btnOk = "",string btnCancel = "",string emptyTips = "")
    {
        if(cacheObject == null)
        {
            cacheObject = Resloader.Load<Object>("UI/Input/UIInputBox");
        }

        GameObject go = (GameObject)GameObject.Instantiate(cacheObject);
        UIInputBox inputBox = go.GetComponent<UIInputBox>();
        inputBox.Init(title, message, btnOk, btnCancel, emptyTips);
        return inputBox;
    }
}

