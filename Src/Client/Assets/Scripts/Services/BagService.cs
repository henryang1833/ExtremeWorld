using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Network;
using SkillBridge.Message;
using UnityEngine;

namespace Services
{
    class BagService : Singleton<BagService>, IDisposable
    {
        public BagService()
        {
            MessageDistributer.Instance.Subscribe<BagSaveResponse>(this.OnBagSave);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<BagSaveResponse>(this.OnBagSave);
        }

        private void OnBagSave(object sender, BagSaveResponse message)
        {
            if(message.Result == Result.Failed)
            {
                MessageBox.Show(message.Errormsg, "背包保存失败");
            }
            else
            {
                Debug.Log(message.Errormsg);
            }
        }

        public void SendBagSaveRequest(NBagInfo bagInfo)
        {
            Debug.Log("SendBagSaveRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.bagSave = new BagSaveRequest();
            message.Request.bagSave.BagInfo = bagInfo;
            NetClient.Instance.SendMessage(message);
        }
    }
}
