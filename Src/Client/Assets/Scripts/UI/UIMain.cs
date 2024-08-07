using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;
public class UIMain : MonoSingleton<UIMain>
{
    public Text avatarName;
    public Text avatarLvele;

    protected override void OnStart()
    {
        this.UpdateAvatar();
    }

    private void UpdateAvatar()
    {
        this.avatarName.text = string.Format("{0}", User.Instance.CurrentCharacter.Name);
        this.avatarLvele.text = User.Instance.CurrentCharacter.Level.ToString();
    }

    public void BackToCharSelect()
    {
        SceneManager.Instance.LoadScene("CharSelect");
        Services.UserService.Instance.SendGameLeave();
    }

    public void OnClickBag()
    {
        UIManager.Instance.Show<UIBag>();
    }
}
