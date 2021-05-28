using System.Net.Sockets;

namespace Core.Network
{
    public interface ITcpServer
    {
        void Start(string host, int port);
        void AcceptAsync();
        void OnAccept(Socket socket);
    }
}