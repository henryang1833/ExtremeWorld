using GameServer.Managers;
using SkillBridge.Message;
using GameServer.Network;
using GameServer.Models;
using System;
using Common;

namespace GameServer.Entities
{
    public class Character : CharacterBase,IPostResponse
    {
        public TCharacter Data;
        public ItemManager ItemManager;
        public QuestManager QuestManager;
        public StatusManager StatusManager;
        public FriendManager FriendManager;
        public Team Team;
        public int TeamUpdateTS;

        public Character(CharacterType type, TCharacter cha) :
            base(new Core.Vector3Int(cha.MapPosX, cha.MapPosY, cha.MapPosZ), new Core.Vector3Int(100, 0,  0))
        {
            this.Data = cha;
            this.Id = cha.ID;
            this.Info = new NCharacterInfo();
            this.Info.Type = type;
            this.Info.Id = cha.ID;
            this.Info.EntityId = this.entityId;
            this.Info.Name = cha.Name;
            this.Info.Level = 10;//cha.Level;
            this.Info.ConfigId = cha.TID;
            this.Info.Class = (CharacterClass)cha.Class;
            this.Info.mapId = cha.MapID;
            this.Info.Gold = cha.Gold;
            this.Info.Ride = 0;
            this.Info.Entity = this.EntityData;
            this.Define = DataManager.Instance.Characters[this.Info.ConfigId];

            ItemManager = new ItemManager(this);
            this.ItemManager.GetItemInfos(this.Info.Items);
            this.Info.Bag = new NBagInfo();
            this.Info.Bag.Unlocked = this.Data.Bag.Unlocked;
            this.Info.Bag.Items = this.Data.Bag.Items;
            this.Info.Equips = this.Data.Equips;
            this.QuestManager = new QuestManager(this);
            this.QuestManager.GetQuestInfos(this.Info.Quests);
            this.StatusManager = new StatusManager(this);
            this.FriendManager = new FriendManager(this);
            this.FriendManager.GetFriendInfos(this.Info.Friends);
        }

        public long Gold
        {
            get { return this.Data.Gold; }
            set
            {
                if (this.Data.Gold == value)
                    return;
                this.StatusManager.AddGoldChange((int)(value - this.Data.Gold));
                this.Data.Gold = value;
            }
        }

        public int Ride //坐骑编号
        {
            get { return this.Info.Ride; }
            set
            {
                if (this.Info.Ride == value) return;
                this.Info.Ride = value;
            }
        }

        public void PostProcess(NetMessageResponse message)
        {
            if(this.Team!=null)
                Log.InfoFormat("PostProcess: characterId：{0}，friendChanged：{1}，TeamUpdateTS < this.Team.timestamp：{2}", this.Id, this.FriendManager.friendChanged, this.TeamUpdateTS < this.Team.timestamp);
            else
                Log.InfoFormat("PostProcess: characterId：{0}，friendChanged：{1}", this.Id, this.FriendManager.friendChanged);

            this.FriendManager.PostProcess(message);
            if (this.Team != null)
            {
                if(TeamUpdateTS < this.Team.timestamp)
                {
                    TeamUpdateTS = Team.timestamp;
                    this.Team.PostProcess(message);
                }
            }
            if (this.StatusManager.HasStatus)
            {
                this.StatusManager.PostProcess(message);
            }
        }

        /// <summary>
        /// 角色离开时调用
        /// </summary>
        public void Clear()
        {
            this.FriendManager.OfflineNotify();
            Team team = this.Team;
            team?.Leave(this);
            team?.MemberLeaveNotify();
        }

        public NCharacterInfo GetBasicInfo()
        {
            return new NCharacterInfo()
            {
                Id = this.Id,
                Name = this.Info.Name,
                Class = this.Info.Class,
                Level = this.Info.Level
            };
        }
    }
}
