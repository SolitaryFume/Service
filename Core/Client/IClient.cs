using Core.Network;
using Proto;

namespace Core.Client
{
    public interface IClient
    { 
        public uint Id { get; set; }
        public ISession NetSession { get; }
        public void Init(ISession session);

        void OnMessage(INetMessage msg);
    }
}
