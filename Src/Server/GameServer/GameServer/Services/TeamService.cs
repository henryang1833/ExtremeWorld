using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network;
using SkillBridge.Message;
using GameServer.Managers;
using GameServer.Entities;
using GameServer.Models;

namespace GameServer.Services
{
    class TeamService : Singleton<TeamService>
    {   
        public TeamService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamInviteRequest>(this.OnTeamInviteRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamInviteResponse>(this.OnTeamInviteResponse);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamLeaveRequest>(this.TeamLeaveRequest);
        }

        public void Init()
        {
            TeamManager.Instance.Init();
        }

        private void OnTeamInviteRequest(NetConnection<NetSession> sender, TeamInviteRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnTeamInviteRequest:FromId:{0} FromName:{1} ToId:{2} ToName:{3}", request.FromId, request.FromName, request.ToId, request.ToName);
            //TODO:执行一些前置数据校验

            //开始逻辑
            NetConnection<NetSession> target = SessionManager.Instance.GetSession(request.ToId);
            if(target == null)
            {
                sender.Session.Response.TeamInviteRes = new TeamInviteResponse();
                sender.Session.Response.TeamInviteRes.Result = Result.Failed;
                sender.Session.Response.TeamInviteRes.Errormsg = "对方不存在或者不在线";
                sender.SendResponse();
                return;
            }

            if(target.Session.Character.Team != null)
            {
                sender.Session.Response.TeamInviteRes = new TeamInviteResponse();
                sender.Session.Response.TeamInviteRes.Result = Result.Failed;
                sender.Session.Response.TeamInviteRes.Errormsg = "对方已有队伍";
                sender.SendResponse();
                return;
            }

            Log.InfoFormat("Forward TeamInvite Request: FromId:{0} FromName:{1} ToId:{2} ToName:{3}", request.FromId, request.FromName, request.ToId, request.ToName);
            target.Session.Response.TeamInviteReq = request;
            target.SendResponse();
        }

        private void OnTeamInviteResponse(NetConnection<NetSession> sender, TeamInviteResponse response)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnTeamInviteResponse:character:{0} Result:{1} FromId:{2} ToId:{3}", character.Id, response.Result, response.Request.FromId, response.Request.ToId);
            if (response.Result == Result.Success)
            {//接受了组队请求
                var requester = SessionManager.Instance.GetSession(response.Request.FromId);
                if (requester == null)
                {
                    sender.Session.Response.TeamInviteRes = response;
                    sender.Session.Response.TeamInviteRes.Result = Result.Failed;
                    sender.Session.Response.TeamInviteRes.Errormsg = "请求者已下线";
                    sender.SendResponse();
                }
                else
                {
                    TeamManager.Instance.AddTeamMember(requester.Session.Character, character);
                    sender.Session.Response.TeamInviteRes = response;
                    sender.SendResponse();
                    requester.Session.Response.TeamInviteRes = response;
                    requester.SendResponse();
                }
            }
            else //拒绝了组队请求
            {
                var requester = SessionManager.Instance.GetSession(response.Request.FromId);
                if (requester != null)
                {
                    requester.Session.Response.TeamInviteRes = response;
                    requester.SendResponse();
                }
            }
        }

        private void TeamLeaveRequest(NetConnection<NetSession> sender, TeamLeaveRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("TeamLeaveRequest: character:{0} TeamId:{1} : {2}", character.Id, request.TeamId,request.characterId);
            Team team = character.Team;
            character.Team.Leave(character);
            sender.Session.Response.TeamLeave = new TeamLeaveResponse();
            sender.Session.Response.TeamLeave.characterId = character.Id;
            sender.Session.Response.TeamLeave.Result = Result.Success;
            sender.Session.Response.TeamLeave.Errormsg = "退出成功";
            sender.SendResponse();
            team?.MemberLeaveNotify();

        }
    }
}
