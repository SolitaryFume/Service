using System;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using Core.Client;
using Proto;

namespace Core.Network
{
    public interface ISession : IDisposable
    {
        public void Init(Socket socket, Action<INetMessage> onMsg);
        public void StartReceive(CancellationToken cancellationToken = default);
        public void SendMessage(INetMessage data);
    }
}