using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class UIMessageBox : MonoBehaviour {
    public Text title;
    public Text message;
    public Image[] icons;

    public Button buttonYes;
    public Button buttonNo;

    public Text buttonYesTitle;
    public Text buttonNoTitle;

    public UnityAction OnYes;
    public UnityAction OnNo;
    

    public void Init(string title,string message,MessageBoxType type = MessageBoxType.Information,string btnOKText = "", string btnCancelText = "")
    {
        if (!string.IsNullOrEmpty(title)) this.title.text = title;
        this.message.text = message;
        this.icons[0].enabled = type == MessageBoxType.Information;
        this.icons[1].enabled = type == MessageBoxType.Confirm;
        this.icons[2].enabled = type == MessageBoxType.Error;

        if (!string.IsNullOrEmpty(btnOKText)) this.buttonYesTitle.text = btnOKText;
        if (!string.IsNullOrEmpty(btnCancelText)) this.buttonNoTitle.text = btnCancelText;

        this.buttonYes.onClick.AddListener(OnClickYes);
        this.buttonNo.onClick.AddListener(OnClickNo);
    }

    private void OnClickYes()
    {
        Destroy(this.gameObject);
        if (this.OnYes != null) this.OnYes();
    }
    private void OnClickNo()
    {
        Destroy(this.gameObject);
        if (this.OnNo != null) this.OnNo();
    }
}
