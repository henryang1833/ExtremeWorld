using System.Collections.Generic;
using UnityEngine;
using Services;
using Entities;
using System.Collections;
using Managers;
using Models;
using Common;
public class GameObjectManager : MonoSingleton<GameObjectManager>
{
    Dictionary<int, GameObject> Characters = new Dictionary<int, GameObject>();

    protected override void OnStart()
    {
        StartCoroutine(InitGameObjects());
        CharacterManager.Instance.OnCharacterEnter += OnCharacterEnter;
        CharacterManager.Instance.OnCharacterLeave += OnCharacterLeave;
    }

    private void OnCharacterLeave(Character character)
    {
        if (!Characters.ContainsKey(character.entityId))
            return;
        if(Characters[character.entityId]!=null)
        {
            Destroy(Characters[character.entityId]);
            this.Characters.Remove(character.entityId);
        }
    }

    private void OnDestroy()
    {
        CharacterManager.Instance.OnCharacterEnter -= OnCharacterEnter;
        CharacterManager.Instance.OnCharacterLeave -= OnCharacterLeave;
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
        if(!Characters.ContainsKey(character.entityId) || Characters[character.entityId] == null)
        {
            Object obj = Resloader.Load<Object>(character.Define.Resource);
            if (obj == null)
            {
                Debug.LogErrorFormat("Character[{0}] Resouce[{1}] not existed", character.Define.TID, character.Define.Resource);
                return;
            }

            GameObject go = (GameObject)Instantiate(obj, this.transform);
            go.name = "Character_" + character.Info.Id + "_" + character.Info.Name;
            Characters[character.entityId] = go;
            UIWorldElementManager.Instance.AddCharacterNameBar(go.transform, character);

            InitGameObject(character, Characters[character.entityId]);//先放到括号里面试一下，不行再放到外面
        }
        
    }

    private  void InitGameObject(Character character, GameObject go)
    {
        //Log.ErrorFormat("InitGameObject");
        go.transform.position = GameObjectTool.LogicToWorld(character.position);
        go.transform.forward = GameObjectTool.LogicToWorld(character.direction);

        EntityController ec = go.GetComponent<EntityController>();
        if (ec != null)
        {
            ec.entity = character;
            ec.isPlayer = character.IsPlayer;
        }
        PlayerInputController pc = go.GetComponent<PlayerInputController>();
        if (pc != null)
        {
            //Log.ErrorFormat("pc != null");
            if (character.Info.Id == Models.User.Instance.CurrentCharacter.Id)
            {
                //Log.ErrorFormat("character.Info.Id == Models.User.Instance.CurrentCharacter.Id");
                User.Instance.CurrentCharacterObject = go;
                MainPlayerCamera.Instance.player = go;
                pc.enabled = true;
                pc.character = character;
                pc.entityController = ec;
            }
            else
            {
                pc.enabled = false;
            }
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
