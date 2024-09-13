using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using SkillBridge.Message;
using GameServer.Entities;
using Common;
namespace GameServer.Services
{
    class BagService : Singleton<BagService>
    {
        public void Init()
        {

        }

        public BagService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<BagSaveRequest>(this.OnBagSave);   
        }

        void OnBagSave(NetConnection<NetSession> sender, BagSaveRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("BagSaveRequest: character:{0}:unlocked:{1}", character.Id, request.BagInfo.Unlocked);

            if (request.BagInfo != null)
            {
                character.Data.Bag.Items = request.BagInfo.Items;
                DBService.Instance.Save();
            }
        }
    }

}
