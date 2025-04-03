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
        public IPostResponse PostResponser { get; set; }

        private NetMessage _pendingNetMessage;
        public NetMessageResponse Response
        {
            get
            {
                if (_pendingNetMessage == null)
                    _pendingNetMessage = new NetMessage();
                if (_pendingNetMessage.Response == null)
                    _pendingNetMessage.Response = new NetMessageResponse();
                return _pendingNetMessage.Response;
            }
        }

        public byte[] GetResponse()
        {
            if(_pendingNetMessage != null)
            {
                if(PostResponser!=null)
                    this.PostResponser.PostProcess(Response);
                
                byte[] data = PackageHandler.PackMessage(_pendingNetMessage);
                _pendingNetMessage = null;
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
