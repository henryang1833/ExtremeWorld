using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Entities;
using System;

public class UINameBar : MonoBehaviour
{
    public Text avaverName;
    public Character character;

    void Start()
    {
        if (this.character != null)
        {

        }
    }

    void Update()
    {
        this.UpdateInfo();
    }

    private void UpdateInfo()
    {
        if (this.character != null)
        {
            string name = character.Name + " Lv." + this.character.Info.Level;
            if (name != this.avaverName.text)
                this.avaverName.text = name;
        }
    }
}

/*
 [ExecuteInEditMode]
public class UINameBar : MonoBehaviour
{
    public Image avatar;
    public Text characterName;
    public Character character;

    void Start()
    {
        if (this.character != null)
        {
            if (character.Info.Type == SkillBridge.Message.CharacterType.Monster)
                this.avatar.gameObject.SetActive(false);
            else
                this.avatar.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        this.UpdateInfo();
    }

    private void UpdateInfo()
    {
        if (this.character != null)
        {
            string name = character.Name + " Lv." + this.character.Info.Level;
            if (name != this.characterName.text)
                this.characterName.text = name;
        }
    }
}
*/
