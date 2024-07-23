using UnityEngine;
using UnityEngine.UI;
using Services;
using SkillBridge.Message;

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

    void OnLogin(Result result, string message)
    {
        if (result == Result.Success)
        {
            //登录成功，进入角色选择
            SceneManager.Instance.LoadScene("CharSelect");
        }
        else
            MessageBox.Show(message, "错误", MessageBoxType.Error);
    }


}
