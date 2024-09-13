using UnityEngine.UI;
using Models;
public class UIMain : MonoSingleton<UIMain>
{
    public Text avatarName;
    public Text avatarLvele;
    public UITeam TeamWindow;

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
        TeamWindow.ShowTeam(false);
    }

    public void OnClickBag()
    {
        UIManager.Instance.Show<UIBag>();
    }

    public void OnClickEquip()
    {
        UIManager.Instance.Show<UICharEquip>();
    }

    public void OnClickQuest()
    {
        UIManager.Instance.Show<UIQuestSystem>();
    }

    public void OnClickFriend()
    {
        UIManager.Instance.Show<UIFriends>();
    }

    public void ShowTeamUI(bool show)
    {
        TeamWindow.ShowTeam(show);
    }

    public void OnClickRide()
    {
        UIManager.Instance.Show<UIRide>();
    }
}
