using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Data;
using GameServer.Managers;
using GameServer.Entities;
using SkillBridge.Message;
using Network;
namespace GameServer.Services
{
    class QuestService :Singleton<QuestService>
    {

        public void Init()
        {

        }
        public QuestService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<QuestAcceptRequest>(this.OnQuestAccept);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<QuestSubmitRequest>(this.OnQuestSubmit);
        }

        private void OnQuestAccept(NetConnection<NetSession> sender, QuestAcceptRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("QuestAcceptRequest:character:{0}:QuestId:{1}", character.Id, request.QuestId);

            sender.Session.Response.questAccept = new QuestAcceptResponse();
            Result result = character.QuestManager.AcceptQuest(sender, request.QuestId);
            sender.Session.Response.questAccept.Result = result;
            sender.SendResponse();
        }

        private void OnQuestSubmit(NetConnection<NetSession> sender, QuestSubmitRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("QuestAcceptSubmit:character:{0}:QuestId:{1}", character.Id, request.QuestId);

            sender.Session.Response.questSubmit = new QuestSubmitResponse();
            Result result = character.QuestManager.SubmitQuest(sender, request.QuestId);
            sender.Session.Response.questSubmit.Result = result;
            sender.SendResponse();
        }

        
    }
}
