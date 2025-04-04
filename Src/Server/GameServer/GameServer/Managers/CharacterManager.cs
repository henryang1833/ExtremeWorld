﻿using System.Collections.Generic;
using Common;
using GameServer.Entities;
using SkillBridge.Message;
namespace GameServer.Managers
{
    /// <summary>
    /// 维护所有在线的角色信息
    /// </summary>
    public class CharacterManager: Singleton<CharacterManager>
    {
        public Dictionary<int, Character> Characters = new Dictionary<int, Character>();

        public void Clear() => this.Characters.Clear();

        public Character AddCharacter(TCharacter cha)
        {
            Character character = new Character(CharacterType.Player,cha);
            EntityManager.Instance.AddEntity(cha.MapID, character);//?
            character.Info.EntityId = character.entityId;
            this.Characters[character.Id] = character;
            return character;
        }

        public void RemoveCharacter(int characterId)
        {
            var cha = this.Characters[characterId];
            EntityManager.Instance.RemoveEntity(cha.Data.MapID, cha);//?
            this.Characters.Remove(characterId);
        }

        public Character GetCharacter(int characterId)
        {
            Character character = null;
            this.Characters.TryGetValue(characterId, out character);
            return character;
        }
    }
}
