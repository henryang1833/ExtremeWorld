using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Entities;
using System;

public class UINameBar : MonoBehaviour {
    public Text avaverName;
    public Character character;
	
	void Start () {
        if (this.character != null)
        {

        }
	}
	
	void Update () {
        this.UpdateInfo();
        this.transform.forward = Camera.main.transform.forward ;
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
