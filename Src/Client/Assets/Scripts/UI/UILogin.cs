using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using SkillBridge.Message;
using System;

public class UILogin : MonoBehaviour {
    public InputField username;
    public InputField password;

    void Start()
    {
        UserService.Instance.OnLogin = OnLogin;
    }

    public void OnClickLogin()
    {
        if (string.IsNullOrEmpty(this.username.text))
        {
            MessageBox.Show("请输入账号");
            return;
        }
        if (string.IsNullOrEmpty(this.password.text))
        {
            MessageBox.Show("请输入密码");
            return;
        }
        UserService.Instance.SendLogin(this.username.text, this.password.text);
    }

    public void OnLogin(Result result, string message)
    {
        if (result == Result.Success)
            MessageBox.Show("登录成功", "提示", MessageBoxType.Information); //todo: OnNo呢？
        else
            MessageBox.Show(message, "错误", MessageBoxType.Error);
    }


}
