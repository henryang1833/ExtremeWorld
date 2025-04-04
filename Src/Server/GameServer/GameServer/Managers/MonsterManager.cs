﻿using System.Collections.Generic;
using GameServer.Entities;
using GameServer.Models;
using SkillBridge.Message;
namespace GameServer.Managers
{
    public class MonsterManager
    {
        public Map Map;
        public Dictionary<int, Monster> Monsters = new Dictionary<int, Monster>();
        public void Init(Map map)
        {
            this.Map = map;
        }

        public Monster Create(int spawnMonID,int spawnLevel,NVector3 position,NVector3 direction)
        {
            Monster monster = new Monster(spawnMonID, spawnLevel, position, direction);
            EntityManager.Instance.AddEntity(this.Map.ID, monster);
            monster.Id = monster.entityId;
            monster.Info.EntityId = monster.entityId;
            monster.Info.mapId = this.Map.ID;
            Monsters[monster.Id] = monster;
            this.Map.MonsterEnter(monster);
            return monster;
        }
    }
}
