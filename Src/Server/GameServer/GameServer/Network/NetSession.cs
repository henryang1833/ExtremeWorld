using GameServer;
using GameServer.Entities;
using SkillBridge.Message;
using GameServer.Services;
using GameServer.Network;
using GameServer.Managers;

namespace Network
{
    public class NetSession : INetSession
    {
        public TUser User { get; set; }
        public Character Character { get; set; }
        public NEntity Entity { get; set; }
        NetMessage response;
        public IPostResponse PostResponser { get; set; }
        public NetMessageResponse Response
        {
            get
            {
                if (response == null)
                    response = new NetMessage();
                if (response.Response == null)
                    response.Response = new NetMessageResponse();
                return response.Response;
            }
        }

        public byte[] GetResponse()
        {
            if(response != null)
            {
                if(PostResponser!=null)
                    this.PostResponser.PostProcess(Response);
                
                byte[] data = PackageHandler.PackMessage(response);
                response = null;
                return data;
            }
            return null;
        }

        internal void Disconnected()
        {
            this.PostResponser = null;
            if (this.Character != null)
            {
                SessionManager.Instance.RemoveSession(this.Character.Id); //自己添加的
                UserService.Instance.CharacterLeave(this.Character);
            }
            
        }
    }
}
