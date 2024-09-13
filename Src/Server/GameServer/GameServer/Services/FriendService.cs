using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;
using Network;
using GameServer.Entities;
using GameServer.Managers;

namespace GameServer.Services
{
    class FriendService : Singleton<FriendService>
    {
        List<FriendAddRequest> friendRequests = new List<FriendAddRequest>();
        public FriendService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendAddRequest>(this.OnFriendAddRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendAddResponse>(this.OnFriendAddResponse);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendRemoveRequest>(this.OnFriendRemoveRequest);

        }

        public void Init()
        {

        }

        private void OnFriendRemoveRequest(NetConnection<NetSession> sender, FriendRemoveRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendRemoveRequest: character:{0} FriendReletionID:{1}", character.Id, request.Id);
            sender.Session.Response.friendRemove = new FriendRemoveResponse();
            sender.Session.Response.friendRemove.Id = request.Id;

            //删除自己的好友
            if (character.FriendManager.RemoveFriendByID(request.Id))
            {
                sender.Session.Response.friendRemove.Result = Result.Success;
                //删除别人好友中的自己
                var friend = SessionManager.Instance.GetSession(request.friendId);
                if (friend != null)
                {
                    friend.Session.Character.FriendManager.RemoveFriendByFriendId(character.Id);
                }
                else
                {
                    this.RemoveFriend(request.friendId, character.Id);
                }
            }
            else
                sender.Session.Response.friendRemove.Result = Result.Failed;

            DBService.Instance.Save();
            sender.SendResponse();
        }
        private void RemoveFriend(int charId, int friendId)
        {
            var removeItem = DBService.Instance.Entities.TCharacterFriends.FirstOrDefault(v => v.CharacterID == charId && v.FriendID == friendId);
            if (removeItem != null)
                DBService.Instance.Entities.TCharacterFriends.Remove(removeItem);
        }

        private void OnFriendAddResponse(NetConnection<NetSession> sender, FriendAddResponse response)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendAddResponse:character:{0} Result:{1} FromId:{2} ToId:{3}", character.Id, response.Result, response.Request.FromId, response.Request.ToId);
            
            if (response.Result == Result.Success)
            {//接受了好友请求
                var requester = SessionManager.Instance.GetSession(response.Request.FromId);
                if (requester == null)
                {
                    sender.Session.Response.friendAddRes = response;
                    sender.Session.Response.friendAddRes.Result = Result.Failed;
                    sender.Session.Response.friendAddRes.Errormsg = "请求者已下线";
                    sender.SendResponse();
                }
                else
                {
                    //互加好友
                    character.FriendManager.AddFriend(requester.Session.Character);
                    requester.Session.Character.FriendManager.AddFriend(character);
                    DBService.Instance.Save();
                    requester.Session.Response.friendAddRes = response;
                    requester.Session.Response.friendAddRes.Result = Result.Success;
                    requester.Session.Response.friendAddRes.Errormsg = "添加好友成功";
                    requester.SendResponse();
                    sender.Session.Response.friendAddRes = response;
                    sender.SendResponse();
                }  
            }
            else //拒绝了好友请求
            {
                var requester = SessionManager.Instance.GetSession(response.Request.FromId);
                if (requester != null)
                {
                    requester.Session.Response.friendAddRes = response;
                    requester.Session.Response.friendAddRes.Result = Result.Failed;
                    requester.Session.Response.friendAddRes.Errormsg = "对方拒绝添加好友";
                    requester.SendResponse();
                }  
            }
        }

        private void OnFriendAddRequest(NetConnection<NetSession> sender, FriendAddRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendAddRequest:FromId:{0} FromName:{1} ToId:{2} ToName:{3}", request.FromId, request.FromName, request.ToId, request.ToName);
            sender.Session.Response.friendAddRes = new FriendAddResponse();
            sender.Session.Response.friendAddRes.Request = request;

            if (request.ToId == 0)
            {//如果没有传入ID，则使用名称查找
                foreach (var cha in CharacterManager.Instance.Characters)
                {
                    if (cha.Value.Data.Name == request.ToName)
                    {
                        request.ToId = cha.Key;
                        break;
                    }
                }
            }
            NetConnection<NetSession> friend = null;
            if (request.ToId > 0)
            {
                if (character.FriendManager.GetFriendInfo(request.ToId) != null)
                {
                    sender.Session.Response.friendAddRes.Result = Result.Failed;
                    sender.Session.Response.friendAddRes.Errormsg = "好友已存在";
                    sender.SendResponse();
                    return;
                }
                friend = SessionManager.Instance.GetSession(request.ToId);
            }
            if (friend == null)
            {
                sender.Session.Response.friendAddRes.Result = Result.Failed;
                sender.Session.Response.friendAddRes.Errormsg = "好友不存在或者不在线";
                sender.SendResponse();
                return;
            }
            Log.InfoFormat("Forward Request: FromId:{0} FromName:{1} ToId:{2} ToName:{3}", request.FromId, request.FromName, request.ToId, request.ToName);
            friend.Session.Response.friendAddReq = request;
            friend.SendResponse();
        }
    }
}
