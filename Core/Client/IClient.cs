using Core.Network;
namespace Core.Client
{
    public interface IClient
    { 
        public uint Id { get; set; }
        public ISession NetSession { get; }
    }
}
