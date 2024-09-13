using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managers
{
    public class FriendManager : Singleton<FriendManager>
    {
        //所有有效任务
        public List<NFriendInfo> allFriends;
        public void Init(List<NFriendInfo> friends)
        {
            this.allFriends = friends;
        }
    }
}
