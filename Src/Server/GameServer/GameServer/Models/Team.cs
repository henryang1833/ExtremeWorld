using System;
using System.Collections.Generic;
using GameServer.Entities;
using SkillBridge.Message;
using Common;
using GameServer.Managers;
using Network;
namespace GameServer.Models
{
    public class Team
    {
        public int Id;
        public Character Leader;

        public List<Character> Members = new List<Character>();

        public int timestamp; //通过时间戳来标记队伍是否发生变化

        public Team(Character leader)
        {
            this.AddMember(leader);
        }

        public void AddMember(Character member)
        {
            if (this.Members.Count == 0)
            {
                this.Leader = member;
            }
            this.Members.Add(member);
            member.Team = this;
            timestamp = Time.timestamp;
        }

        public void Leave(Character member)
        {
            Log.InfoFormat("Leave Team:{0}:{1}", member.Id, member.Info.Name);
            this.Members.Remove(member);
            if (member == Leader)
            {
                if (this.Members.Count > 0)
                    this.Leader = this.Members[0];
                else
                    this.Leader = null;
            }
            member.Team = null;
            timestamp = Time.timestamp;

        }

        public void PostProcess(NetMessageResponse message)
        {
            if (message.TeamInfo == null)
            {
                message.TeamInfo = new TeamInfoResponse();
                message.TeamInfo.Result = Result.Success;
                message.TeamInfo.Team = new NTeamInfo();
                message.TeamInfo.Team.Id = this.Id;
                message.TeamInfo.Team.Leader = this.Leader.Id;
                foreach (var member in this.Members)
                {
                    message.TeamInfo.Team.Members.Add(member.GetBasicInfo());
                }
            }
        }

        public void MemberLeaveNotify()
        {
            foreach (var member in this.Members)
            {
                NetConnection<NetSession> sender = SessionManager.Instance.GetSession(member.Id);
                Object temp = sender.Session.Response;
                if (sender != null)
                    sender.SendResponse();
            }
        }
    }
}
