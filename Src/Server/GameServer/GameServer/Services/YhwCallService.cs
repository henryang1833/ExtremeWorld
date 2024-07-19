using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using SkillBridge.Message;
namespace GameServer.Services
{
    class YhwCallService : Singleton<YhwCallService>
    {
        public void Init()
        {

        }

        public void Start()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<YhwRequest>(this.OnYhwRequest);
            Log.Warning("YhwCallService Started");
        }

        private void OnYhwRequest(NetConnection<NetSession> sender, YhwRequest message)
        {
            Log.InfoFormat("YhwRequest:YhwCall:{0}", message.Call);
        }

        public void Stop()
        {

        }
    }
}
