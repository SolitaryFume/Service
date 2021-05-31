using Core.Network;
using Proto;
using Microsoft.Extensions.DependencyInjection;
using Service;

namespace Core.Client
{
    public class ClientProxy : IClient
    {
        public uint Id { get; set; }
        public ISession NetSession { get; private set; }

        public ClientProxy()
        {
            
        }

        public void OnMessage(INetMessage msg)
        {
            IOC.Root.GetService<MessageHanderService>().Dispatch(msg, this);
        }

        public void Init(ISession session)
        {
            NetSession = session;
        }
    }
}
