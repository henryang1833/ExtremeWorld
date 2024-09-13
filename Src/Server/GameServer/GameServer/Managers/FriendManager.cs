using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using GameServer.Entities;
using GameServer.Services;
using SkillBridge.Message;

namespace GameServer.Managers
{
    public class FriendManager
    {
        private Character Owner;

        List<NFriendInfo> friends = new List<NFriendInfo>();

        public bool friendChanged = false;

        public FriendManager(Character owner)
        {
            this.Owner = owner;
            this.InitFriends();
        }

        public void GetFriendInfos(List<NFriendInfo> list)
        {
            foreach(var f in this.friends)
            {
                list.Add(f);
            }
        }

        public void InitFriends()
        {
            this.friends.Clear();
            foreach(var friend in this.Owner.Data.Friends)
            {
                this.friends.Add(GetFriendInfo(friend));
            }
        }

        internal void AddFriend(Character friend)
        {
            TCharacterFriend tf = new TCharacterFriend()
            {
                FriendID = friend.Id,
                FriendName = friend.Data.Name,
                Class = friend.Data.Class,
                Level = friend.Data.Level
            };
            this.Owner.Data.Friends.Add(tf);
            friendChanged = true;
        }

        public bool RemoveFriendByFriendID(int friendId)
        {
            var removeItem = this.Owner.Data.Friends.FirstOrDefault(v => v.FriendID == friendId);
            if (removeItem != null)
                DBService.Instance.Entities.TCharacterFriends.Remove(removeItem);
            friendChanged = true;
            return true;
        }

        public bool RemoveFriendByID(int id)
        {
            var removeItem = this.Owner.Data.Friends.FirstOrDefault(v => v.Id == id);
            if (removeItem != null)
                DBService.Instance.Entities.TCharacterFriends.Remove(removeItem);
            friendChanged = true;
            return true;
        }

        private NFriendInfo GetFriendInfo(TCharacterFriend friend)
        {
            NFriendInfo friendInfo = new NFriendInfo();
            var character = CharacterManager.Instance.GetCharacter(friend.FriendID);
            friendInfo.friendInfo = new NCharacterInfo();
            friendInfo.Id = friend.Id;
            if(character == null) 
            {
                friendInfo.friendInfo.Id = friend.FriendID;
                friendInfo.friendInfo.Name = friend.FriendName;
                friendInfo.friendInfo.Class = (CharacterClass)friend.Class;
                friendInfo.friendInfo.Level = friend.Level;
                friendInfo.Status = 0;
            }
            else
            {
                friendInfo.friendInfo = character.GetBasicInfo();
                friendInfo.friendInfo.Name = character.Info.Name;
                friendInfo.friendInfo.Class = character.Info.Class;
                friendInfo.friendInfo.Level = character.Info.Level;

                if(friend.Level != character.Info.Level)
                {
                    friend.Level = character.Info.Level;
                }
                character.FriendManager.UpdateFriendInfo(this.Owner.Info, 1);
                friendInfo.Status = 1;
            }
            Log.InfoFormat("{0}:{1} GetFriendInfo:{2}:{3} Status:{4}", this.Owner.Id, this.Owner.Info.Name, friendInfo.friendInfo.Id, friendInfo.friendInfo.Name,friendInfo.Status);
            return friendInfo;
        }

        internal NFriendInfo GetFriendInfo(int friendId)
        {
            foreach(var f in this.friends)
            {
                if(f.friendInfo.Id == friendId)
                {
                    return f;
                }
            }
            return null;
        }

        public void OfflineNotify()
        {
            foreach(var friendInfo in this.friends)
            {
                var friend = CharacterManager.Instance.GetCharacter(friendInfo.friendInfo.Id);
                if (friend != null)
                {
                    friend.FriendManager.UpdateFriendInfo(this.Owner.Info, 0);
                }
            }
        }

        internal void PostProcess(NetMessageResponse message)
        {
            if (friendChanged)
            {
                this.InitFriends();
                if(message.friendList == null)
                {
                    message.friendList = new FriendListResponse();
                    message.friendList.Friends.AddRange(this.friends);
                }
                friendChanged = false;
            }
        }

        internal bool RemoveFriendByFriendId(int friendId)
        {
            var removeItem = this.Owner.Data.Friends.FirstOrDefault(v => v.FriendID == friendId);
            if (removeItem != null)
            {
                DBService.Instance.Entities.TCharacterFriends.Remove(removeItem);
            }
            friendChanged = true;
            return true;
        }

        internal void UpdateFriendInfo(NCharacterInfo friendInfo, int status)
        {
            foreach(var f in this.friends)
            {
                if(f.friendInfo.Id == friendInfo.Id)
                {
                    f.Status = status;
                    break;
                }
            }
            this.friendChanged = true;
        }
    }
}
