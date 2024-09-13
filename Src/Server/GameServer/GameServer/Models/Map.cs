using System.Collections.Generic;
using Network;
using Common;
using Common.Data;
using GameServer.Entities;
using SkillBridge.Message;
using GameServer.Services;
using GameServer.Managers;
namespace GameServer.Models
{
    public class Map
    {
        internal class MapCharacter
        {
            public NetConnection<NetSession> connection;
            public Character character;

            public MapCharacter(NetConnection<NetSession> conn,Character cha)
            {
                this.connection = conn;
                this.character = cha;
            }
        }

        internal MapDefine Define;
        public int ID
        {
            get { return this.Define.ID; }
        }
        /// <summary>
        /// 地图中的角色，以CharacterID为Key
        /// </summary>
        Dictionary<int, MapCharacter> MapCharacters = new Dictionary<int, MapCharacter>();

        //刷怪管理器
        SpawnManager SpawnManager = new SpawnManager();
        public MonsterManager MonsterManager = new MonsterManager();
        public Map(MapDefine define)
        {
            this.Define = define;
            this.SpawnManager.Init(this);
            this.MonsterManager.Init(this);
        }

        internal void Update()
        {
            SpawnManager.Update();
        }

        internal void CharacterEnter(NetConnection<NetSession> conn, Character character)
        {
            Log.InfoFormat("CharacterEnter: Map:{0} characterId:{1}", this.Define.ID, character.Id);
            character.Info.mapId = this.ID;
            this.MapCharacters[character.Id] = new MapCharacter(conn, character);

            conn.Session.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            conn.Session.Response.mapCharacterEnter.mapId = this.Define.ID;
            //conn.Session.Response.mapCharacterEnter.Characters.Add(character.Info); 这一行不需要了？为什么？

            foreach (var kv in this.MapCharacters)
            {
                conn.Session.Response.mapCharacterEnter.Characters.Add(kv.Value.character.Info);
                if (kv.Value.character != character)
                    this.AddCharacterEnterMap(kv.Value.connection, character.Info);
            }

            foreach (var kv in this.MonsterManager.Monsters)
                conn.Session.Response.mapCharacterEnter.Characters.Add(kv.Value.Info);
            conn.SendResponse();
        }

        internal void CharacterLeave(Character cha)
        {
            Log.InfoFormat("CharacterLeave: Map:{0} characterId:{1}", this.Define.ID, cha.Id);
            foreach (var kv in this.MapCharacters)    
                this.SendCharacterLeaveMap(kv.Value.connection, cha);
            this.MapCharacters.Remove(cha.Id);
        }

        internal void UpdateEntity(NEntitySync entity)
        {
            foreach(var kv in this.MapCharacters)
            {
                if(kv.Value.character.entityId == entity.Id)
                {
                    kv.Value.character.Position = entity.Entity.Position;
                    kv.Value.character.Direction = entity.Entity.Direction;
                    kv.Value.character.Speed = entity.Entity.Speed;
                    if(entity.Event == EntityEvent.Ride)
                    {
                        kv.Value.character.Ride = entity.Param;
                    }
                }
                else
                {
                    MapService.Instance.SendEntityUpdate(kv.Value.connection, entity);
                }
            }
        } 

        private void AddCharacterEnterMap(NetConnection<NetSession> conn, NCharacterInfo character)
        {
            if (conn.Session.Response.mapCharacterEnter == null)
            {
                conn.Session.Response.mapCharacterEnter = new MapCharacterEnterResponse();
                conn.Session.Response.mapCharacterEnter.mapId = this.Define.ID;
            }
            conn.Session.Response.mapCharacterEnter.Characters.Add(character);
            conn.SendResponse();
        }

        private void SendCharacterLeaveMap(NetConnection<NetSession> conn, Character cha)
        {
            conn.Session.Response.mapCharacterLeave = new MapCharacterLeaveResponse();
            conn.Session.Response.mapCharacterLeave.entityId = cha.entityId;
            conn.SendResponse();
        }

        internal void MonsterEnter(Monster monster)
        {
            Log.InfoFormat("MonsterEnter:Map:{0} monsterID:{1}",this.Define.ID,monster.Id);
            foreach (var kv in this.MapCharacters)
                this.AddCharacterEnterMap(kv.Value.connection, monster.Info);
        }
    }
}
