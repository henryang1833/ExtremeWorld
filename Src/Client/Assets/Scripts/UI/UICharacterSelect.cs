using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillBridge.Message;
using Models;
using Services;

public class UICharacterSelect : MonoBehaviour {
    public GameObject panelSelect;
    public GameObject panelCreate;

    CharacterClass charClass; //所选类别

    public Image[] titlesImg;
    public Text[] namesText;
    public Text descsText;
    public InputField charName;
    public Transform uiCharList;
    public List<GameObject> uiChars = new List<GameObject>();
    public GameObject uiCharInfo;
    public List<GameObject> classCheckMarks = new List<GameObject>();
    public UICharacterView characterView;

    private int selectCharacterIdx = -1;
    void Start ()
    {
        InitCharacterSelect(true);
        UserService.Instance.OnCharacterCreate = OnCharacterCreate;
    }

    private void OnCharacterCreate(Result result, string message)
    {
        if (result == Result.Success)
            InitCharacterSelect(true);
        else
            MessageBox.Show(message, "错误", MessageBoxType.Error);
    }

    public void InitCharacterSelect(bool init)
    {
        panelCreate.SetActive(false);
        panelSelect.SetActive(true);

        if (init)
        {
            foreach(var old in uiChars)         
                Destroy(old);
            uiChars.Clear();

            for (int i = 0; i < User.Instance.Info.Player.Characters.Count; ++i)
            {
                GameObject go = Instantiate(uiCharInfo, this.uiCharList);
                UICharInfo chrInfo = go.GetComponent<UICharInfo>();
                chrInfo.info = User.Instance.Info.Player.Characters[i];

                Button button = go.GetComponent<Button>();
                int idx = i;
                button.onClick.AddListener(() =>
                {
                    OnSelectCharacter(idx);
                });

                uiChars.Add(go);
                go.SetActive(true);
            }
            if(User.Instance.Info.Player.Characters.Count>0)
                OnSelectCharacter(0);
        }
    }

    public void InitCharacterCreate()
    {
        panelCreate.SetActive(true);
        panelSelect.SetActive(false);
        OnSelectClass(1);
    }

    public void OnSelectClass(int charClass)
    {
        this.charClass = (CharacterClass)charClass;

        characterView.CurrentCharacter = charClass - 1;

        for(int i = 0; i < 3; ++i)
        {
            titlesImg[i].gameObject.SetActive(i == charClass - 1);
            classCheckMarks[i].SetActive(i == charClass - 1);
            namesText[i].text = DataManager.Instance.Characters[i + 1].Name;
        }

        descsText.text = DataManager.Instance.Characters[charClass].Description;
    }

    public void OnSelectCharacter(int idx)
    {
        this.selectCharacterIdx = idx;
        var cha = User.Instance.Info.Player.Characters[idx];
        Debug.LogFormat("Select Char:[{0}]{1}[{2}]", cha.Id, cha.Name, cha.Class);
        
        characterView.CurrentCharacter = (int)cha.Class - 1; // = cha.Class?

        for(int i = 0; i < User.Instance.Info.Player.Characters.Count; ++i)
        {
            UICharInfo ci = this.uiChars[i].GetComponent<UICharInfo>();
            ci.Selected = idx == i;
        }
    }

    public void OnClickPlay()
    {
        if (selectCharacterIdx >= 0)
        {      
            UserService.Instance.SendGameEnter(selectCharacterIdx); //是不是应该再加一
        }
    }

    public void OnClickCreate()
    {
        if (string.IsNullOrEmpty(charName.text))
        {
            MessageBox.Show("请输入角色名称");
            return;
        }
        UserService.Instance.SendCharacterCreate(this.charName.text, this.charClass);
    }

}
