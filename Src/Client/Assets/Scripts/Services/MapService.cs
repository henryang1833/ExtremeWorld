﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Data;
using Models;
using System;
using Network;
using SkillBridge.Message;
namespace Services
{
    public class MapService : Singleton<MapService>, IDisposable
    {

        public int CurrentMapId = 0;
        public MapService()
        {
            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
        }

        private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse response)
        {
            Debug.LogFormat("OnMapCharacterEnter:Map:{0} Count:{1}", response.mapId, response.Characters.Count);
            foreach(var cha in response.Characters)
            {
                if (User.Instance.CurrentCharacter.Id == cha.Id)
                    User.Instance.CurrentCharacter = cha;

                CharacterManager.Instance.AddCharacter(cha);
            }

            if (CurrentMapId != response.mapId)
            {
                this.EnterMap(response.mapId);
                this.CurrentMapId = response.mapId;
            }
        }

        private void EnterMap(int mapId)
        {
            if (DataManager.Instance.Maps.ContainsKey(mapId))
            {
                MapDefine map = DataManager.Instance.Maps[mapId];
                SceneManager.Instance.LoadScene(map.Resource);
            }
            else
                Debug.LogFormat("EnterMap:Map {0} not existed", mapId);
        }

        internal void Init()
        {
            
        }

        private void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse message)
        {
            throw new NotImplementedException();
        }

        

        
    }
}

