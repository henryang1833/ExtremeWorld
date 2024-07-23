using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterView : MonoBehaviour {
    public GameObject[] characters;
    private int currentCharacter = 0;

    public int CurrentCharacter
    {
        get
        {
            return currentCharacter;
        }
        set
        {
            currentCharacter = value;
            this.UpdateCharacter();
        }
    }

    void UpdateCharacter()
    {
        for(int i = 0; i < 3; ++i)
            characters[i].SetActive(this.currentCharacter == i);  
    }
}
