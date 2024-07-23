using System.Collections.Generic;
using UnityEngine;
using Services;
using Entities;
using System.Collections;

public class GameObjectManager : MonoBehaviour
{
    Dictionary<int, GameObject> Characters = new Dictionary<int, GameObject>();

    void Start()
    {
        StartCoroutine(InitGameObjects());
        CharacterManager.Instance.OnCharacterEnter = OnCharacterEnter;
    }

    private void OnDestroy()
    {
        CharacterManager.Instance.OnCharacterEnter = null;
    }

    IEnumerator InitGameObjects()
    {
        foreach(var cha in CharacterManager.Instance.Characters.Values)
        {
            CreateCharacterObject(cha);
            yield return null;
        }
    }

    private void CreateCharacterObject(Character character)
    {
        if(!Characters.ContainsKey(character.Info.Id) || Characters[character.Info.Id] == null)
        {
            Object obj = Resloader.Load<Object>(character.Define.Resource);
            if(obj == null)
            {
                Debug.LogErrorFormat("Character[{0}] Resouce[{1}] not existed", character.Define.TID, character.Define.Resource);
                return;
            }

            GameObject go = (GameObject)Instantiate(obj);
            go.name = "Character_" + character.Info.Id + "_" + character.Info.Name;
            go.transform.position = GameObjectTool.LogicToWorld(character.position);
            go.transform.forward = GameObjectTool.LogicToWorld(character.direction);


            Characters[character.Info.Id] = go;
            MainPlayerCamera.Instance.player = go;
        }


    }

    private void OnCharacterEnter(Character cha)
    {
        CreateCharacterObject(cha);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
