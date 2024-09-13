using System;
using System.Collections.Generic;
using Common.Data;
using GameServer.Models;
using GameServer.Managers;
using Common;

namespace GameServer.Entities
{
    class Spawner
    {
        private SpawnRuleDefine Define { get; set; }
        private Map Map;
        private float spawnTime = 0;
        private float unspawnTIme = 0; //怪物被杀死时间
        private bool spawned = false;
        private SpawnPointDefine spawnPoint = null;
        public Spawner(SpawnRuleDefine define, Map map)
        {
            this.Define = define;
            this.Map = map;

            if (DataManager.Instance.SpawnPoints.ContainsKey(this.Map.ID))
            {
                if (DataManager.Instance.SpawnPoints[this.Map.ID].ContainsKey(this.Define.SpawnPoint))
                    spawnPoint = DataManager.Instance.SpawnPoints[this.Map.ID][this.Define.SpawnPoint];
                else
                    Log.ErrorFormat("SpawnRule[{0}] SpawnPoint[{1}] not existed", this.Define.ID, this.Define.SpawnPoint);
            }
        }

        internal void Update()
        {
            if (this.CanSpawn())
            {
                this.Spawn();
            }
        }

        private bool CanSpawn()
        {
            if (this.spawned)
                return false;
            if (this.unspawnTIme + this.Define.SpawnPeriod > Time.time)
                return false;
            return true;
        }

        private void Spawn()
        {
            this.spawned = true;
            Log.InfoFormat("Map[{0} Spawn[{1}]:Mon:{2},Lv:{3}] At Point:{4}", this.Define.MapID, this.Define.ID, this.Define.SpawnMonID, this.Define.SpawnLevel, this.Define.SpawnPoint);
            this.Map.MonsterManager.Create(this.Define.SpawnMonID, this.Define.SpawnLevel, this.spawnPoint.Position, this.spawnPoint.Direction);
        }
    }
}
