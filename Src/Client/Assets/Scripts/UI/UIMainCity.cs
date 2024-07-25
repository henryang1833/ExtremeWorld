using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;
public class UIMainCity : MonoBehaviour
{
    public Text avatarName;
    public Text avatarLvele;
    
    void Start()
    {
        this.UpdateAvatar();
    }

    private void UpdateAvatar()
    {
        this.avatarName.text = string.Format("{0}", User.Instance.CurrentCharacter.Name);
        this.avatarLvele.text = User.Instance.CurrentCharacter.Level.ToString();
    }
}
