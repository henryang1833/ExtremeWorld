using System;
using Managers;
using Network;
using SkillBridge.Message;
using UnityEngine;

namespace Services
{
    class TeamService : Singleton<TeamService>,IDisposable
    {
        public void Init()
        {

        }

        public TeamService()
        {
            MessageDistributer.Instance.Subscribe<TeamInviteRequest>(this.OnTeamInviteRequest);
            MessageDistributer.Instance.Subscribe<TeamInviteResponse>(this.OnTeamInviteResponse);
            MessageDistributer.Instance.Subscribe<TeamInfoResponse>(this.OnTeamInfoResponse);
            MessageDistributer.Instance.Subscribe<TeamLeaveResponse>(this.OnTeamLeaveResponse);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<TeamInviteRequest>(this.OnTeamInviteRequest);
            MessageDistributer.Instance.Unsubscribe<TeamInviteResponse>(this.OnTeamInviteResponse);
            MessageDistributer.Instance.Unsubscribe<TeamInfoResponse>(this.OnTeamInfoResponse);
            MessageDistributer.Instance.Unsubscribe<TeamLeaveResponse>(this.OnTeamLeaveResponse);
        }

        public void SendTeamInviteRequest(int friendId, string friendName)
        {
            Debug.Log("SendTeamInviteRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.TeamInviteReq = new TeamInviteRequest();
            message.Request.TeamInviteReq.FromId = Models.User.Instance.CurrentCharacter.Id;
            message.Request.TeamInviteReq.FromName = Models.User.Instance.CurrentCharacter.Name;
            message.Request.TeamInviteReq.ToId = friendId;
            message.Request.TeamInviteReq.ToName = friendName;
            NetClient.Instance.SendMessage(message);
        }

        public void SendTeamInviteResponse(bool accept, TeamInviteRequest request)
        {
            Debug.Log("SendTeamInviteResponse");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.TeamInviteRes = new TeamInviteResponse();
            message.Request.TeamInviteRes.Result = accept ? Result.Success : Result.Failed;
            message.Request.TeamInviteRes.Errormsg = accept ? "组队成功" : "对方拒绝了组队请求";
            message.Request.TeamInviteRes.Request = request;
            NetClient.Instance.SendMessage(message);
        }

        private void OnTeamInviteRequest(object sender, TeamInviteRequest request)
        {
            Debug.LogFormat("OnTeamInviteRequest : requesterId:{0} , requesterName:{1}",request.FromId,request.FromName);
            var confirm = MessageBox.Show(string.Format("【{0}】邀请你加入队伍", request.FromName), "组队请求", MessageBoxType.Confirm, "接受", "拒绝");
            confirm.OnYes = () =>
            {
                this.SendTeamInviteResponse(true, request);
            };
            confirm.OnNo = () =>
            {
                this.SendTeamInviteResponse(false, request);
            };
        }

        private void OnTeamInviteResponse(object sender, TeamInviteResponse message)
        {
            if (message.Result == Result.Success)
                //MessageBox.Show(string.Format("{0}加入了队伍！", message.Request.ToName), "邀请成功");
                Debug.LogFormat("{0}加入了队伍！", message.Request.ToName);
            else
                MessageBox.Show(string.Format("{0}！", message.Errormsg), "邀请失败");
        }

        private void OnTeamInfoResponse(object sender, TeamInfoResponse message)
        {
            Debug.Log("OnTeamInfoResponse");
            TeamManager.Instance.UpdateTeamInfo(message.Team);
        }

        public void SendTeamLeaveRequest(int id)
        {
            Debug.Log("SendTeamLeaveRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.TeamLeave = new TeamLeaveRequest();
            message.Request.TeamLeave.TeamId = Models.User.Instance.TeamInfo.Id;
            message.Request.TeamLeave.characterId = Models.User.Instance.CurrentCharacter.Id;
            NetClient.Instance.SendMessage(message);
        }

        private void OnTeamLeaveResponse(object sender, TeamLeaveResponse message)
        {
            if (message.Result == Result.Success)
            {
                TeamManager.Instance.UpdateTeamInfo(null);
                MessageBox.Show("退出成功", "退出队伍");
            }
            else
                MessageBox.Show("退出失败", "退出队伍", MessageBoxType.Error);
        }
        
        
    }
}
