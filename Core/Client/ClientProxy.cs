using Core.Network;
namespace Core.Client
{
    public class ClientProxy : IClient
    {
        public uint Id { get; set; }
        public ISession NetSession { get; private set; }
    }
}
