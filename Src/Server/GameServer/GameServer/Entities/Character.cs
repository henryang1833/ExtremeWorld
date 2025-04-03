using GameServer.Managers;
using SkillBridge.Message;
using GameServer.Network;
using GameServer.Models;
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
        public GuildManager GuildManager;
        public Team Team;
        public double TeamUpdateTS;
        public Guild Guild;
        public double GuildUpdateTS;

        public Character(CharacterType type, TCharacter cha) :
            base(new Core.Vector3Int(cha.MapPosX, cha.MapPosY, cha.MapPosZ), new Core.Vector3Int(100, 0,  0))
        {
            this.Data = cha;
            this.Id = cha.ID;
            this.Info = new NCharacterInfo();
            this.Info.Type = type;
            this.Info.Id = cha.ID;
            this.Info.entityId = this.entityId;
            this.Info.Name = cha.Name;
            this.Info.Level = 10;//cha.Level;
            this.Info.configId = cha.TID;
            this.Info.Class = (CharacterClass)cha.Class;
            this.Info.mapId = cha.MapID;
            this.Info.Gold = cha.Gold;
            this.Info.Ride = 0;
            this.Info.Entity = this.EntityData;
            this.Define = DataManager.Instance.Characters[this.Info.configId];

            ItemManager = new ItemManager(this);
            this.ItemManager.GetItemInfos(this.Info.itemInfos);
            this.Info.bagInfo = new NBagInfo();
            this.Info.bagInfo.Unlocked = this.Data.Bag.Unlocked;
            this.Info.bagInfo.Items = this.Data.Bag.Items;
            this.Info.Equips = this.Data.Equips;
            this.QuestManager = new QuestManager(this);
            this.QuestManager.GetQuestInfos(this.Info.questInfos);
            this.StatusManager = new StatusManager(this);
            this.FriendManager = new FriendManager(this);
            this.FriendManager.GetFriendInfos(this.Info.friendInfos);

            //this.GuildManager = GuildManager.Instance.GetGuild(this.Data.GuildId);
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

        /// <summary>
        /// 就是在发送消息的时候如果检测到好友、队伍、公会、物品有变化也一并处理，没有变化的不处理
        /// </summary>
        /// <param name="message"></param>
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
