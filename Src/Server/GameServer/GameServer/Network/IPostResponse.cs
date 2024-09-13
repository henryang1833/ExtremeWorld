using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;

namespace GameServer.Network
{
    public interface IPostResponse
    {
       void PostProcess(NetMessageResponse response);
    }
}
