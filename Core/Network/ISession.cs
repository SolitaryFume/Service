using System;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using Core.Client;

namespace Core.Network
{
    public interface ISession
    {
        public void Initialize(TcpClient client, Action<byte[]> messageDataHander);
        public void StartReceive(CancellationToken cancellationToken);
        public void SendMessage(byte[] data);
        public void Close();
    }
}